using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Data;

public class B3DMeshData : B3DBlock
{
	public int BrushId { get; set; }

	public B3DVertData Vertices { get; set; }
	public List<B3DTriData> Triangles { get; set; }

	public B3DMeshData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Triangles = new List<B3DTriData>();
	}

	public override void ReadBlock()
	{
		while ( Chunk.TillNextChunk() )
		{
			BrushId = Reader.ReadInt32();

			// Read verts and tris
			while ( Chunk.TillNextChunk() )
			{
				var chunk = new B3DChunk( Reader, Chunk.Model );
				chunk.ReadChunk( Chunk );
				chunk.ProcessChunk();

				if ( chunk.Name == "VRTS" )
				{
					Vertices = chunk.DataBlock as B3DVertData;

				}
				else if ( chunk.Name == "TRIS" )
				{
					Triangles.Add( chunk.DataBlock as B3DTriData );
				}
			}

			Log.Info( $"\tFound Mesh: {BrushId}" );
		}
	}
}
