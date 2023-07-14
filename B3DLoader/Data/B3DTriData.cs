using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Data;

public class B3DTriData : B3DBlock
{
	public int BrushID { get; set; }
	public List<int> Indices { get; set; }

	public B3DTriData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Indices = new List<int>();
	}

	public override void ReadBlock()
	{
		BrushID = Reader.ReadInt32();

		while ( Chunk.NextChunk() )
		{
			Indices.Add( Reader.ReadInt32() );
			Indices.Add( Reader.ReadInt32() );
			Indices.Add( Reader.ReadInt32() );

			// Flip the indices
			// TODO: can we do better?
			int tmp = Indices[Indices.Count - 1];
			Indices[Indices.Count - 1] = Indices[Indices.Count - 2];
			Indices[Indices.Count - 2] = tmp;
		}

		Log.Info( $"\tFound Tri: {BrushID}" );
	}
}
