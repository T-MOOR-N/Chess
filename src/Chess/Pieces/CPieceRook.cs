using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceRook : CPiece
	{
		public CPieceRook(EPlayer player) : base(player, EPieceType.Rook, player == EPlayer.White ? 500 : -500)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♖" : "♜";
		}
	}
}