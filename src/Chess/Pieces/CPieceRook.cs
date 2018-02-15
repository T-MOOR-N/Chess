using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceRook : CPiece
	{
		public CPieceRook(EPlayer player) : base(player, EPieceType.Rook)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♖" : "♜";
		}
	}
}