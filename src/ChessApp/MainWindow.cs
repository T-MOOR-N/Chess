using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Chess;
using Chess.Enums;
using Chess.Moves;

namespace ChessApp
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private static readonly CBoard Board = CBoard.GetDefaultBoard();
		private static readonly CGame Game = new CGame(Board);
		private EPlayer _player = EPlayer.White;
		private readonly Random _random = new Random();

		public MainWindow()
		{
			InitializeComponent();

			ChessBoard.Board = Board;
		}

		private void Do(CMove move)
		{
			Game.Do(move);
			Update();
		}

		private void Undo()
		{
			if (Game.Undo())
			{
				Update();
			}
		}

		private void Redo()
		{
			if (Game.Redo())
			{
				Update();
			}
		}

		private void Update()
		{
			_player = 1 - _player;
			ChessBoard.InvalidateVisual();

			var history = Game.GetHistory();

			var result = new List<string>();
			for (var i = 0; i < history.Count; i += 2)
			{
				var num = i/2 + 1;
				var numSize = num.ToString().Length;
				var spaces = new string(' ', Math.Max(2, numSize) - numSize);

				var text = $"{spaces}{num}. ";
				text += $"{history[i]}";
				if (i + 1 < history.Count)
				{
					text += $"\u2003{history[i + 1]}";
				}
				result.Add(text);
			}



			ListBox.ItemsSource = result;
			ListBox.ScrollIntoView(result.LastOrDefault());
		}

		private void DoButtonClick(object sender, RoutedEventArgs e)
		{
			var moves = Game.GetAllMoves(_player);

			if (moves.Count == 0)
			{
				MessageBox.Show("Все закончилось");
			}
			else
			{
				var index = _random.Next(0, moves.Count);

				Do(moves[index]);
			}
		}

		private void UndoButton_Click(object sender, RoutedEventArgs e)
		{
			Undo();
		}

		private void RedoButton_Click(object sender, RoutedEventArgs e)
		{
			Redo();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			var isControlKey = e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl);

			switch (e.Key)
			{
				case Key.Back:
					Undo();
					break;

				case Key.Z:
					if (isControlKey)
					{
						Undo();
					}
					break;


				case Key.Y:
					if (isControlKey)
					{
						Redo();
					}
					break;
			}
		}
	}
}
