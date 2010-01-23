using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CSCL
{
	/// <summary>
	/// Klasse für die generische Serialization von Daten
	/// </summary>
    public class Serialization
    {
        #region Generisches, komprimiertes, serialisieren von Objekten und Zurück
        /// <summary>
        /// Generisches, komprimiertes, serialisieren von Objekten
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void Save<T>(T obj, string path)
        {
            DirectoryInfo di = new FileInfo(path).Directory;
            if(obj != null && di != null && di.Exists)
            {
                using(FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using(GZipStream zip = new GZipStream(fs, CompressionMode.Compress))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        bf.Serialize(zip, obj);
                    }
                }
            }
        }

        /// <summary>
        /// Generisches, komprimiertes, deserialisieren von Objekten
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path)
        {
            if(path != null && File.Exists(path))
            {
                using(FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using(GZipStream zip = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        return (T)bf.Deserialize(zip);
                    }
                }
            }
            return default(T);
        }
        #endregion

        #region Serialisation in ByteArray und in Typ zurück
        public static byte[] CompressTypeToByteArray<T>(T input) where T: class
        {
            byte[] uncompressed = SerializeTypeToByteArray(input);
            return CompressByteArray(uncompressed);
        }

        public static T DecompressByteArrayToType<T>(byte[] buffer) where T: class
        {
            byte[] decompressed = DecompressByteArray(buffer);
            return DeserializeByteArrayToType<T>(
                decompressed);
        }

        public static byte[] DecompressByteArray(byte[] compressedByteArray)
        {
            MemoryStream compressedStream = new MemoryStream(compressedByteArray);
            DeflateStream compressedzipStream =
                new DeflateStream(compressedStream, CompressionMode.Decompress, true);
            MemoryStream decompressedStream = new MemoryStream();
            const int blockSize = 1024;

            byte[] buffer = new byte[blockSize];
            int bytesRead;
            while((bytesRead = compressedzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                decompressedStream.Write(buffer, 0, bytesRead);
            }

            compressedzipStream.Close();
            decompressedStream.Position = 0;
            byte[] decompressedArray = decompressedStream.ToArray();
            decompressedStream.Close();
            decompressedStream.Dispose();
            compressedzipStream.Close();
            compressedzipStream.Dispose();
            return decompressedArray;
        }

        public static byte[] CompressByteArray(byte[] uncompressedByteArray)
        {
            MemoryStream msCompressed = new MemoryStream();
            DeflateStream compressedzipStream =
                new DeflateStream(msCompressed, CompressionMode.Compress, true);

            compressedzipStream.Write(uncompressedByteArray, 0, uncompressedByteArray.Length);
            compressedzipStream.Flush();
            compressedzipStream.Close();

            byte[] compressedByteArray = msCompressed.ToArray();
            msCompressed.Close();
            msCompressed.Dispose();

            return compressedByteArray;
        }

        public static T DeserializeStreamToType<T>(Stream stream) where T: class
        {
            BinaryFormatter bf = new BinaryFormatter();
            T users = bf.Deserialize(stream) as T;
            return users;
        }

        public static T DeserializeByteArrayToType<T>(byte[] stream) where T: class
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(stream);
            T users = bf.Deserialize(ms) as T;
            ms.Close();
            ms.Dispose();
            return users;
        }

        public static byte[] SerializeTypeToByteArray<T>(T input) where T: class
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, input);

            byte[] array = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return array;
        }
        #endregion

		#region XML Serialisierer
		public static byte[] SerializeTypeToXmlByteArray<T>(T input) where T : class
		{
			MemoryStream ms=new MemoryStream();
			XmlSerializer xmlSer=new XmlSerializer(typeof(T));
			xmlSer.Serialize(ms, input);
			byte[] array=ms.ToArray();
			ms.Close();
			ms.Dispose();
			return array;
		}

		public static T DeserializeXmlByteArrayToType<T>(byte[] stream) where T : class
		{
			XmlSerializer xmlSer=new XmlSerializer(typeof(T));
			MemoryStream ms=new MemoryStream(stream);
			T users=xmlSer.Deserialize(ms) as T;
			ms.Close();
			ms.Dispose();
			return users;
		}
		#endregion
    }
}
