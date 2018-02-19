using System;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Enums;
using Chess.Moves;
using Chess.Pieces;

namespace ChessApp
{
	public class CGame
	{
		public struct CAnalyzeResult
		{
			public CMove BestMove { get; }
			public double BestMark { get; }
			public int Iterations { get; }

			public CAnalyzeResult(CMove bestMove, double bestMark, int iterations)
			{
				BestMove = bestMove;
				BestMark = bestMark;
				Iterations = iterations;
			}
		}
		
		private Random _random = new Random();

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
			Continue = 0,
			Break = 1,
			Return = 2

		}

		private static EMoveResult MoveOrBreak(ICollection<CMove> listToAdd, CMoveItem fromItem, int file, int rank)
		{
			var piece = fromItem.Piece;
			var partner = fromItem.Board[file, rank];
			var board = fromItem.Board;

			if (partner == null)
			{
				//Ход без взятия
				var move = new CMove(
					new[] {fromItem},
					new[] {new CMoveItem(board, piece, file, rank)}
					);

				listToAdd.Add(move);

				return EMoveResult.Continue;
			}

			if (partner.Player != piece.Player)
			{
				//Ход со взятием
				var move = new CMove(
					new[] {fromItem, new CMoveItem(board, partner, file, rank)},
					new[] {new CMoveItem(board, piece, file, rank)}
					);

				listToAdd.Add(move);

				if (partner.Type == EPieceType.King)
				{
					return EMoveResult.Return;
				}

			}

			return EMoveResult.Break;
		}

