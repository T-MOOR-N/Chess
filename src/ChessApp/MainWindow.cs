using System;
using System.Collections.Generic;
using System.Windows;
using Chess;
using Chess.Enums;
using Chess.Moves;
using Chess.Pieces;

namespace ChessApp
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public class CGame
		{
			public CBoard Board { get; private set; }

			public CGame(CBoard board)
			{
				Board = board;
			}

			private static bool MoveOrBreak(List<CMove> listToAdd, CPiece piece, int file, int rank)
			{
				var partner = board[file, rank];

				if (partner == null)
				{
					//Ход без взятия
					listToAdd.Add(new CMoveSimple(piece, file, rank));
					return false;
				}

				if (partner.Player != piece.Player)
				{
					//Ход со взятием
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

			private static IEnumerable<CMove> GetAllRookMoves(CPiece piece)
			{
				var result = new List<CMove>();
				var file = piece.File;
				var rank = piece.Rank;

				//Смотрим все ходы направо
				for (var f = file + 1; f <= 7; f++)
				{
					if (MoveOrBreak(result, piece, f, rank))
					{
						break;
					}
				}

				//Смотрим все ходы налево
				for (var f = file - 1; f >= 0; f--)
				{
					if (MoveOrBreak(result, piece, f, rank))
					{
						break;
					}
				}

				//Смотрим все ходы вверх
				for (var r = rank + 1; r <= 7; r++)
				{
					if (MoveOrBreak(result, piece, file, r))
					{
						break;
					}
				}

				//Смотрим все ходы вверх
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

				//Следующий горизонталь по направлению игры
				var nextRank = player == EPlayer.White ? rank + 1 : rank - 1;

				//Начальное положение пешки
				var smallIndex = player == EPlayer.White ? 1 : 6;

				//Двойной ход пешки
				var largeIndex = player == EPlayer.White ? 3 : 4;

				//Предпоследний индекс
				var penultimateIndex = player == EPlayer.White ? 6 : 1;
				

				if (board[file, nextRank] == null)
				{
					if (rank == penultimateIndex)
					{
						//Ход без взятия с превращением
						result.Add(new CMoveSimplePromotion(pawn, new CPieceQueen(pawn.Player)));
						result.Add(new CMoveSimplePromotion(pawn, new CPieceRook(pawn.Player)));
						result.Add(new CMoveSimplePromotion(pawn, new CPieceBishop(pawn.Player)));
						result.Add(new CMoveSimplePromotion(pawn, new CPieceKnight(pawn.Player)));
					}
					else
					{
						//Ход без взятия без превращения
						result.Add(new CMoveSimple(pawn, file, nextRank));
					}

					if (rank == smallIndex && board[file, largeIndex] == null)
					{
						//Двойной ход пешки
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
							//Ход со взятием с превращением
							result.Add(new CMoveCapturePromotion(pawn, new CPieceQueen(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceRook(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceBishop(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceKnight(pawn.Player), partner));
						}
						else
						{
							//Ход со взятием без превращения
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
							//Ход со взятием с превращением
							result.Add(new CMoveCapturePromotion(pawn, new CPieceQueen(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceRook(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceBishop(pawn.Player), partner));
							result.Add(new CMoveCapturePromotion(pawn, new CPieceKnight(pawn.Player), partner));
						}
						else
						{
							//Ход со взятием без превращения
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

						if (piece is CPieceQueen)
						{
							result.AddRange(GetAllQueenMoves(piece as CPieceQueen));
						}

						if (piece is CPieceRook)
						{
							result.AddRange(GetAllRookMoves(piece as CPieceRook));
						}
						
						if (piece is CPieceBishop)
						{
							result.AddRange(GetAllBishopMoves(piece as CPieceBishop));
						}

						if (piece is CPieceKnight)
						{
							result.AddRange(GetAllKnightMoves(piece as CPieceKnight));
						}
					}
				}

				return result;
			}
		}

		private static CBoard board = CBoard.GetDefaultBoard();

		private static CGame game = new CGame(board);

		public MainWindow()
		{
			InitializeComponent();

			//var whitePawn = new CPiecePawn(EPlayer.White);
			//var blackQueen = new CPieceQueen(EPlayer.Black);

			
			ChessBoard.Board = board;

			//board["B3"] = new CPiecePawn(EPlayer.Black);
			//board["A7"] = new CPiecePawn(EPlayer.White);
			//board["A8"] = null;
			
		}

		EPlayer player = EPlayer.White;

		private readonly Random random = new Random();

		private void button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var moves = game.GetAllMoves(player);

			if (moves.Count == 0)
			{
				MessageBox.Show("Все закончилось");
			}
			else
			{
				var index = random.Next(0, moves.Count);
				moves[index].Do();
				ChessBoard.InvalidateVisual();

				player = 1 - player;
			}

		}
	}
}
