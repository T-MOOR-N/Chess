using System;
using System.Collections;
using System.Collections.Generic;
using Chess;
using Chess.Enums;
using Chess.Moves;
using Chess.Pieces;

namespace ChessApp
{
	public class CGame
	{
		private readonly List<CMove> _history = new List<CMove>();

		private int _historyIndex = -1;

		public CBoard Board { get; }

		public List<CMove> GetHistory()
		{
			return _history.GetRange(0, _historyIndex + 1);
		}

		public void Do(CMove move)
		{
			_history.RemoveRange(_historyIndex + 1, _history.Count - _historyIndex - 1);
			_history.Add(move);

			Redo();
		}

		public bool Undo()
		{
			if (_historyIndex == -1)
			{
				return false;
			}

			_history[_historyIndex].Undo();
			_historyIndex--;
			return true;
		}

		public bool Redo()
		{
			if (_historyIndex == _history.Count - 1)
			{
				return false;
			}

			_historyIndex++;
			_history[_historyIndex].Do();

			return true;
		}

		public CGame(CBoard board)
		{
			Board = board;
		}

		private enum EMoveResult
		{
			Impossible = 0,
			Easy = 1,
			Capture = 2,
			KingCapture = 3,
			
		}

		private static EMoveResult MoveOrBreak(ICollection<CMove> listToAdd, CMoveItem fromItem, int file, int rank)
		{
			var piece = fromItem.Piece;
			var partner = fromItem.Board[file, rank];
			var board = fromItem.Board;

			if (partner == null)
			{
				//Ход без взятия
				//listToAdd.Add(new CMoveSimple(piece, file, rank));

				var move = new CMove(
					new[] { fromItem },
					new[] { new CMoveItem(board, piece, file, rank) }
					);

				listToAdd.Add(move);

				return EMoveResult.Easy;
			}

			if (partner.Player != piece.Player)
			{
				//Ход со взятием
				//listToAdd.Add(new CMoveCapture(piece, partner));

				if (partner.Type == EPieceType.King)
				{
					return EMoveResult.KingCapture;
				}

				var move = new CMove(
					new[] { fromItem, new CMoveItem(board, partner, file, rank) },
					new[] { new CMoveItem(board, piece, file, rank) }
					);

				listToAdd.Add(move);
				return EMoveResult.Capture;
			}

			return EMoveResult.Impossible;
		}

		private static bool AddKnightMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			if (startFile + 1 < 8 && startRank + 2 < 8)
			{
				if (MoveOrBreak(result, fromItem, startFile + 1, startRank + 2) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile + 1 < 8 && startRank - 2 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile + 1, startRank - 2) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank + 2 < 8)
			{
				if (MoveOrBreak(result, fromItem, startFile - 1, startRank + 2) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank - 2 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile - 1, startRank - 2) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile + 2 < 8 && startRank + 1 < 8)
			{
				if (MoveOrBreak(result, fromItem, startFile + 2, startRank + 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 2 >= 0 && startRank + 1 < 8)
			{
				if (MoveOrBreak(result, fromItem, startFile - 2, startRank + 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile + 2 < 8 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile + 2, startRank - 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 2 >= 0 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile - 2, startRank - 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			return true;
		}

		private static bool AddBishopMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			int f, r;

			f = startFile + 1;
			r = startRank + 1;
			while (f < 8 && r < 8)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}

				f++;
				r++;
			}

			f = startFile + 1;
			r = startRank - 1;
			while (f < 8 && r >= 0)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}

				f++;
				r--;
			}

			f = startFile - 1;
			r = startRank + 1;
			while (f >= 0 && r < 8)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}

