using Chess.Enums;

namespace Chess.Pieces
{
	public class CPiecePawn : CPiece
	{
		public CPiecePawn(EPlayer player) : base(player, EPieceType.Pawn, player == EPlayer.White ? 100 : -100)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♙" : "♟";
		}
	}
}