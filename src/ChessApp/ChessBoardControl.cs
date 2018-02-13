using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using Chess;
using System.Windows;
using Chess.Enums;
using Chess.Pieces;

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
			
			var board = Board ?? CBoard.GetDefaultBoard();

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
