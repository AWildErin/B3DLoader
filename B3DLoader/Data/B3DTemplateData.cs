using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Data;

public class B3DTemplateData : B3DBlock
{
	public class SubData
	{
		public string Name { get; set; }
	}

	public List<SubData> ChildData { get; set; }

	public B3DTemplateData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		ChildData = new List<SubData>();
	}

	public override void ReadBlock()
	{
		while ( Chunk.NextChunk() )
		{
			var sub = new SubData();
			sub.Name = Reader.ReadNullTerminatedString();


			Log.Info( $"\tFound Child: {sub.Name}" );

			ChildData.Add( sub );
		}
	}
}
