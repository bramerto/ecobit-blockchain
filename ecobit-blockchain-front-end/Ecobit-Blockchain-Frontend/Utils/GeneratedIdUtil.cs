using System;

namespace Ecobit_Blockchain_Frontend.Utils
{
	public static class GenerateIdUtil
	{
		/// <summary>
		/// Generates a unique id
		/// </summary>
		/// <returns>a Transaction</returns>
		public static string GenerateUniqueId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}