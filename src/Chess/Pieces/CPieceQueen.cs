using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceQueen : CPiece
	{
		public CPieceQueen(EPlayer player) : base(player, EPieceType.Queen, player == EPlayer.White ? 900 : -900)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♕" : "♛";
		}
	}
}