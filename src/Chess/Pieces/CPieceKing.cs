using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceKing : CPiece
	{
		public CPieceKing(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "K";
		}
	}
}