				f--;
				r++;
			}

			f = startFile - 1;
			r = startRank - 1;
			while (f >= 0 && r >= 0)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}

				f--;
				r--;
			}

			return true;
		}

		private static bool AddQueenMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			if (!AddRookMoves(result, fromItem))
			{
				return false;
			}

			if (!AddBishopMoves(result, fromItem))
			{
				return false;
			}

			return true;
		}

		private static bool AddKingMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			if (startFile + 1 <= 7)
			{
				if (MoveOrBreak(result, fromItem, startFile + 1, startRank) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile - 1, startRank) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startRank + 1 <= 7)
			{
				if (MoveOrBreak(result, fromItem, startFile, startRank + 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startRank - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile, startRank - 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile + 1 <= 7 && startRank + 1 <= 7)
			{
				if (MoveOrBreak(result, fromItem, startFile + 1, startRank + 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile + 1 <= 7 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile + 1, startRank - 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank + 1 <= 7)
			{
				if (MoveOrBreak(result, fromItem, startFile - 1, startRank + 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(result, fromItem, startFile - 1, startRank - 1) == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			return true;
		}

		private static bool AddRookMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			//Смотрим все ходы направо
			for (var f = startFile + 1; f <= 7; f++)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, startRank);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			//Смотрим все ходы налево
			for (var f = startFile - 1; f >= 0; f--)
			{
				var moveResult = MoveOrBreak(result, fromItem, f, startRank);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			//Смотрим все ходы вверх
			for (var r = startRank + 1; r <= 7; r++)
			{
				var moveResult = MoveOrBreak(result, fromItem, startFile, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			//Смотрим все ходы вверх
			for (var r = startRank - 1; r >= 0; r--)
			{
				var moveResult = MoveOrBreak(result, fromItem, startFile, r);
				if (moveResult == EMoveResult.Impossible || moveResult == EMoveResult.Capture)
				{
					break;
				}
				if (moveResult == EMoveResult.KingCapture)
				{
					return false;
				}
			}

			return true;
		}

		private static bool AddPawnMoves(ICollection<CMove> result, CMoveItem fromItem)
		{
			var piece = fromItem.Piece;
			var player = piece.Player;
			var board = fromItem.Board;
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			//Следующий горизонталь по направлению игры
			var nextRank = player == EPlayer.White ? startRank + 1 : startRank - 1;

			//Начальное положение пешки
			var smallIndex = player == EPlayer.White ? 1 : 6;

			//Двойной ход пешки
			var largeIndex = player == EPlayer.White ? 3 : 4;

			//Предпоследний индекс
			var penultimateIndex = player == EPlayer.White ? 6 : 1;


			if (board[startFile, nextRank] == null)
			{
				var remove = new[] { fromItem };

				if (startRank == penultimateIndex)
				{
					//Ход без взятия с превращением
					//result.Add(new CMoveSimplePromotion(piece, new CPieceQueen(piece.Player)));
					//result.Add(new CMoveSimplePromotion(piece, new CPieceRook(piece.Player)));
					//result.Add(new CMoveSimplePromotion(piece, new CPieceBishop(piece.Player)));
					//result.Add(new CMoveSimplePromotion(piece, new CPieceKnight(piece.Player)));
					
					result.Add(new CMove(remove, new[] { new CMoveItem(board, new CPieceQueen(piece.Player), startFile, nextRank) }));
					result.Add(new CMove(remove, new[] { new CMoveItem(board, new CPieceRook(piece.Player), startFile, nextRank) }));
					result.Add(new CMove(remove, new[] { new CMoveItem(board, new CPieceBishop(piece.Player), startFile, nextRank) }));
					result.Add(new CMove(remove, new[] { new CMoveItem(board, new CPieceKnight(piece.Player), startFile, nextRank) }));
				}
				else
				{
					//Ход без взятия без превращения
					//result.Add(new CMoveSimple(piece, file, nextRank));
					result.Add(new CMove(remove, new[] { new CMoveItem(board, piece, startFile, nextRank) }));

				}

				if (startRank == smallIndex && board[startFile, largeIndex] == null)
				{
					//Двойной ход пешки
					//result.Add(new CMoveSimple(piece, file, largeIndex));
					result.Add(new CMove(remove, new[] { new CMoveItem(board, piece, startFile, largeIndex) }));
				}
			}

			if (startFile < 7)
			{
				var file = startFile + 1;
				var rank = nextRank;
				var partner = board[file, rank];
				if (partner != null && partner.Player != player)
				{
					if (partner.Type == EPieceType.King)
					{
						return false;
					}
					
					var remove = new[] { fromItem, new CMoveItem(board, partner, file, rank) };

					if (startRank == penultimateIndex)
					{
						//Ход со взятием с превращением
						//result.Add(new CMoveCapturePromotion(piece, new CPieceQueen(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceRook(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceBishop(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceKnight(piece.Player), partner));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceQueen(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceRook(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceBishop(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceKnight(piece.Player), file, rank) }));
					}
					else
					{
						//Ход со взятием без превращения
						//result.Add(new CMoveCapture(piece, partner));

						result.Add(new CMove(remove, new[] { new CMoveItem(board, piece, file, rank) }));
					}
				}
			}

			if (startFile > 0)
			{
				var file = startFile - 1;
				var rank = nextRank;
				var partner = board[file, rank];
				if (partner != null && partner.Player != player)
				{
					if (partner.Type == EPieceType.King)
					{
						return false;
					}

					var remove = new[] { fromItem, new CMoveItem(board, partner, file, rank) };

					if (startRank == penultimateIndex)
					{
						//Ход со взятием с превращением
						//result.Add(new CMoveCapturePromotion(piece, new CPieceQueen(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceRook(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceBishop(piece.Player), partner));
						//result.Add(new CMoveCapturePromotion(piece, new CPieceKnight(piece.Player), partner));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceQueen(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceRook(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceBishop(piece.Player), file, rank) }));

						result.Add(new CMove(remove,
							new[] { new CMoveItem(board, new CPieceKnight(piece.Player), file, rank) }));
					}
					else
					{
						//Ход со взятием без превращения
						//result.Add(new CMoveCapture(piece, partner));

						result.Add(new CMove(remove, new[] { new CMoveItem(board, piece, file, rank) }));
					}
				}
			}

			return true;
		}

		//private class CMoveEnumerator : IEnumerator<CMove>
		//{
		//	public void Dispose()
		//	{
		//	}

		//	public bool MoveNext()
		//	{
		//		throw new System.NotImplementedException();
		//	}

		//	public void Reset()
		//	{
		//	}

		//	public CMove Current { get; }

		//	object IEnumerator.Current => Current;
		//}


		//private class CMoveEnumerable : IEnumerable<CMove>
		//{
		//	public IEnumerator<CMove> GetEnumerator()
		//	{
		//		throw new System.NotImplementedException();
		//	}

		//	IEnumerator IEnumerable.GetEnumerator()
		//	{
		//		return GetEnumerator();
		//	}
		//}

		public IEnumerable<CMove> GetAllMoves(EPlayer player)
		{
			var result = new List<CMove>();


			var kingResult = true;
			for (var file = 0; file < 8; file++)
			{
				for (var rank = 0; rank < 8; rank++)
				{
					var piece = Board[file, rank];

					if (piece == null || piece.Player != player)
					{
						continue;
					}

					var fromItem = new CMoveItem(Board, piece, file, rank);

					switch (piece.Type)
					{
						case EPieceType.Pawn:
							kingResult = AddPawnMoves(result, fromItem);
							break;
						case EPieceType.King:
							kingResult = AddKingMoves(result, fromItem);
							break;
						case EPieceType.Queen:
							kingResult = AddQueenMoves(result, fromItem);
							break;
						case EPieceType.Rook:
							kingResult = AddRookMoves(result, fromItem);
							break;
						case EPieceType.Bishop:
							kingResult = AddBishopMoves(result, fromItem);
							break;
						case EPieceType.Knight:
							kingResult = AddKnightMoves(result, fromItem);
							break;
					}

					if (!kingResult)
					{
						return null;
					}
				}
			}


			return result;
		}


		public enum EAnalizeResult
		{
			Normal,

			/// <summary>
			/// Невозможная комбинация.
			/// </summary>
			Impossible,

			/// <summary>
			/// Мат или пат.
			/// </summary>
			Mate
		}

		public EAnalizeResult Analyze(EPlayer player, int depth, out int counter)
		{
			counter = 0;

			if (depth == 0)
			{
				return EAnalizeResult.Normal;
			}


			var moves = GetAllMoves(player);

			if (moves == null)
			{
				return EAnalizeResult.Impossible;
			}

			foreach (var move in moves)
			{
				move.Do();
				counter++;

				var counterMoves = GetAllMoves(1 - player);

				if (counterMoves != null)
				{
					foreach (var counterMove in counterMoves)
					{
						counterMove.Do();
						counter++;

						int count;
						Analyze(player, depth - 1, out count);
						counter += count;

						counterMove.Undo();
					}
				}

				move.Undo();
			}
			
			return EAnalizeResult.Normal;
		}
	}
}