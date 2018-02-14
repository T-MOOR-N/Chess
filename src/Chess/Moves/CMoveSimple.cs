using Chess.Pieces;

namespace Chess.Moves
{
	public class CMoveSimple : CMove
	{
		public CPiece Piece { get; }
		public int StartFile { get; }
		public int StartRank { get; }
		public int FinishFile { get; }
		public int FinishRank { get; }


		public CMoveSimple(CPiece piece, int file, int rank)
		{
			Piece = piece;
			StartFile = piece.File;
			StartRank = piece.Rank;
			FinishFile = file;
			FinishRank = rank;


			DoAction = () =>
			{
				piece.MoveTo(FinishFile, FinishRank);
			};

			UndoAction = () =>
			{
				piece.MoveTo(StartFile, StartRank);
			};
		}
		
		public override string ToString()
		{
			return $"{Piece}{new CSquare(StartFile, StartRank)}\u202F\u2013\u202F{new CSquare(FinishFile, FinishRank)}";
		}
	}
}