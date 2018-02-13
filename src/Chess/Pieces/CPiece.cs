using Chess.Enums;

namespace Chess.Pieces
{
	public abstract class CPiece
	{
		public EPlayer Player { get; }

		protected CPiece(EPlayer player)
		{
			Player = player;
		}

		public CBoard Board { get; internal set; }
	}
}