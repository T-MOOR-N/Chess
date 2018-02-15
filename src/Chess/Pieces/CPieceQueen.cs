using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceQueen : CPiece
	{
		public CPieceQueen(EPlayer player) : base(player, EPieceType.Queen)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♕" : "♛";
		}
	}
}