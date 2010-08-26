using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CSCL;
using System.Xml;
using System.IO;
using System.IO.Compression;
using CSCL.Crypto;
using CSCL.Compression;
using CSCL.Graphic;

namespace CSCL.FileFormats.TMX
{
	/// <summary>
	/// Klasse für das TMX Format welches unteranderem von Tiled benutzt wird
	/// Es werden auch nur rechteckige Karten unterstützt
	/// Zur Zeit ist nur der Lesezugriff implementiert
	/// </summary>
	public class TMX
	{
		#region Datenstrukturen
		public class TilesetData
		{
			public string name;
			public int firstgid;
			public int tilewidth;
			public int tileheight;

			public string imgsource;
			public gtImage img;
		}

		public class LayerData
		{
			public string name;
			public int width;
			public int height;
			public int[,] data;

			public void SaveLayer(string filename)
			{
				StreamWriter sw=new StreamWriter(filename);

				for (int y=0; y<height; y++)
				{
					string line="";

					for (int x=0; x<width; x++)
					{
						line+=data[x, y].ToString()+"\t";
					}

					sw.WriteLine(line);
				}

				sw.Close();
			}
		}
		#endregion

		XmlData FileData;

		string MapVersion;
		string Orientation;
		int Width;
		int Height;
		int TileWidth;
		int TileHeight;

		public List<TilesetData> Tilesets { get; private set; }
		public List<LayerData> Layers { get; private set; }
		public List<Objectgroup> ObjectLayers { get; private set; }

		public void Open(string filename)
		{
			Open(filename, true);
		}

		public void Open(string filename, bool loadTilesets)
		{
			//Datei öffnen
			Tilesets=new List<TilesetData>();
			Layers=new List<LayerData>();
			ObjectLayers=new List<Objectgroup>();

			//XMLdata öffnen
            FileData=new XmlData(filename);

			#region MapsInfo ermitteln
			XmlNodeList xnl=FileData.Document.SelectNodes("/map");

			MapVersion=xnl[0].Attributes["version"].Value;
			Orientation=xnl[0].Attributes["orientation"].Value;

			Width=Convert.ToInt32(xnl[0].Attributes["width"].Value);
			Height=Convert.ToInt32(xnl[0].Attributes["height"].Value);

			TileWidth=Convert.ToInt32(xnl[0].Attributes["tilewidth"].Value);
			TileHeight=Convert.ToInt32(xnl[0].Attributes["tileheight"].Value);
			#endregion

			#region Tilesets ermitteln
			xnl=FileData.Document.SelectNodes("/map/tileset");

			foreach (XmlNode j in xnl)
			{
				//Tilesets
				TilesetData ts=new TilesetData();

				ts.imgsource=j.SelectNodes("child::image")[0].Attributes[0].Value; //Image Source für den Layer
				string imgsourceComplete=FileSystem.GetPath(filename)+ts.imgsource;

				if(loadTilesets)
				{
					ts.img=gtImage.FromFile(imgsourceComplete);
				}

				//Attrribute
				ts.name=j.Attributes["name"].Value; 
				ts.firstgid=Convert.ToInt32(j.Attributes["firstgid"].Value);
				ts.tilewidth=Convert.ToInt32(j.Attributes["tilewidth"].Value);
				ts.tileheight=Convert.ToInt32(j.Attributes["tileheight"].Value);

				Tilesets.Add(ts);
			}
			#endregion

			#region Layers ermitteln
			xnl=FileData.Document.SelectNodes("/map/layer");

			foreach (XmlNode j in xnl) //pro layer
			{
				//Layer
				LayerData lr=new LayerData();

				//Attribute
				lr.name=j.Attributes["name"].Value;
				lr.width=Convert.ToInt32(j.Attributes["width"].Value);
				lr.height=Convert.ToInt32(j.Attributes["height"].Value);

				//Layerdaten
				// Attribute werden als "<data encoding="base64" compression="gzip">" angenommen
				string encoding=j["data"].Attributes["encoding"].Value;
				string compression=j["data"].Attributes["compression"].Value;

				if (encoding!="base64"||compression!="gzip")
				{
					throw(new NotImplementedException("Weitere Codierungsarten sind noch nicht implementiert!"));
				}

				//Base64 Encodierung
				string layerdataBase64Compressed=j.SelectNodes("child::data")[0].InnerText;
				layerdataBase64Compressed=layerdataBase64Compressed.TrimStart('\n');
				layerdataBase64Compressed=layerdataBase64Compressed.Trim();
				byte[] layerdataCompressed=CSCL.Crypto.Encoding.Base64.Decode(layerdataBase64Compressed); 

				//Gzip Decodierung
				byte[] layerdataDecompressed=gzip.Decompress(layerdataCompressed);

				//Interpretieren der Codierten Daten
				lr.data=new int[lr.width, lr.height];
				//int[,] zelle=new int[4, 3];
				BinaryReader br=new BinaryReader(new MemoryStream(layerdataDecompressed));

				for (int y=0; y<lr.height; y++)
				{
					for (int x=0; x<lr.width; x++)
					{
						lr.data[x, y]=br.ReadInt32();
					}
				}
				
				Layers.Add(lr);
			}
			#endregion

			#region Objektlayer ermitteln
			xnl=FileData.Document.SelectNodes("/map/objectgroup");

			foreach(XmlNode j in xnl) //pro layer
			{
				ObjectLayers.Add(new Objectgroup(j));
			}
			#endregion
		}

