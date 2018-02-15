using Chess.Pieces;

namespace Chess.Moves
{
	public class CMoveItem
	{
		public CBoard Board;

		public CPiece Piece;

		public int File;

		public int Rank;

		public CMoveItem(CBoard board, CPiece piece, int file, int rank)
		{
			Board = board;
			Piece = piece;
			File = file;
			Rank = rank;
		}
	}
}