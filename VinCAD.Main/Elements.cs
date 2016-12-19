using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VinCAD.Main
{
	public struct ElementValue
	{
		public bool? Value { get; set; }
		public uint? Delay { get; set; }
	}

    public interface IElement
    {
        string Name { get; set; }
		ElementValue Value { get; set; }
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
        public ElementValue Value { get; set; }

        [JsonConstructor]
        public Component(string name, ComponentType type, IEnumerable<string> input)
        {
            Name = name;
            Type = type;
            _input = input.ToList();
			Value = new ElementValue();
		}

        public void Calculate(ElementValue[] inputValues)
        {
            #region arguments check

            if (inputValues == null)
                throw new ArgumentNullException(nameof(inputValues));

			#endregion

			ElementValue tmp = new ElementValue();
            switch (Type)
            {
                case ComponentType.And:
					tmp.Value = true;
                    foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value && inputValue.Value.Value;

					if (tmp.Value == true)
						tmp.Delay = inputValues.Max(x => x.Delay) + 1;
					else
						tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;
					break;
                case ComponentType.AndNot:
					tmp.Value = true;
					foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value && inputValue.Value.Value;

					if (tmp.Value == true)
						tmp.Delay = inputValues.Max(x => x.Delay) + 1;
					else
						tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;

					tmp.Value = !tmp.Value;
                    break;
                case ComponentType.Or:
					tmp.Value = false;
                    foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value || inputValue.Value.Value;

					if (tmp.Value == true)
						tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
					else
						tmp.Delay = inputValues.Max(y => y.Delay) + 1;
					break;
                case ComponentType.OrNot:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value || inputValue.Value.Value;

					if (tmp.Value == true)
						tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
					else
						tmp.Delay = inputValues.Max(y => y.Delay) + 1;

					tmp.Value = !tmp.Value;
                    break;
                case ComponentType.Xor:
					tmp.Value = false;
                    foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value ^ inputValue.Value.Value;

					tmp.Delay = inputValues.Max(y => y.Delay) + 1;
					break;
                case ComponentType.XorNot:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						tmp.Value = tmp.Value.Value ^ inputValue.Value.Value;

					tmp.Delay = inputValues.Max(y => y.Delay) + 1;

					tmp.Value = !tmp.Value;
                    break;
                case ComponentType.Not:
					tmp.Value = !inputValues[0].Value.Value;
					tmp.Delay = inputValues[0].Delay + 1;
                    break;
                default:
                    break;
            }
			Value = tmp;
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
        public ElementValue Value { get; set; }

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
        public ElementValue Value { get; set; }
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
