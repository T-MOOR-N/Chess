using System;
using System.Windows;
using Chess;
using Chess.Enums;

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

		private void button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var moves = Game.GetAllMoves(_player);

			if (moves.Count == 0)
			{
				MessageBox.Show("Все закончилось");
			}
			else
			{
				var index = _random.Next(0, moves.Count);
				moves[index].Do();
				ChessBoard.InvalidateVisual();

				_player = 1 - _player;
			}

		}
	}
}
