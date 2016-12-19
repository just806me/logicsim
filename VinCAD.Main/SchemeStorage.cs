using Newtonsoft.Json;
using System;
using System.IO;

namespace VinCAD.Main
{
    public static class SchemeStorage
    {
        private const string Empty = "{'inputs':[],'outputs':[],'components':[]}";

        public static Scheme Load(string json)
        {
            #region arguments check

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json));

            #endregion

            return JsonConvert.DeserializeObject<Scheme>(json);
        }

        public static Scheme Load(Stream stream)
        {
            #region arguments check

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("The stream must be readable.", nameof(stream));

            #endregion

            using (StreamReader reader = new StreamReader(stream))
                return Load(reader.ReadToEnd());
        }

        public static Scheme LoadEmpty() => Load(Empty);

        public static string Save(Scheme scheme)
        {
            #region arguments check

            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            #endregion

            return JsonConvert.SerializeObject(scheme);
        }

        public static void Save(Scheme scheme, Stream stream)
        {
            #region arguments check

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite)
                throw new ArgumentException("The stream must be writeable.", nameof(stream));

            #endregion

            using (var writer = new StreamWriter(stream))
                writer.Write(Save(scheme));
        }
    }
}
