using System;
using System.Runtime.Serialization;

namespace VinCAD.Main
{

    [Serializable]
    public class SchemeException : Exception
    {
        public string InputName { get; set; }

        public SchemeException() { }
        public SchemeException(string message) : base(message) { }
        public SchemeException(string message, Exception inner) : base(message, inner) { }
        protected SchemeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class InputNotFoundException : SchemeException
    {
        public InputNotFoundException() { }
        public InputNotFoundException(string message) : base(message) { }
        public InputNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected InputNotFoundException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class InputHasNoValueException : SchemeException
    {
        public InputHasNoValueException() { }
        public InputHasNoValueException(string message) : base(message) { }
        public InputHasNoValueException(string message, Exception inner) : base(message, inner) { }
        protected InputHasNoValueException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}