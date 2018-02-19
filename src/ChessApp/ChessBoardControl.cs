using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using Chess;
using System.Windows;
using Chess.Enums;

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


		public double SquareSize => (double)GetValue(SquareSizeProperty);

		private static readonly DependencyProperty SquareSizeProperty = DependencyProperty.Register(nameof(SquareSize),
			typeof(double), typeof(ChessBoardControl));

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == ActualWidthProperty || e.Property == ActualHeightProperty)
			{
				var size = Math.Min(ActualWidth, ActualHeight);
				SetValue(SquareSizeProperty, size/8D);
			}

			InvalidateVisual();
		}

		private static readonly Color BlackColor = Color.FromRgb(110, 110, 110);
		private static readonly Color WhiteColor = Color.FromRgb(140, 140, 140);
		private static readonly Color TextColor = Color.FromArgb(250, 125, 125, 125);

		private static readonly SolidColorBrush BlackBrush = new SolidColorBrush(BlackColor);
		private static readonly SolidColorBrush WhiteBrush = new SolidColorBrush(WhiteColor);
		private static readonly SolidColorBrush TextBrush = new SolidColorBrush(TextColor);

		private static void DrawText(DrawingContext drawingContext, Rect area, Typeface typeFace, double fontSize, Brush foreground, string text)
		{
			var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace,
				fontSize, foreground)
			{
				MaxTextWidth = area.Width*1.5,
				MaxTextHeight = area.Height*1.5,
				Trimming = TextTrimming.CharacterEllipsis
			};

			var x = area.Left + (area.Width - formattedText.Width) / 2D;
			var y = area.Top + (area.Height - formattedText.Height) / 2D;

			drawingContext.DrawText(formattedText, new Point(x, y));
		}

		private static void DrawField(DrawingContext drawingContext, CBoard board, Rect area, int coorX, int coorY,
			Typeface typeFace, double fontSize)
		{

			var squareWidth = area.Width / 8D;
			var squareHeight = area.Height / 8D;

			var l = area.Left + coorX * squareWidth;
			var b = area.Bottom - coorY * squareHeight;
			var r = l + squareWidth;
			var t = b - squareHeight;

			l = Math.Floor(l + 0.5D);
			t = Math.Floor(t + 0.5D);
			r = Math.Floor(r + 0.5D);
			b = Math.Floor(b + 0.5D);

			var square = new Rect(l, t, r - l, b - t);
			if (square.Width <= 0 || square.Height <= 0D)
			{
				return;
			}

			var brush = (coorX + coorY)%2 == 0 ? BlackBrush : WhiteBrush;

			drawingContext.DrawRectangle(brush, null, square);
			if (true)
			{
				DrawText(drawingContext, square, typeFace, squareWidth*0.5D, TextBrush, new CSquare(coorX, coorY).ToString());
			}


			var piece = board[coorX, coorY];

			if (piece != null)
			{
				var foreground = piece.Player == EPlayer.Black ? Brushes.Black : Brushes.White;
				DrawText(drawingContext, square, typeFace, fontSize, foreground, piece.ToString());
			}
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

			var main = new Rect(0,0, ActualWidth, ActualHeight);
			drawingContext.DrawRectangle(Background, null, main);

			var size = SquareSize*8D;
			var left = (main.Width - size)/2D;
			var top = (main.Height - size)/2D;
			var area = new Rect(left, top, size, size);
			var board = Board ?? CBoard.GetDefaultBoard();
			var typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

			for (var x = 0; x < 8; x++)
			{
				for (var y = 0; y < 8; y++)
				{
					DrawField(drawingContext, board, area, x, y, typeFace, FontSize);
				}
			}
		}
	}
}
