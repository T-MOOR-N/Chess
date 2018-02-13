using System;
using System.Collections.Generic;
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

			private static IEnumerable<CMove> GetAllPawnMoves(CPiecePawn pawn)
			{
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
				
				var result = new List<CMove>();

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
					}
				}

				return result;
			}
		}

		public MainWindow()
		{
			InitializeComponent();

			//var whitePawn = new CPiecePawn(EPlayer.White);
			//var blackQueen = new CPieceQueen(EPlayer.Black);

			var board = CBoard.GetDefaultBoard();
			ChessBoard.Board = board;

			board["B3"] = new CPiecePawn(EPlayer.Black);
			board["A7"] = new CPiecePawn(EPlayer.White);
			board["A8"] = null;
			

			var game = new CGame(board);
			var moves = game.GetAllMoves(EPlayer.White);
			
			var move = moves[8];
			move.Do();
			move.Undo();

		}
	}
}
