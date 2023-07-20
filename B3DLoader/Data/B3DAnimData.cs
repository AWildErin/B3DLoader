using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B3DLoader.Extensions;

namespace B3DLoader.Data;

public class B3DAnimData : B3DBlock
{
	public int Flags { get; set; }
	public int Frames { get; set; }
	public float FPS { get; set; }

	public B3DAnimData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{

	}

	public override void ReadBlock()
	{
		while ( Chunk.TillNextChunk() )
		{
			Flags = Reader.ReadInt32();
			Frames = Reader.ReadInt32();
			FPS = Reader.ReadSingle();

			Log.Info( $"\tFound Anim" );
		}

		// We cheat here and process the next block as it'll always be a SEQS block, 
	}
}
