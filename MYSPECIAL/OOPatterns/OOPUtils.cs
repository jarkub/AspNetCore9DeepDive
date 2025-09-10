using System.Text;

namespace iMonnit8.OOPatterns
{
	public static class OOPUtils
	{
		public static string PrintThenReturnResultString(StringBuilder sb)
		{
			var result = sb.ToString();
			Console.WriteLine(result);
			return result;
		}
	}
}
