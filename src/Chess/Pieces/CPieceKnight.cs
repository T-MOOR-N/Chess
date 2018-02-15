using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceKnight : CPiece
	{
		public CPieceKnight(EPlayer player) : base(player, EPieceType.Knight)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♘" : "♞";
		}
	}
}