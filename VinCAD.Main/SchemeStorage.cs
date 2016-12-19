using Newtonsoft.Json;
using System;
using System.IO;

#if DEBUG
using VinCAD.Logger;
#endif

namespace VinCAD.Main
{
    public static class SchemeStorage
    {
        private const string Empty = "{'inputs':[],'outputs':[],'components':[]}";

        public static Scheme Load(string json)
        {
#if DEBUG
            Log.Method(
                new MethodInfo { Name = "SchemeStorage::Load", Type = typeof(Scheme) },
                new ArgumentInfo { Name = nameof(json), Type = json.GetType(), Data = json }
            );
#endif

            #region arguments check

            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException(nameof(json));

            #endregion

            return JsonConvert.DeserializeObject<Scheme>(json);
        }

        public static Scheme Load(Stream stream)
        {
#if DEBUG
            Log.Method(
                new MethodInfo { Name = "SchemeStorage::Load", Type = typeof(Scheme) },
                new ArgumentInfo { Name = nameof(stream), Type = stream.GetType(), Data = new { stream.CanRead, stream.CanSeek, stream.CanWrite, stream.Length, stream.Position } }
            );
#endif

            #region arguments check

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("The stream must be readable.", nameof(stream));

            #endregion

            using (StreamReader reader = new StreamReader(stream))
                return Load(reader.ReadToEnd());
        }

        public static Scheme LoadEmpty()
        {
#if DEBUG
            Log.Method(
                new MethodInfo { Name = "SchemeStorage::LoadEmpty", Type = typeof(Scheme) }
            );
#endif

            return Load(Empty);
        }

        public static string Save(Scheme scheme)
        {
#if DEBUG
            Log.Method(
                new MethodInfo { Name = "SchemeStorage::Save", Type = typeof(string) },
                new ArgumentInfo { Name = nameof(scheme), Type = scheme.GetType(), Data = scheme }
            );
#endif

            #region arguments check

            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));

            #endregion

            return JsonConvert.SerializeObject(scheme);
        }

        public static void Save(Scheme scheme, Stream stream)
        {
#if DEBUG
            Log.Method(
                new MethodInfo { Name = "SchemeStorage::Save", Type = typeof(void) },
                new ArgumentInfo { Name = nameof(scheme), Type = scheme.GetType(), Data = scheme },
                new ArgumentInfo { Name = nameof(stream), Type = stream.GetType(), Data = new { stream.CanRead, stream.CanSeek, stream.CanWrite, stream.Length, stream.Position } }
            );
#endif

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
