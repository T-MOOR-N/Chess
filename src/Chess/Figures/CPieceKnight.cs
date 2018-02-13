using Chess.Enums;

namespace Chess.Figures
{
	public class CPieceKnight : CPiece
	{
		public CPieceKnight(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "N";
		}
	}
}