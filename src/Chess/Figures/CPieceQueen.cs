using Chess.Enums;

namespace Chess.Figures
{
	public class CPieceQueen : CPiece
	{
		public CPieceQueen(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "Q";
		}
	}
}