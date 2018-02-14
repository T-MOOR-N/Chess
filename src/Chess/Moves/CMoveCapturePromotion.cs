using Chess.Pieces;

namespace Chess.Moves
{
	public class CMoveCapturePromotion : CMove
	{
		public CPiecePawn Pawn { get; }
		public CPiece Promotion { get; }
		public int StartFile { get; }
		public int FinishFile { get; }
		public int StartRank { get; }
		public int FinishRank { get; }

		public CMoveCapturePromotion(CPiecePawn pawn, CPiece promotion, CPiece partner)
		{
			var board = pawn.Board;
			Pawn = pawn;
			Promotion = promotion;
			StartFile = pawn.File;
			StartRank = pawn.Rank;
			FinishFile = partner.File;
			FinishRank = partner.Rank;

			DoAction = () =>
			{
				board[StartFile, StartRank] = null;
				board[FinishFile, FinishRank] = promotion;
			};

			UndoAction = () =>
			{
				board[StartFile, StartRank] = pawn;
				board[FinishFile, FinishRank] = partner;
			};
		}

		public override string ToString()
		{
			return $"{Pawn}{new CSquare(StartFile, StartRank)}\u202F×\u202F{new CSquare(FinishFile, FinishRank)}{Promotion}";
		}
	}
}