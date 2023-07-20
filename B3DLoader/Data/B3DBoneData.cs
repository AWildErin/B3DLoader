using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B3DLoader.Extensions;

namespace B3DLoader.Data;

public class B3DBoneData : B3DBlock
{
	public class SubData
	{
		public int VertexId { get; set; }
		public float Weight { get; set; }
	}

	public List<SubData> Weights { get; set; }

	public B3DBoneData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Weights = new List<SubData>();
	}

	public override void ReadBlock()
	{
		while ( Chunk.NextChunk() )
		{
			var sub = new SubData();

			sub.VertexId = Reader.ReadInt32();
			sub.Weight = Reader.ReadSingle();

			Weights.Add( sub );
		}
	}
}
