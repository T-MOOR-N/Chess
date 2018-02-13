using Chess;
using Chess.Enums;
using Chess.Pieces;

namespace ChessApp
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			ChessBoard.Board = new CBoard
			{
				["A2"] = new CPiecePawn(EPlayer.White),
				["A3"] = new CPieceQueen(EPlayer.Black)
			};
		}
	}
}
