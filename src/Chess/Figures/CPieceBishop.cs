using Chess.Enums;

namespace Chess.Figures
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