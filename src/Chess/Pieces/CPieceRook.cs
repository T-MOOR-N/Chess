using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceRook : CPiece
	{
		public CPieceRook(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "R";
		}
	}
}