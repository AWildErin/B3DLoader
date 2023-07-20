using B3DLoader.Extensions;
using B3DLoader.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Data;

public class B3DBrushData : B3DBlock
{
	public class SubData
	{
		public string Name { get; set; }

		// RGBA color as 0-1
		public Color Color { get; set; }

		public float Shininess { get; set; }
		public int Blend { get; set; }
		public int Fx { get; set; }
		public int[] TextureIds { get; set; }
	}

	public List<SubData> BrushData { get; set; }
	public int TextureCount { get; set; }

	public B3DBrushData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		BrushData = new List<SubData>();
	}

	public override void ReadBlock()
	{
		// Read the number of textures for this brush
		TextureCount = Reader.ReadInt32();

		while ( Chunk.TillNextChunk() )
		{
			var sub = new SubData();
			sub.Name = Reader.ReadNullTerminatedString();

			float r = Reader.ReadSingle();
			float g = Reader.ReadSingle();
			float b = Reader.ReadSingle();
			float a = Reader.ReadSingle();
			sub.Color = new Color( r, g, b, a );

			sub.Shininess = Reader.ReadSingle();
			sub.Blend = Reader.ReadInt32();
			sub.Fx = Reader.ReadInt32();

			sub.TextureIds = new int[TextureCount];
			for ( int i = 0; i < TextureCount; i++ )
			{
				sub.TextureIds[i] = Reader.ReadInt32();
			}

			Log.Info( $"\tFound Brush: {sub.Name}" );

			BrushData.Add( sub );
		}
	}
}
