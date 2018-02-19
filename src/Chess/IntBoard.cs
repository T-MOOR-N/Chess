namespace Chess
{
	public class IntBoard
	{
		public enum EPiece
		{
			WhitePawn = 0x10000001,
			WhiteKnight = 0x10000002,
			WhiteBishop = 0x10000004,
			WhiteRook = 0x10000008,
			WhiteQueen = 0x10000010,
			WhiteKing = 0x10000020,

			BlackPawn = 0x20000001,
			BlackKnight = 0x20000002,
			BlackBishop = 0x20000004,
			BlackRook = 0x20000008,
			BlackQueen = 0x20000010,
			BlackKing = 0x20000020
		}

		private readonly int[] _pieces = new int[64];

		public EPiece this[string coordinate]
		{
			get { return this[CSquare.Parse(coordinate)]; }
			set { this[CSquare.Parse(coordinate)] = value; }
		}

		public EPiece this[CSquare square]
		{
			get { return this[square.File, square.Rank]; }
			set { this[square.File, square.Rank] = value; }
		}

		public EPiece this[int file, int rank]
		{
			get { return (EPiece) _pieces[file + 8*rank]; }
			set { _pieces[file + 8*rank] = (int)value; }
		}

		public bool Test()
		{
			foreach (var piece1 in _pieces)
			{
				if (piece1 >> 7 == 2)
				{
					foreach (var piece2 in _pieces)
					{
						if (piece2 >> 7 == 1)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}