using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceBishop : CPiece
	{
		public CPieceBishop(EPlayer player) : base(player, EPieceType.Bishop)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♗" : "♝";
		}
	}
}