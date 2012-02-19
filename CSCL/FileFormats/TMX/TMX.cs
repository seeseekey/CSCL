//
//  TMX.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
using CSCL.Exceptions;

namespace CSCL.FileFormats.TMX
{
	/// <summary>
	/// Klasse für das TMX Format welches unteranderem von Tiled benutzt wird
	/// Es werden auch nur rechteckige Karten unterstützt
	/// Zur Zeit ist nur der Lesezugriff implementiert
	/// </summary>
	public class TMX
	{
		static gtImage.PooledLoader pooledLoader=new gtImage.PooledLoader(100);

		#region Datenstrukturen
		public class Tile
		{
			public Tile()
			{
				Properties=new List<Property>();
			}

			public string ID;
			public List<Property> Properties;
		}

		public class TilesetData: IComparable
		{
			public TilesetData()
			{
				Tiles=new List<Tile>();
			}

			public string name;
			public int firstgid;
			public int tilewidth;
			public int tileheight;

			public string imgsource;
			public gtImage img;

			public List<Tile> Tiles;

			#region IComparable Members
			public int CompareTo(object obj)
			{
				TilesetData tmp=(TilesetData)obj;
				if(tmp.firstgid<this.firstgid)
				{
					return 1;
				}
				else if(tmp.firstgid==this.firstgid)
				{
					return 0;
				}
				else
				{
					return -1;
				}
			}
			#endregion
		}

		public class LayerData
		{
			public TilesetData[,] tilesetmap;
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

		public string MapVersion
		{
			get;
			private set;
		}

		public string Orientation
		{
			get;
			private set;
		}

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}

		public int TileWidth
		{
			get;
			private set;
		}

		public int TileHeight
		{
			get;
			private set;
		}

		public List<TilesetData> Tilesets { get; private set; }
		public List<LayerData> Layers { get; private set; }
		public List<Objectgroup> ObjectLayers { get; private set; }
		public List<Property> Properties { get; private set; }

		public Property GetProperty(string name)
		{
			foreach(Property i in Properties)
			{
				if(i.Name==name)
				{
					return i;
				}
			}

			return null;
		}

		public void RemoveGidsFromLayerData()
		{
			foreach(TMX.LayerData ld in Layers)
			{
				ld.tilesetmap=new TilesetData[ld.width, ld.height];

				for(int y=0; y<ld.height; y++)
				{
					for(int x=0; x<ld.width; x++)
					{
						int TileNumber=ld.data[x, y];

						TMX.TilesetData ts=GetTileset(TileNumber);
						ld.tilesetmap[x, y]=ts;

						int tilesetNumber=TileNumber-ts.firstgid;
						ld.data[x, y]=tilesetNumber;
					}
				}
			}
		}

		public void ReplaceTilesetInTilesetMap(TMX.TilesetData oldTileset, TMX.TilesetData newTileset)
		{
			foreach(TMX.LayerData ld in Layers)
			{
				for(int y=0; y<ld.height; y++)
				{
					for(int x=0; x<ld.width; x++)
					{
						TMX.TilesetData ts=ld.tilesetmap[x, y];

						if(ts==oldTileset)
						{
							ld.tilesetmap[x, y]=newTileset;
						}
					}
				}
			}
		}

		public void AddsGidsToLayerData()
		{
			foreach(TMX.LayerData ld in Layers)
			{
				for(int y=0; y<ld.height; y++)
				{
					for(int x=0; x<ld.width; x++)
					{
						int TileNumber=ld.data[x, y];

						TMX.TilesetData ts=ld.tilesetmap[x, y];
	
						int tilesetNumber=TileNumber+ts.firstgid;
						ld.data[x, y]=tilesetNumber;
					}
				}
			}
		}

		public TMX(string filename)
		{
			Open(filename, true);
		}

		public TMX(string filename, bool loadTilesets)
		{
			Open(filename, loadTilesets);
		}

