using Chess;
using Chess.Enums;
using Chess.Figures;

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

			//ChessBoard.Board = new CBoard();
			//ChessBoard.Board[0,1] = new CPiecePawn(EPlayer.White);
		}
	}
}
