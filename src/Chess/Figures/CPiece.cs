using Chess.Enums;

namespace Chess.Figures
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