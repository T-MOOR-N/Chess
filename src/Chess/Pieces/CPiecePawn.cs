using Chess.Enums;

namespace Chess.Pieces
{
	public class CPiecePawn : CPiece
	{
		public CPiecePawn(EPlayer player) : base(player)
		{
		}

		public override string ToString()
		{
			return "p";
		}
	}
}