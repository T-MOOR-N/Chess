using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using Chess;
using System.Windows;
using Chess.Enums;
using Chess.Figures;

namespace ChessApp
{
	public class ChessBoardControl : Control
	{

		public CBoard Board
		{
			get { return (CBoard)GetValue(BoardProperty); }
			set { SetValue(BoardProperty, value); }
		}

		private static readonly DependencyProperty BoardProperty = DependencyProperty.Register(nameof(Board),
			typeof(CBoard), typeof(ChessBoardControl));


		private void DrawPiece(DrawingContext drawingContext, CPiece piece, Rect square)
		{
			if (piece == null)
			{
				return;
			}

			var foreground = piece.Player == EPlayer.Black ? Brushes.Black : Brushes.White;

			var typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
			var formattedText = new FormattedText(piece.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace,
				FontSize, foreground)
			{
				MaxTextWidth = square.Width,
				MaxTextHeight = square.Height,
				Trimming = TextTrimming.CharacterEllipsis
			};

			var x = square.Left + (square.Width - formattedText.Width)/2D;
			var y = square.Top + (square.Height - formattedText.Height) / 2D;
			
			
			drawingContext.DrawText(formattedText, new Point(x, y));
		}


		private static CBoard GetDefaultBoard()
		{
			var result = new CBoard
			{
				["A1"] = new CPieceRook(EPlayer.White),
				["B1"] = new CPieceKnight(EPlayer.White),
				["C1"] = new CPieceBishop(EPlayer.White),
				["D1"] = new CPieceQueen(EPlayer.White),
				["E1"] = new CPieceKing(EPlayer.White),
				["F1"] = new CPieceBishop(EPlayer.White),
				["G1"] = new CPieceKnight(EPlayer.White),
				["H1"] = new CPieceRook(EPlayer.White),

				["A2"] = new CPiecePawn(EPlayer.White),
				["B2"] = new CPiecePawn(EPlayer.White),
				["C2"] = new CPiecePawn(EPlayer.White),
				["D2"] = new CPiecePawn(EPlayer.White),
				["E2"] = new CPiecePawn(EPlayer.White),
				["F2"] = new CPiecePawn(EPlayer.White),
				["G2"] = new CPiecePawn(EPlayer.White),
				["H2"] = new CPiecePawn(EPlayer.White),

				["A8"] = new CPieceRook(EPlayer.Black),
				["B8"] = new CPieceKnight(EPlayer.Black),
				["C8"] = new CPieceBishop(EPlayer.Black),
				["D8"] = new CPieceQueen(EPlayer.Black),
				["E8"] = new CPieceKing(EPlayer.Black),
				["F8"] = new CPieceBishop(EPlayer.Black),
				["G8"] = new CPieceKnight(EPlayer.Black),
				["H8"] = new CPieceRook(EPlayer.Black),

				["A7"] = new CPiecePawn(EPlayer.Black),
				["B7"] = new CPiecePawn(EPlayer.Black),
				["C7"] = new CPiecePawn(EPlayer.Black),
				["D7"] = new CPiecePawn(EPlayer.Black),
				["E7"] = new CPiecePawn(EPlayer.Black),
				["F7"] = new CPiecePawn(EPlayer.Black),
				["G7"] = new CPiecePawn(EPlayer.Black),
				["H7"] = new CPiecePawn(EPlayer.Black),
			};

			return result;
		}
		
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			var main = new Rect(0,0, ActualWidth, ActualHeight);
			drawingContext.DrawRectangle(Background, null, main);

			var size = Math.Min(ActualWidth, ActualHeight);
			var left = (main.Width - size)/2D;
			var top = (main.Height - size)/2D;

			var boardRect = new Rect(left, top, size, size);



			//var whiteBrush = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/"
			//                                                        + Assembly.GetExecutingAssembly().GetName().Name
			//                                                        + ";component/"
			//                                                        + "Images/White.jpg", UriKind.Absolute)));
			
			var board = Board ?? GetDefaultBoard();

			for (var i = 0; i < 8; i++)
			{
				for (var j = 0; j < 8; j++)
				{
					var l = boardRect.Left + i*size/8;
					var b = boardRect.Bottom - j*size/8;
					var r = l + size / 8;
					var t = b - size / 8;

					var square = new Rect(l, t, r - l, b - t);
					var brush = (i + j)%2 == 0 ? Brushes.Peru : Brushes.PeachPuff;

					drawingContext.DrawRectangle(brush, null, square);

					DrawPiece(drawingContext, board[i, j], square);
				}
			}
		}
	}
}
