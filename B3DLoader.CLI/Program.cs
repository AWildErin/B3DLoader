namespace B3DLoader.CLI;

internal class Program
{
	private static void Main( string[] args )
	{
		string filePath = args[0];

		var mdl = new B3DModel();
		var result = mdl.ReadFromPath( filePath );

		if ( !result )
		{
			Log.Error( "Failed to read B3D File!" );
			return;
		}
		else
		{
			Log.Info( "Model Loaded" );
		}

		// Write some debug info after the fact
		logChildren( mdl.RootChunk );
	}

	private static void logChildren( B3DChunk chunk )
	{
		Console.WriteLine( chunk.Name );

		foreach ( var child in chunk.Children )
		{
			logChildren( child );
		}
	}
}
