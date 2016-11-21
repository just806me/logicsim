using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LogicSimulator.Main
{
    public interface IElement
    {
        string Name { get; set; }
        bool? Value { get; set;  }
    }

    public class Component : IElement
    {
        [JsonRequired]
        public ComponentType Type { get; }
        [JsonRequired]
        public ReadOnlyCollection<string> Input => new ReadOnlyCollection<string>(_input);
        private readonly IList<string> _input;
        [JsonRequired]
        public string Name { get; set; }
        [JsonIgnore]
        public bool? Value { get; set; }

        [JsonConstructor]
        public Component(string name, ComponentType type, IEnumerable<string> input)
        {
            Name = name;
            Type = type;
            _input = input.ToList();
        }

        public void Calculate(bool[] inputValues)
        {
            #region arguments check

            if (inputValues == null)
                throw new ArgumentNullException(nameof(inputValues));

            #endregion

            switch (Type)
            {
                case ComponentType.And:
                    Value = true;
                    foreach (var value in inputValues)
                        Value = Value.Value && value;
                    break;
                case ComponentType.AndNot:
                    Value = true;
                    foreach (var value in inputValues)
                        Value = Value.Value && value;
                    Value = !Value.Value;
                    break;
                case ComponentType.Or:
                    Value = false;
                    foreach (var value in inputValues)
                        Value = Value.Value || value;
                    break;
                case ComponentType.OrNot:
                    Value = false;
                    foreach (var value in inputValues)
                        Value = Value.Value || value;
                    Value = !Value.Value;
                    break;
                case ComponentType.Xor:
                    Value = false;
                    foreach (var value in inputValues)
                        Value = Value.Value ^ value;
                    break;
                case ComponentType.XorNot:
                    Value = false;
                    foreach (var value in inputValues)
                        Value = Value.Value ^ value;
                    Value = !Value.Value;
                    break;
                case ComponentType.Not:
                    Value = !inputValues[0];
                    break;
                default:
                    break;
            }
        }

        public void AddInput(string input)
        {
            #region arguments check

            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            #endregion

            _input.Add(input);
        }

        public void RemoveInput(string input)
        {
            #region arguments check

            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            #endregion

            _input.Remove(input);
        }
    }

    public enum ComponentType
    {
        And,
        AndNot,
        Or,
        OrNot,
        Xor,
        XorNot,
        Not
    }

    public class Input : IElement
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonIgnore]
        public bool? Value { get; set; }

        [JsonConstructor]
        public Input(string name)
        {
            Name = name;
        }
    }

    public class Output : IElement
    {
        [JsonRequired]
        public string Name { get; set; }
        [JsonIgnore]
        public bool? Value { get; set; }
        [JsonRequired]
        public string Input { get; set; }

        [JsonConstructor]
        public Output(string name, string input)
        {
            Name = name;
            Input = input;
        }
    }
}
