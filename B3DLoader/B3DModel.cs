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

	public B3DBrushData Brushes { get; set; }
	public B3DTexData Textures { get; set; }

	public B3DNodeData RootNode { get; set; }
	public List<B3DNodeData> Nodes { get; private set; }

	/* Mesh Data */
	public List<B3DVertData> Vertices { get; private set; }
	public List<B3DTriData> Triangles { get; private set; }

	private BinaryReader reader;

	/* Unknown Chunks */
	public List<UnknownChunk> UnknownChunks { get; private set; }

	/// <summary>
	/// Sets whether or not to report unknown chunks and list them once the model is fully loaded.
	/// </summary>
	public bool ReportUnknownChunks { get; private set; }

	public struct UnknownChunk
	{
		public string name;
		public int offset;
	}


	public B3DModel( bool reportUnkChunks = false )
	{
		reader = null;

		Meshes = new List<B3DMeshData>();
		Nodes = new List<B3DNodeData>();

		Vertices = new List<B3DVertData>();
		Triangles = new List<B3DTriData>();

		ReportUnknownChunks = reportUnkChunks;
		UnknownChunks = new List<UnknownChunk>();
	}

	public bool ReadFromReader( BinaryReader br )
	{
		reader = br;
		return readFile();
	}

	public bool ReadFromPath( string path )
	{
		if ( !File.Exists( path ) )
		{
			Log.Error( $"{path} does not exist!" );
			return false;
		}

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

		// Report all unknown chunks and their offsets
		if ( ReportUnknownChunks )
		{
			foreach ( var chunk in UnknownChunks )
			{
				Log.Info( $"Encountered unknown chunk \"{chunk.name}\" at offset: {chunk.offset}" );
			}
		}

		return true;
	}
}
