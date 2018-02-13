using Chess.Enums;

namespace Chess.Pieces
{
	public class CPieceBishop : CPiece
	{
		public CPieceBishop(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "B";
		}
	}
}