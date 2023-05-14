using System.Security.Cryptography;

namespace ClassLibrary.Common.Security
{
	/// <summary>
	/// Class for generating random number.&lt;br /&gt;&lt;br /&gt;
	/// https://stackoverflow.com/a/42426750/8041900
	/// </summary>
	/// <revision>
	/// __Revisions:__~~
	/// | Contributor | Build | Revison Date | Description |~
	/// |-------------|-------|--------------|-------------|~
	/// | Christopher D. Cavell | 1.0.4.0 | 05/14/2023 | User Role Claims Development |~ 
	/// </revision>
	public class RandomGenerator
	{
		readonly RandomNumberGenerator rng;

		/// <summary>
		/// Constructor Method
		/// </summary>
		public RandomGenerator()
		{
			rng = RandomNumberGenerator.Create();
		}

		/// <summary>
		/// Returns random number 
		/// </summary>
		/// <param name="minValue">int</param>
		/// <param name="maxExclusiveValue">int</param>
		/// <returns>int</returns>
		public int Next(int minValue, int maxExclusiveValue)
		{
			if (minValue >= maxExclusiveValue)
				throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

			long diff = (long)maxExclusiveValue - minValue;
			long upperBound = uint.MaxValue / diff * diff;

			uint ui;
			do
			{
				ui = GetRandomUInt();
			} while (ui >= upperBound);
			return (int)(minValue + (ui % diff));
		}

		private uint GetRandomUInt()
		{
			var randomBytes = GenerateRandomBytes(sizeof(uint));
			return BitConverter.ToUInt32(randomBytes, 0);
		}

		private byte[] GenerateRandomBytes(int bytesNumber)
		{
			byte[] buffer = new byte[bytesNumber];
			rng.GetBytes(buffer);
			return buffer;
		}
	}
}
