using System.Collections.Generic;
using System.IO;

namespace B3DLoader
{
	internal static class BinaryReaderExtensions
	{
		public static string ReadNullTerminatedString( this BinaryReader br )
		{
			List<char> rawString = new List<char>();

			char curChar;
			while ( (curChar = br.ReadChar()) != '\0' )
			{
				rawString.Add( curChar );
			}

			return new string( rawString.ToArray() );
		}
	}
}