		void Open(string filename, bool loadTilesets)
		{
			//Datei öffnen
			Tilesets=new List<TilesetData>();
			Layers=new List<LayerData>();
			ObjectLayers=new List<Objectgroup>();
			Properties=new List<Property>();

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

			#region Properties auslesen
			xnl=FileData.Document.SelectNodes("/map/properties");

			foreach (XmlNode j in xnl)
			{
				XmlNodeList subnodes=j.SelectNodes("child::property");

				foreach(XmlNode pNode in subnodes)
				{
					string name=pNode.Attributes[0].Name;
					string value=pNode.Attributes[0].Value;

					Properties.Add(new Property(pNode));
				}
			}
			#endregion

			#region Tilesets ermitteln
			xnl=FileData.Document.SelectNodes("/map/tileset");

			foreach (XmlNode j in xnl)
			{
				//Tilesets
				TilesetData ts=new TilesetData();

				ts.imgsource=j.SelectNodes("child::image")[0].Attributes[0].Value; //Image Source für den Layer
				string imgsourceComplete=FileSystem.GetPath(filename)+ts.imgsource;

				//Tiles laden, wenn vorhanden
				XmlNodeList nodelist=j.SelectNodes("child::tile");

				foreach(XmlNode tileXml in nodelist)
				{
					Tile tile=new Tile();
					tile.ID=tileXml.Attributes["id"].Value.ToString();

					xnl=tileXml.SelectNodes("child::properties");

					foreach(XmlNode jProp in xnl)
					{
						XmlNodeList subnodes=jProp.SelectNodes("child::property");

						foreach(XmlNode pNode in subnodes)
						{
							tile.Properties.Add(new Property(pNode));
						}
					}

					ts.Tiles.Add(tile);
				}

				//Tilebildl laden
				if(loadTilesets)
				{
					try
					{
						ts.img=pooledLoader.FromFile(imgsourceComplete);
					}
					catch(FileNotFoundException ex)
					{
						throw new TilesetNotExistsException(ex.Message);
					}
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

				if(encoding!="base64")
				{
					throw (new NotImplementedException("Weitere Codierungsarten sind noch nicht implementiert!"));
				}

				if(compression!="gzip")
				{
					throw (new NotSupportedCompressionException("Weitere Kompressionsverfahren sind noch nicht implementiert!"));
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
		/// Gibt das Tileset zurück in welchem 
		/// das Tile ist
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public TilesetData GetTileset(int number)
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

			int tsPosX=tilesetNumber%tilesPerLine;
			int tsPosY=tilesetNumber/tilesPerLine;

			int tilesetPixelStartX=tsPosX*ts.tilewidth;
			int tilesetPixelStartY=tsPosY*ts.tileheight;

			try
			{
				return ts.img.GetSubImage((uint)tilesetPixelStartX, (uint)tilesetPixelStartY, (uint)ts.tilewidth, (uint)ts.tileheight);
			}
			catch
			{
				return new gtImage((uint)ts.tilewidth, (uint)ts.tileheight, gtImage.Format.RGBA);
			}
		}

		public void Save(string filename)
		{
			XmlData fileData=new XmlData();

			#region Root speichern und Attribute anhängen
			XmlNode root=fileData.AddRoot("map");

			fileData.AddAttribute(root, "version", MapVersion);
			fileData.AddAttribute(root, "orientation", Orientation);

			fileData.AddAttribute(root, "width", Width);
			fileData.AddAttribute(root, "height", Height);

			fileData.AddAttribute(root, "tilewidth", TileWidth);
			fileData.AddAttribute(root, "tileheight", TileHeight);
			#endregion

			#region Properties speichern
			if(Properties.Count>0)
			{
				XmlNode properties=fileData.AddElement(root, "properties");

				foreach(Property prop in Properties)
				{
					XmlNode propertyXml=fileData.AddElement(properties, "property");
					fileData.AddAttribute(propertyXml, "name", prop.Name);
					fileData.AddAttribute(propertyXml, "value", prop.Value);
				}
			}
			#endregion

			#region Tilesets
			foreach(TilesetData tileset in Tilesets)
			{
				XmlNode tilesetXml=fileData.AddElement(root, "tileset");
				fileData.AddAttribute(tilesetXml, "firstgid", tileset.firstgid);
				fileData.AddAttribute(tilesetXml, "name", tileset.name);
				fileData.AddAttribute(tilesetXml, "tilewidth", tileset.tilewidth);
				fileData.AddAttribute(tilesetXml, "tileheight", tileset.tileheight);

				XmlNode imageTag=fileData.AddElement(tilesetXml, "image");
				fileData.AddAttribute(imageTag, "source", tileset.imgsource);

				foreach(Tile tile in tileset.Tiles)
				{
					XmlNode tileTag=fileData.AddElement(tilesetXml, "tile");
					fileData.AddAttribute(tileTag, "id", tile.ID);

					if(tile.Properties.Count>0)
					{
						XmlNode properties=fileData.AddElement(tileTag, "properties");

						foreach(Property prop in tile.Properties)
						{
							XmlNode propertyXml=fileData.AddElement(properties, "property");
							fileData.AddAttribute(propertyXml, "name", prop.Name);
							fileData.AddAttribute(propertyXml, "value", prop.Value);
						}
					}
				}

			}
			#endregion

			#region Layer
			foreach(LayerData layer in Layers)
			{
				XmlNode layerXml=fileData.AddElement(root, "layer");
				fileData.AddAttribute(layerXml, "name", layer.name);
				fileData.AddAttribute(layerXml, "width", layer.width);
				fileData.AddAttribute(layerXml, "height", layer.height);

				XmlNode dataTag=fileData.AddElement(layerXml, "data", ConvertLayerDataToString(layer));
				fileData.AddAttribute(dataTag, "encoding", "base64");
				fileData.AddAttribute(dataTag, "compression", "gzip");
			}
			#endregion

			#region Objectlayer
			foreach(Objectgroup objGroup in ObjectLayers)
			{
				XmlNode objGroupXml=fileData.AddElement(root, "objectgroup");
				fileData.AddAttribute(objGroupXml, "name", objGroup.Name);
				fileData.AddAttribute(objGroupXml, "width", objGroup.Width);
				fileData.AddAttribute(objGroupXml, "height", objGroup.Height);
				fileData.AddAttribute(objGroupXml, "x", objGroup.X);
				fileData.AddAttribute(objGroupXml, "y", objGroup.Y);

				foreach(Object obj in objGroup.Objects)
				{
					XmlNode objXml=fileData.AddElement(objGroupXml, "object");
					fileData.AddAttribute(objXml, "name", obj.Name);
					fileData.AddAttribute(objXml, "type", obj.Type);
					fileData.AddAttribute(objXml, "x", obj.X);
					fileData.AddAttribute(objXml, "y", obj.Y);
					fileData.AddAttribute(objXml, "width", obj.Width);
					fileData.AddAttribute(objXml, "height", obj.Height);

					XmlNode objPropertiesXml=fileData.AddElement(objXml, "properties");

					foreach(Property objProp in obj.Properties)
					{
						XmlNode propertyXml=fileData.AddElement(objPropertiesXml, "property");
						fileData.AddAttribute(propertyXml, "name", objProp.Name);
						fileData.AddAttribute(propertyXml, "value", objProp.Value);
					}
				}
			}
			#endregion

			fileData.Save(filename);
		}

		private string ConvertLayerDataToString(LayerData layer)
		{
			//
			MemoryStream ms=new MemoryStream();
			BinaryWriter bw=new BinaryWriter(ms);

			for(int y=0; y<layer.height; y++)
			{
				for(int x=0; x<layer.width; x++)
				{
					bw.Write(layer.data[x, y]);
				}
			}

			//Gzip Decodierung
			byte[] layerdataCompressed=gzip.Compress(ms.ToArray());

			//Base64 Encodierung
			string layerdataEncoded=CSCL.Crypto.Encoding.Base64.Encode(layerdataCompressed);
			return layerdataEncoded;
		}

		public gtImage Render()
		{
			return Render(Width*TileWidth, Height*TileHeight);
		}

		public gtImage Render(string onlyLayer)
		{
			return Render(Width*TileWidth, Height*TileHeight, onlyLayer);
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
						if(number<=0) continue; //Kein Tile zugewiesen //TODO was genau meint eine Zahl kleiner 0 genau
						gtImage Tile=GetTile(number);

						//Korrekturfaktor für Tiles welche breiter bzw. 
						//höher sind als normal
						int CorFactorX=0; //Korrekturfaktor für x wird nicht benötigt (denke ich)
						//int CorFactorX=(int)(Tile.Width-TileWidth);
						int CorFactorY=(int)(Tile.Height-TileHeight);

						ret.Draw(x*TileWidth-CorFactorX, y*TileHeight-CorFactorY, Tile, true);
					}
				}
			
			}

			return ret;
		}
	}
}
