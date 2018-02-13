using Chess.Enums;

namespace Chess.Pieces
{
	public abstract class CPiece
	{
		public EPlayer Player { get; }

		public CBoard Board { get; private set; }

		public int File { get; private set; }

		public int Rank { get; private set; }

		protected CPiece(EPlayer player)
		{
			Player = player;
		}

		public void MoveTo(string coordinate)
		{
			MoveTo(CSquare.Parse(coordinate));
		}

		public void MoveTo(CSquare square)
		{
			MoveTo(Board, square.File, square.Rank);
		}

		public void MoveTo(int file, int rank)
		{
			MoveTo(Board, file, rank);
		}

		internal void MoveTo(CBoard board, int file, int rank)
		{
			if (Board != board || File != file || Rank != rank)
			{
				Board?.SetPiece(null, File, Rank);
				board?.SetPiece(this, file, rank);

				Board = board;
				File = file;
				Rank = rank;
			}
		}

	}
}