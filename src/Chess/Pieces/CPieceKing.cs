using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceKing : CPiece
	{
		public CPieceKing(EPlayer player) : base(player, EPieceType.King)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♔" : "♚";
		}
	}
}