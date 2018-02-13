using System.Text.RegularExpressions;

namespace Chess
{
	public class CSquare
	{
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
				return null;
			}

			var file = match.Groups[1].Value.ToUpper()[0] - 'A';
			var rank = int.Parse(match.Groups[2].Value) - 1;
			return new CSquare(file, rank);
		}
	}
}