		/// <summary>
		/// Gibt die maximale zu erwartende Mapfläche an
		/// </summary>
		/// <returns></returns>
		public Size GetMaxMapSite()
		{
			int w=0;
			int h=0;

			foreach(LayerData i in Layers)
			{
				if(i.width>w) w=i.width;
				if(i.height>h) h=i.height;
			}

			return new Size(w, h);
		}

		/// <summary>
		/// Gibt das Tileset zurück in welchem 
		/// das Tile ist
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		private TilesetData GetTileset(int number)
		{
			TilesetData ret=new TilesetData();

			foreach(TilesetData i in Tilesets)
			{
				if(number>=i.firstgid)
				{
					ret=i;
				}
			}

			return ret;
		}

		public gtImage GetTile(int number)
		{
			TilesetData ts=GetTileset(number);
			int tilesetNumber=number-ts.firstgid;

			int tilesPerLine=(int)(ts.img.Width/TileWidth);
			//int tilesPerCol=ts.img.Height/TileHeight;

			int tsPosX=tilesetNumber%tilesPerLine;
			int tsPosY=tilesetNumber/tilesPerLine;

			int tilesetPixelStartX=tsPosX*TileWidth;
			int tilesetPixelStartY=tsPosY*TileHeight;

			//gtImage ret=new gtImage(TileWidth, TileHeight);

			return ts.img.GetSubImage((uint)tilesetPixelStartX, (uint)tilesetPixelStartY, (uint)ts.tilewidth, (uint)ts.tileheight);
		}

		public gtImage Render()
		{
			Size size = GetMaxMapSite();
			return Render(size.Width*TileWidth, size.Height*TileHeight);
		}

		public gtImage Render(string onlyLayer)
		{
			Size size=GetMaxMapSite();
			return Render(size.Width*TileWidth, size.Height*TileHeight, onlyLayer);
		}

		private gtImage Render(int width, int height)
		{
			return Render(width, height, "");
		}

		private gtImage Render(int width, int height, string onlyLayer)
		{
			gtImage ret=new gtImage((uint)width, (uint)height, gtImage.Format.RGBA);
			ret=ret.InvertAlpha();

			foreach(LayerData i in Layers)
			{
				if (onlyLayer=="")
				{
					if (i.name=="Collision") continue; //Collision Layer überspringen
				}
				else
				{
					if (i.name!=onlyLayer) continue;
				}

				//Für jedes Tile in X bzw Y Richtung
				for(int y=0; y<i.height; y++)
				{
					for(int x=0; x<i.width; x++)
					{
						int number = i.data[x,y];
						if(number==0) continue; //Kein Tile zugewiesen
						gtImage Tile=GetTile(number);

						//Korrekturfaktor für Tiles welche breiter bzw. 
						//höher sind als normal
						int CorFactorX=(int)(Tile.Width-TileWidth);
						int CorFactorY=(int)(Tile.Height-TileHeight);

						ret.Draw(x*TileWidth-CorFactorX, y*TileHeight-CorFactorY, Tile, true);
					}
				}
			
			}

			return ret;
		}
	}
}
