using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace LogicSimulator.Main
{
	public class Scheme
	{
		[JsonIgnore]
		public ReadOnlyCollection<IElement> Elements => new ReadOnlyCollection<IElement>(_elements);
		private List<IElement> _elements;

		[JsonRequired]
		public ReadOnlyCollection<Input> Inputs => new ReadOnlyCollection<Input>(_inputs);
		private List<Input> _inputs;

		[JsonRequired]
		public ReadOnlyCollection<Output> Outputs => new ReadOnlyCollection<Output>(_outputs);
		private List<Output> _outputs;

		[JsonRequired]
		public ReadOnlyCollection<Component> Components => new ReadOnlyCollection<Component>(_components);
		private List<Component> _components;

		[JsonIgnore]
		private List<List<Component>> ComponentLayers;

		public Scheme(IEnumerable<Input> inputs, IEnumerable<Output> outputs, IEnumerable<Component> components)
		{
			_inputs = inputs.ToList();
			_outputs = outputs.ToList();
			_components = components.ToList();
			_elements = _inputs.Cast<IElement>().Concat(_components).Concat(_outputs).ToList();

			#region split components to layers

			ComponentLayers = new List<List<Component>>();

			var addedList = new List<IElement>();
			addedList.AddRange(inputs);

			while (true)
			{
				var toadd = components.Where(component => !addedList.Contains(component) && component.Input.All(input => addedList.Any(e => e.Name == input))).ToList();
				if (toadd.Any())
				{
					addedList.AddRange(toadd);
					ComponentLayers.Add(toadd);
				}
				else
				{
					break;
				}
			}

			#endregion
		}

		public void SetCurrentState(IEnumerable<ElementValue> state)
		{
			#region arguments check

			if (state == null)
				throw new ArgumentNullException(nameof(state));
			if (state.Count() != _inputs.Count)
				throw new ArgumentException(
					$"The state array Count must be equal to the count of the inputs ({_inputs.Count}).",
					nameof(state)
				);

			#endregion

			for (int j = 0; j < _inputs.Count; j++)
				_inputs[j].Value = state.ElementAt(j);
		}

		public void SetCurrentState(int stateNumber)
		{
			#region arguments check

			if (stateNumber >= Convert.ToInt32(Math.Pow(2, _inputs.Count)) || stateNumber < 0)
				throw new ArgumentOutOfRangeException(
					nameof(stateNumber),
					$"The state number must be greater than or equal to 0 and less than 2 in power of the count of the inputs ({Convert.ToInt32(Math.Pow(2, _inputs.Count))})."
				);

			#endregion

			var state = new ElementValue[_inputs.Count];

			for (int i = 0; i < state.Length; i++)
			{
				state[state.Length - i - 1] = new ElementValue();
				state[state.Length - i - 1].Value = ((stateNumber >> i) & 1) == 1;
				state[state.Length - i - 1].Delay = 0;
			}

			SetCurrentState(state);
		}

		public void CalculateForCurrentState()
		{
			foreach (var layer in ComponentLayers)
				foreach (var component in layer)
				{
					var values = new ElementValue[component.Input.Count];
					for (int i = 0; i < component.Input.Count; i++)
						values[i] = GetElementValue(component.Input.ElementAt(i));

					component.Calculate(values);
				}

			foreach (var output in _outputs)
				output.Value = GetElementValue(output.Input);
		}

		public ElementValue GetElementValue(string name)
		{
			var match = _elements.FirstOrDefault(e => e.Name == name);

			if (match == null)
				throw new InputNotFoundException(name);
			if (!match.Value.Value.HasValue)
				throw new InputHasNoValueException(name);
			if (!match.Value.Delay.HasValue)
				throw new InputHasNoValueException(name);

			return match.Value;
		}

		public ElementValue[,] CalculateTable()
		{
			var inputs_count_pow2 = Convert.ToInt32(Math.Pow(2, _inputs.Count));
			var inputs_plus_outputs_count = _inputs.Count + _outputs.Count;
			var table = new ElementValue[inputs_count_pow2, inputs_plus_outputs_count];

			for (int i = 0; i < inputs_count_pow2; i++)
			{
				SetCurrentState(i);
				CalculateForCurrentState();

				for (int j = 0; j < inputs_plus_outputs_count; j++)
				{
					if (j < _inputs.Count)
						table[i, j] = _inputs[j].Value;
					else
						table[i, j] = _outputs[j - _inputs.Count].Value;
				}
			}

			return table;
		}

		const int fieldFirstWidth = 60;
		const int fieldStepWidth = 15;
		const int fieldHeight = 30;

		public Bitmap DrawTimeDiagram(uint[] InputDelays, uint TimeLimit)
		{
			var rows = _inputs.Select(x => x.Name).Concat(_outputs.Select(x => x.Name)).ToArray();

			int ImgRows = _inputs.Count + _outputs.Count;
			int ImgStepColumns = (int)(TimeLimit);
			int ImgWidth = fieldFirstWidth + ImgStepColumns * fieldStepWidth;
			int ImgHeight = fieldHeight * ImgRows;

			var bt = new Bitmap(ImgWidth, ImgHeight);
			var gr = Graphics.FromImage(bt);

			gr.FillRectangle(Brushes.White, 0, 0, ImgWidth, ImgHeight);

			Pen gridPen = new Pen(Color.Black);
			gridPen.DashStyle = DashStyle.Solid;

			for (int gor = 1; gor <= ImgRows; gor++)
			{
				gr.DrawLine(gridPen, 0, gor * fieldHeight, ImgWidth, gor * fieldHeight);
				gr.DrawString(rows[gor - 1], new Font("Arial", 10), Brushes.Black,
					new RectangleF(5, gor * fieldHeight - 20, fieldFirstWidth, 20));
			}

			gridPen.Width = 2;
			gr.DrawLine(gridPen, fieldFirstWidth, 0, fieldFirstWidth, ImgHeight);
			gridPen.Width = 1;

			gridPen.Color = Color.Gray;
			gridPen.DashStyle = DashStyle.Dot;
			for (int ver = 0; ver <= ImgStepColumns; ver++)
				gr.DrawLine(gridPen, fieldFirstWidth + ver * fieldStepWidth, 0,
					fieldFirstWidth + ver * fieldStepWidth, ImgHeight);

			gridPen.Color = Color.Red;
			gridPen.Width = 2;
			gridPen.DashStyle = DashStyle.Solid;

			uint CurrentTick = 0, CurrentColumn = 0;
			uint[] LastInputChangeTick = new uint[_inputs.Count];
			ElementValue[] state = new ElementValue[_inputs.Count];
			for (int i = 0; i < _inputs.Count; i++)
				state[i] = new ElementValue() { Delay = 0, Value = false };

			while (CurrentTick < TimeLimit)
			{
				for (int i = 0; i < _inputs.Count; i++)
					if (CurrentTick - LastInputChangeTick[i] >= InputDelays[i])
					{
						LastInputChangeTick[i] = CurrentTick;
						state[i].Value = !state[i].Value;
					}

				SetCurrentState(state);
				CalculateForCurrentState();

				for (int rw = 0; rw < ImgRows; rw++)
				{
					if (rw < _inputs.Count)
						gr.DrawLine(gridPen,
							fieldFirstWidth + CurrentColumn * fieldStepWidth, (rw + (state[rw].Value.Value ? .3f : 1f)) * fieldHeight,
							fieldFirstWidth + (CurrentColumn + 1) * fieldStepWidth, (rw + (state[rw].Value.Value ? .3f : 1f)) * fieldHeight);
					else
						gr.DrawLine(gridPen,
							fieldFirstWidth + CurrentColumn * fieldStepWidth,
							(rw + (_outputs[rw - _inputs.Count].Value.Value.Value ? .3f : 1f)) * fieldHeight,
							fieldFirstWidth + (CurrentColumn + 1) * fieldStepWidth,
							(rw + (_outputs[rw - _inputs.Count].Value.Value.Value ? .3f : 1f)) * fieldHeight);
				}

				CurrentColumn++;
				CurrentTick += 1;
			}
			gr.Dispose();
			return bt;
		}

		private Bitmap DrawTimeWorkDiagram()
		{
			//int ImgColumns = 1 + table.GetLength(0);
			//int ImgRows = table.GetLength(1);
			//int ImgWidth = fieldWidth * ImgColumns;
			//int ImgHeight = fieldHeight * ImgRows;

			//Image img = diagramBox.Image;

			//var bt = new Bitmap(ImgWidth, ImgHeight);
			//var gr = Graphics.FromImage(bt);

			//gr.FillRectangle(Brushes.White, 0, 0, ImgWidth, ImgHeight);

			//Pen gridPen = new Pen(Color.Black);
			//gridPen.DashStyle = DashStyle.Solid;

			//for (int gor = 1; gor <= ImgRows; gor++)
			//{
			//	gr.DrawLine(gridPen, 0, gor * fieldHeight, ImgWidth, gor * fieldHeight);
			//	gr.DrawString(rows[gor - 1], new Font("Arial", 10), Brushes.Black,
			//		new RectangleF(5, gor * fieldHeight - 20, fieldWidth, 20));
			//}

			//gridPen.Width = 2;
			//gr.DrawLine(gridPen, fieldWidth, 0, fieldWidth, ImgHeight);
			//gridPen.Width = 1;

			//gridPen.DashStyle = DashStyle.Dash;
			//for (int ver = 2; ver <= ImgColumns; ver++)
			//	gr.DrawLine(gridPen, ver * fieldWidth, 0, ver * fieldWidth, ImgHeight);

			//gridPen.Color = Color.Red;
			//gridPen.Width = 2;
			//gridPen.DashStyle = DashStyle.Solid;

			//for (int rw = 0; rw < ImgRows; rw++)
			//{
			//	List<Point> sigLv = new List<Point>();
			//	for (int st = 0; st < ImgColumns - 1; st++)
			//	{
			//		sigLv.Add(new Point((st + 1) * fieldWidth, (int)((rw - (table[st, rw].Value.Value ? -.3f : -1)) * fieldHeight)));
			//		sigLv.Add(new Point((st + 2) * fieldWidth, (int)((rw - (table[st, rw].Value.Value ? -.3f : -1)) * fieldHeight)));
			//	}

			//	gr.DrawLines(gridPen, sigLv.ToArray());
			//}
			//gr.Dispose();

			return null;
		}

		public void WriteTable(Stream stream, ElementValue[,] table)
		{
			#region arguments check

			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (!stream.CanWrite)
				throw new ArgumentException("The stream must be writeable.", nameof(stream));
			if (table == null)
				throw new ArgumentNullException(nameof(table));

			#endregion

			var table_Count = new int[2] { table.GetLength(0), table.GetLength(1) };

			#region table dimensions check

			if (table_Count[0] != Convert.ToInt32(Math.Pow(2, _inputs.Count)))
				throw new ArgumentException(
					$"The Count of the 0 dimension of the table array must be equal to 2 in power of the count of the inputs ({Convert.ToInt32(Math.Pow(2, _inputs.Count))}).",
					nameof(table)
				);
			if (table_Count[1] != _inputs.Count + _outputs.Count)
				throw new ArgumentException(
					$"The Count of the 1 dimension of the table array must be equal to the count of the inputs + the count of the outputs ({_inputs.Count + _outputs.Count}).",
					nameof(table)
				);

			#endregion

			using (var writer = new StreamWriter(stream))
			{
				writer.Write("<!DOCTYPE html><html><head><meta charset='utf-8'><title>Table of values</title><style>table * {padding: 5px; text-align: center;} table {border: 1px solid black;} th {border-bottom: 1px solid black;} .border-left {margin-left: 1px; border-left: 1px solid black;}</style></head><body><table cellspacing='0' cellpadding='0'>");
				writer.Write($"<tr><th>{string.Join("</th><th>", _elements.Where(e => e is Input).Select(i => i.Name))}</th><th class='border-left'>{string.Join("</th><th>", _elements.Where(e => e is Output).Select(o => $"{o.Name}</th><th>τ"))}</th></tr>");
				for (int i = 0; i < table_Count[0]; i++)
				{
					writer.Write("<tr>");
					for (int j = 0; j < table_Count[1]; j++)
						writer.Write($"<td{(j == _inputs.Count ? " class='border-left'" : string.Empty)}>{Convert.ToInt16(table[i, j].Value)}{(j >= _inputs.Count ? "</td><td>" + Convert.ToInt32(table[i, j].Delay).ToString() : string.Empty)}</td>");

                    writer.Write("</tr>");
				}
				writer.Write("</table></body></html>");
			}
		}

		public void CalculateAndWriteTable(Stream stream) => WriteTable(stream, CalculateTable());
	}
}
