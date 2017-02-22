using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#if DEBUG
using VinCAD.Logger;
#endif

namespace VinCAD.Main
{
	public struct ElementValue
	{
		public bool Value { get; set; }
		public uint Delay { get; set; }
		public bool isSet { get; set; }
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
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Component::ctor", Type = typeof(Component) },
				new ArgumentInfo { Name = nameof(name), Type = name.GetType(), Data = name },
				new ArgumentInfo { Name = nameof(type), Type = type.GetType(), Data = type },
				new ArgumentInfo { Name = nameof(input), Type = input.GetType(), Data = input }
			);
#endif

			Name = name;
			Type = type;
			_input = input.ToList();
			Value = new ElementValue();
		}

		public void Calculate(ElementValue[] inputValues)
		{
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Component::Calculate", Type = typeof(void) },
				new ArgumentInfo { Name = nameof(inputValues), Type = inputValues.GetType(), Data = inputValues }
			);
#endif

			#region arguments check

			if (inputValues == null)
				throw new ArgumentNullException(nameof(inputValues));

			#endregion

			ElementValue tmp = new ElementValue();
			bool hasNull = false;
			switch (Type)
			{
				case ComponentType.And:
					tmp.Value = true;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value && inputValue.Value;
						else
							hasNull = true;

					if (hasNull)
					{
						if (tmp.Value == true)
							return;
						else
							tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;
					}
					else
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Max(x => x.Delay) + 1;
						else
							tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;
					}

					break;
				case ComponentType.AndNot:
					tmp.Value = true;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value && inputValue.Value;
						else
							hasNull = true;

					if (hasNull)
					{
						if (tmp.Value == true)
							return;
						else
							tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;
					}
					else
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Max(x => x.Delay) + 1;
						else
							tmp.Delay = inputValues.Where(x => x.Value == false).Min(y => y.Delay) + 1;
					}

					tmp.Value = !tmp.Value;
					break;
				case ComponentType.Or:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value || inputValue.Value;
						else
							hasNull = true;

					if (hasNull)
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
						else
							return;
					}
					else
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
						else
							tmp.Delay = inputValues.Max(y => y.Delay) + 1;
					}
					break;
				case ComponentType.OrNot:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value || inputValue.Value;
						else
							hasNull = true;

					if (hasNull)
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
						else
							return;
					}
					else
					{
						if (tmp.Value == true)
							tmp.Delay = inputValues.Where(x => x.Value == true).Min(x => x.Delay) + 1;
						else
							tmp.Delay = inputValues.Max(y => y.Delay) + 1;
					}

					tmp.Value = !tmp.Value;
					break;
				case ComponentType.Xor:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value ^ inputValue.Value;
						else
							return;

					tmp.Delay = inputValues.Max(y => y.Delay) + 1;
					break;
				case ComponentType.XorNot:
					tmp.Value = false;
					foreach (var inputValue in inputValues)
						if (inputValue.isSet)
							tmp.Value = tmp.Value ^ inputValue.Value;
						else
							return;

					tmp.Delay = inputValues.Max(y => y.Delay) + 1;

					tmp.Value = !tmp.Value;
					break;
				case ComponentType.Not:
					if (!inputValues[0].isSet)
						return;

					tmp.Value = !inputValues[0].Value;
					tmp.Delay = inputValues[0].Delay + 1;
					break;
				default:
					break;
			}
			tmp.isSet = true;
			Value = tmp;
		}

		public void AddInput(string input)
		{
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Component::AddInput", Type = typeof(void) },
				new ArgumentInfo { Name = nameof(input), Type = input.GetType(), Data = input }
			);
#endif

			#region arguments check

			if (string.IsNullOrEmpty(input))
				throw new ArgumentNullException(nameof(input));

			#endregion

			_input.Add(input);
		}

		public void RemoveInput(string input)
		{
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Component::RemoveInput", Type = typeof(void) },
				new ArgumentInfo { Name = nameof(input), Type = input.GetType(), Data = input }
			);
#endif

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
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Input::ctor", Type = typeof(Input) },
				new ArgumentInfo { Name = nameof(name), Type = name.GetType(), Data = name }
			);
#endif

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
#if DEBUG
			Log.Method(
				new MethodInfo { Name = "Output::ctor", Type = typeof(Output) },
				new ArgumentInfo { Name = nameof(name), Type = name.GetType(), Data = name },
				new ArgumentInfo { Name = nameof(input), Type = input.GetType(), Data = input }
			);
#endif

			Name = name;
			Input = input;
		}
	}
}
