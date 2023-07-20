﻿using B3DLoader.Types;
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
	public int BrushId { get; set; }
	public List<Triangle> Triangles { get; set; }

	public B3DTriData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Triangles = new List<Triangle>();
	}

	public override void ReadBlock()
	{
		BrushId = Reader.ReadInt32();

		while ( Chunk.TillNextChunk() )
		{
			int p1 = Reader.ReadInt32();
			int p2 = Reader.ReadInt32();
			int p3 = Reader.ReadInt32();

			// Flip the indices
			Triangles.Add( new Triangle( p1, p3, p2 ) );
		}

		Log.Info( $"\tFound Tri: {BrushId}" );
	}
}
