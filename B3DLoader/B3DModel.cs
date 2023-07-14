using B3DLoader.Data;
using System.Collections.Generic;
using System.IO;

namespace B3DLoader;

public class B3DChunk
{
	public string Name { get; set; }
	public int Length { get; set; }
	public int Position { get; set; }

	public B3DChunk Parent { get; set; }
	public List<B3DChunk> Children { get; set; }

	public B3DBlock DataBlock { get; set; }
	public BinaryReader Reader { get; set; }

	public B3DModel Model { get; set; }

	private bool isChunkRead;

	public B3DChunk( BinaryReader br, B3DModel model )
	{
		Children = new List<B3DChunk>();

		Model = model;
		Reader = br;

		isChunkRead = false;
	}

	public bool NextChunk()
	{
		return Reader.BaseStream.Position < Length + Position;
	}

	public void ReadChunk( B3DChunk parent )
	{
		Name = new string( Reader.ReadChars( 4 ) );
		Parent = parent;

		if ( parent != null )
		{
			Parent.Children.Add( this );
		}

		Position = (int)Reader.BaseStream.Position - 4;
		Length = Reader.ReadInt32() + 8;
		isChunkRead = true;
	}

	public void ProcessChunk()
	{
		// Read the chunk if it hasn't been already
		if ( !isChunkRead )
		{
			// If we haven't been read before this, we likely dont have a parent anyway
			ReadChunk( null );
		}

		Log.Info( $"Processing: {Name}" );

		// Read our data first

		// TODO: probably make this a list of types or something so it's easy to add new ones
		switch ( Name )
		{
			case "BB3D":
				break;
			case "TEXS":
				DataBlock = new B3DTexData( Reader, this );
				Model.Textures.Add( DataBlock as B3DTexData );
				break;
			case "BRUS":
				DataBlock = new B3DBrushData( Reader, this );
				Model.Brushes.Add( DataBlock as B3DBrushData );
				break;
			case "NODE":
				DataBlock = new B3DNodeData( Reader, this );
				Model.Nodes.Add( DataBlock as B3DNodeData );
				break;
			case "MESH":
				DataBlock = new B3DMeshData( Reader, this );
				Model.Meshes.Add( DataBlock as B3DMeshData );
				break;
			case "VRTS":
				DataBlock = new B3DVertData( Reader, this );
				Model.Vertices.Add( DataBlock as B3DVertData );
				break;
			case "TRIS":
				DataBlock = new B3DTriData( Reader, this );
				Model.Triangles.Add( DataBlock as B3DTriData );
				break;
			default:
				Reader.BaseStream.Seek( Length, SeekOrigin.Begin );
				break;
		}

		if ( DataBlock != null )
		{
			DataBlock.ReadBlock();
		}

		// Then read our children
		while ( NextChunk() )
		{
			var child = new B3DChunk( Reader, Model );
			child.ReadChunk( this );
			child.ProcessChunk();
		}

		//Debugger.Break();
	}
}

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
