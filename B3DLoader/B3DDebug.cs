using System;
using System.Diagnostics;

namespace B3DLoader;

public class Log
{
	public static bool EnableDebug = false;

	public static void Info( object obj )
	{
		if ( !EnableDebug )
		{
			return;
		}

		Console.WriteLine( obj );
	}

	public static void Warning( object obj )
	{
		if ( !EnableDebug )
		{
			return;
		}

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine( obj );
		Console.ResetColor();
	}

	public static void Error( object obj )
	{
		if ( !EnableDebug )
		{
			return;
		}

		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine( obj );
		Console.ResetColor();

		Debugger.Break();
	}
}
