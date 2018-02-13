using Chess.Pieces;

namespace Chess.Moves
{
	public class CMoveCapture : CMove
	{
		public CPiece Piece { get; }
		public int StartFile { get; }
		public int StartRank { get; }
		public int FinishFile { get; }
		public int FinishRank { get; }

		public CMoveCapture(CPiece piece, CPiece partner)
		{
			Piece = piece;
			StartFile = piece.File;
			StartRank = piece.Rank;
			FinishFile = partner.File;
			FinishRank = partner.Rank;


			DoAction = () =>
			{
				piece.MoveTo(FinishFile, FinishRank);
			};

			UndoAction = () =>
			{
				piece.MoveTo(StartFile, StartRank);
				piece.Board[FinishFile, FinishRank] = partner;
			};
		}

		public override string ToString()
		{
			return $"{Piece}{new CSquare(StartFile, StartRank)}×{new CSquare(FinishFile, FinishRank)}";
		}
	}
}