		private static bool AddKnightMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			if (startFile + 1 < 8 && startRank + 2 < 8)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 1, startRank + 2) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile + 1 < 8 && startRank - 2 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 1, startRank - 2) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank + 2 < 8)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 1, startRank + 2) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank - 2 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 1, startRank - 2) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile + 2 < 8 && startRank + 1 < 8)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 2, startRank + 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 2 >= 0 && startRank + 1 < 8)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 2, startRank + 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile + 2 < 8 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 2, startRank - 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 2 >= 0 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 2, startRank - 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			return true;
		}

		private static bool AddBishopMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			int f, r;

			f = startFile + 1;
			r = startRank + 1;
			while (f < 8 && r < 8)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}

				f++;
				r++;
			}

			f = startFile + 1;
			r = startRank - 1;
			while (f < 8 && r >= 0)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}

				f++;
				r--;
			}

			f = startFile - 1;
			r = startRank + 1;
			while (f >= 0 && r < 8)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}

				f--;
				r++;
			}

			f = startFile - 1;
			r = startRank - 1;
			while (f >= 0 && r >= 0)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}

				f--;
				r--;
			}

			return true;
		}

		private static bool AddQueenMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
		{
			if (!AddRookMoves(listToAdd, fromItem))
			{
				return false;
			}

			if (!AddBishopMoves(listToAdd, fromItem))
			{
				return false;
			}

			return true;
		}

		private static bool AddKingMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			if (startFile + 1 <= 7)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 1, startRank) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 1, startRank) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startRank + 1 <= 7)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile, startRank + 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startRank - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile, startRank - 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile + 1 <= 7 && startRank + 1 <= 7)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 1, startRank + 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile + 1 <= 7 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile + 1, startRank - 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank + 1 <= 7)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 1, startRank + 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			if (startFile - 1 >= 0 && startRank - 1 >= 0)
			{
				if (MoveOrBreak(listToAdd, fromItem, startFile - 1, startRank - 1) == EMoveResult.Return)
				{
					return false;
				}
			}

			return true;
		}

		private static bool AddRookMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
		{
			var startFile = fromItem.File;
			var startRank = fromItem.Rank;

			//Смотрим все ходы направо
			for (var f = startFile + 1; f <= 7; f++)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, startRank);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}
			}

			//Смотрим все ходы налево
			for (var f = startFile - 1; f >= 0; f--)
			{
				var result = MoveOrBreak(listToAdd, fromItem, f, startRank);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}
			}

			//Смотрим все ходы вверх
			for (var r = startRank + 1; r <= 7; r++)
			{
				var result = MoveOrBreak(listToAdd, fromItem, startFile, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}
			}

			//Смотрим все ходы вверх
			for (var r = startRank - 1; r >= 0; r--)
			{
				var result = MoveOrBreak(listToAdd, fromItem, startFile, r);
				if (result == EMoveResult.Return)
				{
					return false;
				}
				if (result == EMoveResult.Break)
				{
					break;
				}
			}

			return true;
		}

		private static bool AddPawnMoves(ICollection<CMove> listToAdd, CMoveItem fromItem)
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
				var remove = new[] {fromItem};

				if (startRank == penultimateIndex)
				{
					//Ходы без взятия с превращением
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, new CPieceQueen(piece.Player), startFile, nextRank)}));
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, new CPieceRook(piece.Player), startFile, nextRank)}));
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, new CPieceBishop(piece.Player), startFile, nextRank)}));
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, new CPieceKnight(piece.Player), startFile, nextRank)}));
				}
				else
				{
					//Ход без взятия без превращения
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, piece, startFile, nextRank)}));

				}

				if (startRank == smallIndex && board[startFile, largeIndex] == null)
				{
					//Двойной ход пешки
					listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, piece, startFile, largeIndex)}));
				}
			}

			if (startFile < 7)
			{
				var file = startFile + 1;
				var rank = nextRank;
				var partner = board[file, rank];
				if (partner != null && partner.Player != player)
				{
					var remove = new[] {fromItem, new CMoveItem(board, partner, file, rank)};

					if (startRank == penultimateIndex)
					{
						//Ходы со взятием с превращением
						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceQueen(piece.Player), file, rank)}));


						if (partner.Type == EPieceType.King)
						{
							return false;
						}

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceRook(piece.Player), file, rank)}));

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceBishop(piece.Player), file, rank)}));

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceKnight(piece.Player), file, rank)}));
					}
					else
					{
						//Ход со взятием без превращения
						listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, piece, file, rank)}));


						if (partner.Type == EPieceType.King)
						{
							return false;
						}
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
					var remove = new[] {fromItem, new CMoveItem(board, partner, file, rank)};

					if (startRank == penultimateIndex)
					{
						//Ходы со взятием с превращением
						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceQueen(piece.Player), file, rank)}));


						if (partner.Type == EPieceType.King)
						{
							return false;
						}

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceRook(piece.Player), file, rank)}));

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceBishop(piece.Player), file, rank)}));

						listToAdd.Add(new CMove(remove,
							new[] {new CMoveItem(board, new CPieceKnight(piece.Player), file, rank)}));
					}
					else
					{
						//Ход со взятием без превращения
						listToAdd.Add(new CMove(remove, new[] {new CMoveItem(board, piece, file, rank)}));


						if (partner.Type == EPieceType.King)
						{
							return false;
						}
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

		public IEnumerable<CMove> GetMoves(EPlayer player)
		{
			var result = new List<CMove>();

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
							if (!AddPawnMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
						case EPieceType.King:
							if (!AddKingMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
						case EPieceType.Queen:
							if (!AddQueenMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
						case EPieceType.Rook:
							if (!AddRookMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
						case EPieceType.Bishop:
							if (!AddBishopMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
						case EPieceType.Knight:
							if (!AddKnightMoves(result, fromItem))
							{
								return new List<CMove> {result.Last()};
							}
							break;
					}
				}
			}


			return result;
		}
		
		private double GetMark(EPlayer player)
		{
			double result = 0;
			var kingNotFound = true;

			for (var file = 0; file < 8; file++)
			{
				for (var rank = 0; rank < 8; rank++)
				{
					var piece = Board[file, rank];
					if (piece == null)
					{
						continue;
					}

					if (piece.Type == EPieceType.King)
					{
						if (piece.Player == player)
						{
							kingNotFound = false;
						}

						continue;
					}


					if (piece.Type == EPieceType.Pawn)
					{
						switch (player)
						{
							case EPlayer.White:
								result += rank;
								break;
							case EPlayer.Black:
								result += rank - 7;
								break;
						}
					}

					result += piece.Mark;
				}
			}

			//Временно
			//result += _random.Next(1000) / 1000000D;

			return kingNotFound ? double.NegativeInfinity : (player == EPlayer.White ? result : -result);
		}
		
		private double GetMark(EPlayer player, int depth, ref int iterations)
		{
			if (depth == 1)
			{
				iterations++;
				return GetMark(player);
			}

			var result = double.PositiveInfinity;
			foreach (var counterMove in GetMoves(1 - player))
			{
				//Делаем ход фигурой противника
				counterMove.Do();

				var analyze = Analyze(player, depth - 1);
				iterations += analyze.Iterations;
				result = Math.Min(result, analyze.BestMark);

				//Откатываем ход фигурой противника
				counterMove.Undo();
			}
			return result;
		}

		public CAnalyzeResult Analyze(EPlayer player, int depth)
		{
			if (depth <= 0)
			{
				throw new ArgumentException($"Значение параметра \"{nameof(depth)}\" должен быть больше 0");
			}
			
			var maxMark = double.NegativeInfinity;
			CMove maxMove = null;
			var iterations = 0;

			foreach (var move in GetMoves(player))
			{
				//Делаем ход
				move.Do();

				var mark = GetMark(player, depth, ref iterations);
				if (maxMark < mark)
				{
					maxMark = mark;
					maxMove = move;
				}

				//Откатываем ход
				move.Undo();
			}

			return new CAnalyzeResult(maxMove, maxMark, iterations);
		}
	}
}