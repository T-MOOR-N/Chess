using Chess.Enums;
using Chess.Pieces;

namespace Chess.Moves
{
	public class CMoveSimplePromotion : CMove
	{
		public CPiecePawn Pawn { get; }
		public CPiece Promotion { get; }
		public int File { get; }
		public int StartRank { get; }
		public int FinishRank { get; }

		public CMoveSimplePromotion(CPiecePawn pawn, CPiece promotion)
		{
			var board = pawn.Board;
			Pawn = pawn;
			Promotion = promotion;
			File = pawn.File;
			StartRank = pawn.Rank;
			FinishRank = pawn.Player == EPlayer.White ? 7 : 0;
			
			DoAction = () =>
			{
				board[File, StartRank] = null;
				board[File, FinishRank] = promotion;
			};

			UndoAction = () =>
			{
				board[File, StartRank] = pawn;
				board[File, FinishRank] = null;
			};
		}

		public override string ToString()
		{
			return $"{Pawn}{new CSquare(File, StartRank)}—{new CSquare(File, FinishRank)}{Promotion}";
		}
	}
}