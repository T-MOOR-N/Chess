using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceBishop : CPiece
	{
		public CPieceBishop(EPlayer player) : base(player, EPieceType.Bishop, player == EPlayer.White ? 330 : -330)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♗" : "♝";
		}
	}
}