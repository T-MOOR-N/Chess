using System;
using System.Text.RegularExpressions;

namespace Chess
{
	public struct CSquare : IEquatable<CSquare>
	{
		#region Statics

		public static readonly CSquare A1 = new CSquare(0, 0);
		public static readonly CSquare A2 = new CSquare(0, 1);
		public static readonly CSquare A3 = new CSquare(0, 2);
		public static readonly CSquare A4 = new CSquare(0, 3);
		public static readonly CSquare A5 = new CSquare(0, 4);
		public static readonly CSquare A6 = new CSquare(0, 5);
		public static readonly CSquare A7 = new CSquare(0, 6);
		public static readonly CSquare A8 = new CSquare(0, 7);

		public static readonly CSquare B1 = new CSquare(1, 0);
		public static readonly CSquare B2 = new CSquare(1, 1);
		public static readonly CSquare B3 = new CSquare(1, 2);
		public static readonly CSquare B4 = new CSquare(1, 3);
		public static readonly CSquare B5 = new CSquare(1, 4);
		public static readonly CSquare B6 = new CSquare(1, 5);
		public static readonly CSquare B7 = new CSquare(1, 6);
		public static readonly CSquare B8 = new CSquare(1, 7);

		public static readonly CSquare C1 = new CSquare(2, 0);
		public static readonly CSquare C2 = new CSquare(2, 1);
		public static readonly CSquare C3 = new CSquare(2, 2);
		public static readonly CSquare C4 = new CSquare(2, 3);
		public static readonly CSquare C5 = new CSquare(2, 4);
		public static readonly CSquare C6 = new CSquare(2, 5);
		public static readonly CSquare C7 = new CSquare(2, 6);
		public static readonly CSquare C8 = new CSquare(2, 7);

		public static readonly CSquare D1 = new CSquare(3, 0);
		public static readonly CSquare D2 = new CSquare(3, 1);
		public static readonly CSquare D3 = new CSquare(3, 2);
		public static readonly CSquare D4 = new CSquare(3, 3);
		public static readonly CSquare D5 = new CSquare(3, 4);
		public static readonly CSquare D6 = new CSquare(3, 5);
		public static readonly CSquare D7 = new CSquare(3, 6);
		public static readonly CSquare D8 = new CSquare(3, 7);

		public static readonly CSquare E1 = new CSquare(4, 0);
		public static readonly CSquare E2 = new CSquare(4, 1);
		public static readonly CSquare E3 = new CSquare(4, 2);
		public static readonly CSquare E4 = new CSquare(4, 3);
		public static readonly CSquare E5 = new CSquare(4, 4);
		public static readonly CSquare E6 = new CSquare(4, 5);
		public static readonly CSquare E7 = new CSquare(4, 6);
		public static readonly CSquare E8 = new CSquare(4, 7);

		public static readonly CSquare F1 = new CSquare(5, 0);
		public static readonly CSquare F2 = new CSquare(5, 1);
		public static readonly CSquare F3 = new CSquare(5, 2);
		public static readonly CSquare F4 = new CSquare(5, 3);
		public static readonly CSquare F5 = new CSquare(5, 4);
		public static readonly CSquare F6 = new CSquare(5, 5);
		public static readonly CSquare F7 = new CSquare(5, 6);
		public static readonly CSquare F8 = new CSquare(5, 7);

		public static readonly CSquare G1 = new CSquare(6, 0);
		public static readonly CSquare G2 = new CSquare(6, 1);
		public static readonly CSquare G3 = new CSquare(6, 2);
		public static readonly CSquare G4 = new CSquare(6, 3);
		public static readonly CSquare G5 = new CSquare(6, 4);
		public static readonly CSquare G6 = new CSquare(6, 5);
		public static readonly CSquare G7 = new CSquare(6, 6);
		public static readonly CSquare G8 = new CSquare(6, 7);

		public static readonly CSquare H1 = new CSquare(7, 0);
		public static readonly CSquare H2 = new CSquare(7, 1);
		public static readonly CSquare H3 = new CSquare(7, 2);
		public static readonly CSquare H4 = new CSquare(7, 3);
		public static readonly CSquare H5 = new CSquare(7, 4);
		public static readonly CSquare H6 = new CSquare(7, 5);
		public static readonly CSquare H7 = new CSquare(7, 6);
		public static readonly CSquare H8 = new CSquare(7, 7);

		#endregion

		public int File { get; }

		public int Rank { get; }

		public CSquare(int file, int rank)
		{
			File = file;
			Rank = rank;
		}

		private static readonly Regex SquareRegex = new Regex(@"^\s*([a-h])\s*([1-8])\s*$",
			RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static CSquare Parse(string text)
		{
			var match = SquareRegex.Match(text);
			if (!match.Success)
			{
				throw new ArgumentException("Неправильный формат");
			}

			var file = match.Groups[1].Value.ToUpper()[0] - 'A';
			var rank = int.Parse(match.Groups[2].Value) - 1;
			return new CSquare(file, rank);
		}

		public override bool Equals(object obj)
		{
			return obj is CSquare && Equals((CSquare) obj);
		}

		public bool Equals(CSquare other)
		{
			return File == other.File && Rank == other.Rank;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (File * 397) ^ Rank;
			}
		}

		public override string ToString()
		{
			return $"{(char) ('a' + File)}{Rank + 1}";
		}
	}
}