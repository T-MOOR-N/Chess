using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceKing : CPiece
	{
		public CPieceKing(EPlayer player) : base(player, EPieceType.King, player == EPlayer.White ? 20000 : -20000)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♔" : "♚";
		}
	}
}