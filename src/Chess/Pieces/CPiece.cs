using Chess.Enums;

namespace Chess.Pieces
{
	public enum EPieceType
	{
		None,
		King,
		Queen,
		Rook,
		Bishop,
		Knight,
		Pawn
	}

	public abstract class CPiece
	{
		public EPlayer Player { get; }

		public EPieceType Type { get; }

		public double Mark { get; }

		//public CBoard Board { get; internal set; }

		//public int File { get; internal set; }

		//public int Rank { get; internal set; }

		protected CPiece(EPlayer player, EPieceType type, double mark)
		{
			Player = player;
			Type = type;
			Mark = mark;
		}

		//public void MoveTo(string coordinate)
		//{
		//	MoveTo(CSquare.Parse(coordinate));
		//}

		//public void MoveTo(CSquare square)
		//{
		//	MoveTo(Board, square.File, square.Rank);
		//}

		//public void MoveTo(int file, int rank)
		//{
		//	MoveTo(Board, file, rank);
		//}

		//internal void MoveTo(CBoard board, int file, int rank)
		//{
		//	if (Board != board || File != file || Rank != rank)
		//	{
		//		if (Board != null)
		//		{
		//			Board[File, Rank] = null;
		//		}

		//		if (board != null)
		//		{
		//			board[file, rank] = this;
		//		}

		//		Board = board;
		//		File = file;
		//		Rank = rank;
		//	}
		//}

		//internal void MoveTo(CBoard board, int file, int rank)
		//{
		//	if (Board != board || File != file || Rank != rank)
		//	{
		//		Board?.SetPiece(null, File, Rank);
		//		board?.SetPiece(this, file, rank);

		//		Board = board;
		//		File = file;
		//		Rank = rank;
		//	}
		//}

	}
}