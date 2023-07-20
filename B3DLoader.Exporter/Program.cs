using B3DLoader.Data;
using B3DLoader.Extensions;
using SharpGLTF.Scenes;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace B3DLoader.Exporter;

internal class Program
{
	private static void Main( string[] args )
	{
		string filePath = args[0];

		var mdl = new B3DModel( true );
		var result = mdl.ReadFromPath( filePath );

		// Enable Logging after we've read the model
		Log.EnableDebug = true;

		if ( !result )
		{
			Log.Error( "Failed to read B3D File!" );
			return;
		}

		var outputPath = Path.ChangeExtension( filePath, "gltf" );

		var export = new B3DExport( mdl );
		export.ExportModel( outputPath );
	}
}
