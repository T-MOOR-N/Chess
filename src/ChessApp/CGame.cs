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

		private static bool MoveOrBreak(ICollection<CMove> listToAdd, CPiece piece, int file, int rank)
		{
			var partner = piece.Board[file, rank];

			if (partner == null)
			{
				//��� ��� ������
				listToAdd.Add(new CMoveSimple(piece, file, rank));
				return false;
			}

			if (partner.Player != piece.Player)
			{
				//��� �� �������
				listToAdd.Add(new CMoveCapture(piece, partner));
			}

			return true;
		}

		private static IEnumerable<CMove> GetAllKnightMoves(CPiece piece)
		{
			var result = new List<CMove>();
			var file = piece.File;
			var rank = piece.Rank;

			if (file + 1 < 8 && rank + 2 < 8)
			{
				MoveOrBreak(result, piece, file + 1, rank + 2);
			}

			if (file + 1 < 8 && rank - 2 >= 0)
			{
				MoveOrBreak(result, piece, file + 1, rank - 2);
			}

			if (file - 1 >= 0 && rank + 2 < 8)
			{
				MoveOrBreak(result, piece, file - 1, rank + 2);
			}

			if (file - 1 >= 0 && rank - 2 >= 0)
			{
				MoveOrBreak(result, piece, file - 1, rank - 2);
			}

			if (file + 2 < 8 && rank + 1 < 8)
			{
				MoveOrBreak(result, piece, file + 2, rank + 1);
			}

			if (file - 2 >= 0 && rank + 1 < 8)
			{
				MoveOrBreak(result, piece, file - 2, rank + 1);
			}

			if (file + 2 < 8 && rank - 1 >= 0)
			{
				MoveOrBreak(result, piece, file + 2, rank - 1);
			}

			if (file - 2 >= 0 && rank - 1 >= 0)
			{
				MoveOrBreak(result, piece, file - 2, rank - 1);
			}

			return result;
		}

		private static IEnumerable<CMove> GetAllBishopMoves(CPiece piece)
		{
			var result = new List<CMove>();
			var file = piece.File;
			var rank = piece.Rank;

			int f, r;

			f = file + 1;
			r = rank + 1;
			while (f < 8 && r < 8)
			{
				if (MoveOrBreak(result, piece, f, r))
				{
					break;
				}

				f++;
				r++;
			}

			f = file + 1;
			r = rank - 1;
			while (f < 8 && r >= 0)
			{
				if (MoveOrBreak(result, piece, f, r))
				{
					break;
				}

				f++;
				r--;
			}

			f = file - 1;
			r = rank + 1;
			while (f >= 0 && r < 8)
			{
				if (MoveOrBreak(result, piece, f, r))
				{
					break;
				}

				f--;
				r++;
			}

			f = file - 1;
			r = rank - 1;
			while (f >= 0 && r >= 0)
			{
				if (MoveOrBreak(result, piece, f, r))
				{
					break;
				}

				f--;
				r--;
			}

			return result;
		}

		private static IEnumerable<CMove> GetAllQueenMoves(CPiece piece)
		{
			var result = new List<CMove>();
			result.AddRange(GetAllRookMoves(piece));
			result.AddRange(GetAllBishopMoves(piece));
			return result;
		}

		private static IEnumerable<CMove> GetAllKingMoves(CPiece piece)
		{
			var result = new List<CMove>();
			var file = piece.File;
			var rank = piece.Rank;
			
			if (file + 1 <= 7)
			{
				MoveOrBreak(result, piece, file + 1, rank);
			}

			if (file - 1 >= 0)
			{
				MoveOrBreak(result, piece, file - 1, rank);
			}

			if (rank + 1 <= 7)
			{
				MoveOrBreak(result, piece, file, rank + 1);
			}

			if (rank - 1 >= 0)
			{
				MoveOrBreak(result, piece, file, rank - 1);
			}

			if (file + 1 <= 7 && rank + 1 <= 7)
			{
				MoveOrBreak(result, piece, file + 1, rank + 1);
			}

			if (file + 1 <= 7 && rank - 1 >= 0)
			{
				MoveOrBreak(result, piece, file + 1, rank - 1);
			}

			if (file - 1 >= 0 && rank + 1 <= 7)
			{
				MoveOrBreak(result, piece, file - 1, rank + 1);
			}

			if (file - 1 >= 0 && rank - 1 >= 0)
			{
				MoveOrBreak(result, piece, file - 1, rank - 1);
			}

			//Todo:���������� ������

			return result;
		}

		private static IEnumerable<CMove> GetAllRookMoves(CPiece piece)
		{
			var result = new List<CMove>();
			var file = piece.File;
			var rank = piece.Rank;

			//������� ��� ���� �������
			for (var f = file + 1; f <= 7; f++)
			{
				if (MoveOrBreak(result, piece, f, rank))
				{
					break;
				}
			}

			//������� ��� ���� ������
			for (var f = file - 1; f >= 0; f--)
			{
				if (MoveOrBreak(result, piece, f, rank))
				{
					break;
				}
			}

			//������� ��� ���� �����
			for (var r = rank + 1; r <= 7; r++)
			{
				if (MoveOrBreak(result, piece, file, r))
				{
					break;
				}
			}

			//������� ��� ���� �����
			for (var r = rank - 1; r >= 0; r--)
			{
				if (MoveOrBreak(result, piece, file, r))
				{
					break;
				}
			}

			return result;
		}

		private static IEnumerable<CMove> GetAllPawnMoves(CPiecePawn pawn)
		{
			var result = new List<CMove>();
			var player = pawn.Player;
			var board = pawn.Board;
			var file = pawn.File;
			var rank = pawn.Rank;

			//��������� ����������� �� ����������� ����
			var nextRank = player == EPlayer.White ? rank + 1 : rank - 1;

			//��������� ��������� �����
			var smallIndex = player == EPlayer.White ? 1 : 6;

			//������� ��� �����
			var largeIndex = player == EPlayer.White ? 3 : 4;

			//������������� ������
			var penultimateIndex = player == EPlayer.White ? 6 : 1;


			if (board[file, nextRank] == null)
			{
				if (rank == penultimateIndex)
				{
					//��� ��� ������ � ������������
					result.Add(new CMoveSimplePromotion(pawn, new CPieceQueen(pawn.Player)));
					result.Add(new CMoveSimplePromotion(pawn, new CPieceRook(pawn.Player)));
					result.Add(new CMoveSimplePromotion(pawn, new CPieceBishop(pawn.Player)));
					result.Add(new CMoveSimplePromotion(pawn, new CPieceKnight(pawn.Player)));
				}
				else
				{
					//��� ��� ������ ��� �����������
					result.Add(new CMoveSimple(pawn, file, nextRank));
				}

				if (rank == smallIndex && board[file, largeIndex] == null)
				{
					//������� ��� �����
					result.Add(new CMoveSimple(pawn, file, largeIndex));
				}
			}

			if (file < 7)
			{
				var partner = board[file + 1, nextRank];
				if (partner != null && partner.Player != player)
				{
					if (rank == penultimateIndex)
					{
						//��� �� ������� � ������������
						result.Add(new CMoveCapturePromotion(pawn, new CPieceQueen(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceRook(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceBishop(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceKnight(pawn.Player), partner));
					}
					else
					{
						//��� �� ������� ��� �����������
						result.Add(new CMoveCapture(pawn, partner));
					}
				}
			}

			if (file > 0)
			{
				var partner = board[file - 1, nextRank];
				if (partner != null && partner.Player != player)
				{
					if (rank == penultimateIndex)
					{
						//��� �� ������� � ������������
						result.Add(new CMoveCapturePromotion(pawn, new CPieceQueen(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceRook(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceBishop(pawn.Player), partner));
						result.Add(new CMoveCapturePromotion(pawn, new CPieceKnight(pawn.Player), partner));
					}
					else
					{
						//��� �� ������� ��� �����������
						result.Add(new CMoveCapture(pawn, partner));
					}
				}
			}

			return result;
		}
		
		public List<CMove> GetAllMoves(EPlayer player)
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

					if (piece is CPiecePawn)
					{
						result.AddRange(GetAllPawnMoves(piece as CPiecePawn));
					}

					if (piece is CPieceKing)
					{
						result.AddRange(GetAllKingMoves(piece));
					}

					if (piece is CPieceQueen)
					{
						result.AddRange(GetAllQueenMoves(piece));
					}

					if (piece is CPieceRook)
					{
						result.AddRange(GetAllRookMoves(piece));
					}

					if (piece is CPieceBishop)
					{
						result.AddRange(GetAllBishopMoves(piece));
					}

					if (piece is CPieceKnight)
					{
						result.AddRange(GetAllKnightMoves(piece));
					}
				}
			}

			return result;
		}

		
	}
}