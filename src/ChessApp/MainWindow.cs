using System;
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
			history.Reverse();
			ListBox.ItemsSource = history;
			ListBox.SelectedIndex = Math.Min(ListBox.Items.Count, 0);
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
