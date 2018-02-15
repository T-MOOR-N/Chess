using System.Linq;

namespace Chess.Moves
{
	public class CMove
	{
		public readonly CMoveItem[] Remove;
		public readonly CMoveItem[] Insert;

		public CMove(CMoveItem[] remove, CMoveItem[] insert)
		{
			Remove = remove;
			Insert = insert;
		}

		public void Do()
		{
			foreach (var item in Remove)
			{
				item.Board[item.File, item.Rank] = null;
			}

			foreach (var item in Insert)
			{
				item.Board[item.File, item.Rank] = item.Piece;
			}
		}

		public void Undo()
		{
			foreach (var item in Insert)
			{
				item.Board[item.File, item.Rank] = null;
			}

			foreach (var item in Remove)
			{
				item.Board[item.File, item.Rank] = item.Piece;
			}
		}

		public override string ToString()
		{
			if (Insert.Length == 1)
			{
				var to = Insert[0];
				var partner = Remove.FirstOrDefault(x => x.File == to.File && x.Rank == to.Rank);
				var operation = partner == null ? "–" : "×";

				var from = Remove.FirstOrDefault(x => x.Piece == to.Piece) ??
				           Remove.FirstOrDefault(x => x.Piece.Player == to.Piece.Player);

				var promotion = from.Piece != to.Piece ? to.Piece : null;
				return $"{from.Piece}{new CSquare(from.File, from.Rank)}\u202F{operation}\u202F{new CSquare(to.File, to.Rank)}{promotion}";
			}

			return base.ToString();
		}
	}
}