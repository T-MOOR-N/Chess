using Chess.Enums;
using Chess.Figures;

namespace Chess
{
	public class CBoard
	{
		private readonly CPiece[,] _pieces = new CPiece[8,8];
		
		public CPiece this[string coordinate]
		{
			get
			{
				var square = CSquare.Parse(coordinate);
				return this[square.File, square.Rank];
			}
			set
			{
				var square = CSquare.Parse(coordinate);
				this[square.File, square.Rank] = value;
			}
		}

		public CPiece this[int file, int rank]
		{
			get { return _pieces[file, rank]; }
			set
			{
				var oldValue = _pieces[file, rank];

				if (_pieces[file, rank] != null)
				{
					oldValue.Board = null;
				}

				_pieces[file, rank] = value;

				if (value != null)
				{
					value.Board = this;
				}
			}
		}

		public bool IsCheck(EPlayer player)
		{
			return false;
		}

		public bool IsCheckMate(EPlayer player)
		{
			return false;
		}

		public bool WasCastling(EPlayer player)
		{
			return false;
		}
		
		
	}
}