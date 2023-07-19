using B3DLoader.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader;

public enum ChunkTypes
{
	UNKNOWN,
	BB3D,
	TEXS,
	BRUS,
	VRTS,
	TRIS,
	MESH,
	BONE,
	KEYS,
	ANIM,
	NODE
}

public class B3DChunk
{
	public string Name { get; set; }
	public int Length { get; set; }
	public int Position { get; set; }

	public ChunkTypes ChunkType { get; set; }

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

		ChunkType = ChunkTypes.UNKNOWN;

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
				ChunkType = ChunkTypes.BB3D;
				break;
			case "TEXS":
				DataBlock = new B3DTexData( Reader, this );
				Model.Textures = DataBlock as B3DTexData;
				ChunkType = ChunkTypes.TEXS;
				break;
			case "BRUS":
				DataBlock = new B3DBrushData( Reader, this );
				Model.Brushes = DataBlock as B3DBrushData;
				ChunkType = ChunkTypes.BRUS;
				break;
			case "NODE":
				DataBlock = new B3DNodeData( Reader, this );
				if (Model.RootNode == null)
				{
					Model.RootNode = DataBlock as B3DNodeData;
				}

				Model.Nodes.Add( DataBlock as B3DNodeData );
				ChunkType = ChunkTypes.NODE;
				break;
			case "MESH":
				DataBlock = new B3DMeshData( Reader, this );
				Model.Meshes.Add( DataBlock as B3DMeshData );
				ChunkType = ChunkTypes.MESH;
				break;
			case "VRTS":
				DataBlock = new B3DVertData( Reader, this );
				Model.Vertices.Add( DataBlock as B3DVertData );
				ChunkType = ChunkTypes.VRTS;
				break;
			case "TRIS":
				DataBlock = new B3DTriData( Reader, this );
				Model.Triangles.Add( DataBlock as B3DTriData );
				ChunkType = ChunkTypes.TRIS;
				break;
			case "ANIM":
				DataBlock = new B3DAnimData( Reader, this );
				ChunkType = ChunkTypes.ANIM;
				break;
			default:
				Log.Info( $"Unknown chunk type: {Name}" );
				Reader.BaseStream.Seek( Position + Length, SeekOrigin.Begin );
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
