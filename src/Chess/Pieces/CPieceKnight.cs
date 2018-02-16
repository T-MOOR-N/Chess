using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceKnight : CPiece
	{
		public CPieceKnight(EPlayer player) : base(player, EPieceType.Knight, player == EPlayer.White ? 320 : -320)
		{
		}

		public override string ToString()
		{
			return Player == EPlayer.White ? "♘" : "♞";
		}
	}
}