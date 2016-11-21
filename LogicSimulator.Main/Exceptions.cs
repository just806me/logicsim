using System;

namespace LogicSimulator.Main
{
    public class SchemeException : Exception
    {
        public string InputName { get; }

        public SchemeException(string inputName)
        {
            InputName = inputName;
        }

        public SchemeException(string message, Exception innerException) : base(message, innerException) { }

        public SchemeException(string inputName, string message) : base(message)
        {
            InputName = inputName;
        }

        public SchemeException(string inputName, string message, Exception innerException) : base(message, innerException)
        {
            InputName = inputName;
        }
    }

    public class InputNotFoundException : SchemeException
    {
        public InputNotFoundException(string inputName) : base(inputName) { }

        public InputNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }

        public InputNotFoundException(string inputName, string message) 
            : base(inputName, message) { }

        public InputNotFoundException(string inputName, string message, Exception innerException) 
            : base(inputName, message, innerException) { }
    }
    
    public class InputHasNoValueException : SchemeException
    {
        public InputHasNoValueException(string inputName) : base(inputName) { }

        public InputHasNoValueException(string message, Exception innerException) 
            : base(message, innerException) { }

        public InputHasNoValueException(string inputName, string message) 
            : base(inputName, message) { }

        public InputHasNoValueException(string inputName, string message, Exception innerException) 
            : base(inputName, message, innerException) { }
    }


}