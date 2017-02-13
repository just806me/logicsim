using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace VinCAD.WindowsUI
{
    public static class DrawableSchemeStorage
	{
		public static DrawableScheme Load(string json)
		{
            #region arguments check

            if (string.IsNullOrEmpty(json))
				throw new ArgumentNullException(nameof(json));

            #endregion

            var scheme = JsonConvert.DeserializeObject<DrawableScheme>(json);

            foreach (var line in scheme.Lines)
            {
                line.Start = scheme.Elements.FirstOrDefault(e => e.Name == line.StartName);
                line.End = scheme.Elements.FirstOrDefault(e => e.Name == line.EndName);
            }

			return scheme;
		}

		public static DrawableScheme Load(Stream stream)
		{
            #region arguments check

            if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (!stream.CanRead)
				throw new ArgumentException("The stream must be readable.", nameof(stream));

            #endregion

            using (var streamReader = new StreamReader(stream))
                return Load(streamReader.ReadToEnd());
        }

		public static string Save(DrawableScheme scheme)
		{
            #region arguments check

            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            #endregion

            return JsonConvert.SerializeObject(scheme);
		}

		public static void Save(DrawableScheme scheme, Stream stream)
		{
            #region arguments check

            if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (!stream.CanWrite)
				throw new ArgumentException("The stream must be writeable.", nameof(stream));

            #endregion

            using (var streamWriter = new StreamWriter(stream))
                streamWriter.Write(Save(scheme));
		}
	}
} 