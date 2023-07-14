using B3DLoader.Data;
using System.Collections.Generic;
using System.IO;

namespace B3DLoader;

public class B3DModel
{
	public B3DChunk RootChunk { get; private set; }
	public int B3DVersion { get; private set; }

	/* Blocks */

	public List<B3DMeshData> Meshes { get; private set; }
	public List<B3DBrushData> Brushes { get; private set; }
	public List<B3DTexData> Textures { get; private set; }
	public List<B3DNodeData> Nodes { get; private set; }

	/* Mesh Data */
	public List<B3DVertData> Vertices { get; private set; }
	public List<B3DTriData> Triangles { get; private set; }

	private BinaryReader reader;

	public B3DModel()
	{
		reader = null;

		Meshes = new List<B3DMeshData>();
		Brushes = new List<B3DBrushData>();
		Textures = new List<B3DTexData>();
		Nodes = new List<B3DNodeData>();

		Vertices = new List<B3DVertData>();
		Triangles = new List<B3DTriData>();
	}

	public bool ReadFromReader( BinaryReader br )
	{
		reader = br;
		return readFile();
	}

	public bool ReadFromPath( string path )
	{
		return ReadFromReader( new BinaryReader( File.OpenRead( path ) ) );
	}

	private bool readFile()
	{
		RootChunk = new B3DChunk( reader, this );

		RootChunk.ReadChunk( null );
		if ( RootChunk.Name != "BB3D" )
		{
			Log.Error( "File was not a b3d file." );
			return false;
		}

		B3DVersion = reader.ReadInt32();

		// Process the main chunk
		RootChunk.ProcessChunk();

		return true;
	}
}
