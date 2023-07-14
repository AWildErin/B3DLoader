using B3DLoader.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Data;

public class B3DVertData : B3DBlock
{
	[Flags]
	public enum VertFlags
	{
		None = 0,
		HasNormals = 1,
		HasColor = 2
	}

	public class SubData
	{
		public Vector3 Position { get; set; }
		public Vector3 Normal { get; set; }
		public Color Color { get; set; }
		public float[][] TexCoords { get; set; }
	}

	public VertFlags Flags { get; set; }
	public int TexCoordSets { get; set; }
	public int TexCoordSetSize { get; set; }

	public List<SubData> Verts { get; set; }

	public B3DVertData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Verts = new List<SubData>();
	}

	public override void ReadBlock()
	{
		Flags = (VertFlags)Reader.ReadInt32();
		TexCoordSets = Reader.ReadInt32();
		TexCoordSetSize = Reader.ReadInt32();

		while ( Chunk.NextChunk() )
		{
			var sub = new SubData();

			float x = Reader.ReadSingle();
			float y = Reader.ReadSingle();
			float z = Reader.ReadSingle();

			sub.Position = new Vector3( x, y, z ).Flip();

			if ( Flags.HasFlag( VertFlags.HasNormals ) )
			{
				float normX = Reader.ReadSingle();
				float normY = Reader.ReadSingle();
				float normZ = Reader.ReadSingle();

				sub.Normal = new Vector3( normX, normY, normZ ).Flip();
			}
			else
			{
				sub.Normal = Vector3.Zero;
			}

			if ( Flags.HasFlag( VertFlags.HasColor ) )
			{
				float r = Reader.ReadSingle();
				float g = Reader.ReadSingle();
				float b = Reader.ReadSingle();
				float a = Reader.ReadSingle();

				sub.Color = new Color( r, g, b, a );
			}
			else
			{
				sub.Color = Color.White;
			}

			sub.TexCoords = new float[TexCoordSets][];
			for ( int i = 0; i < TexCoordSets; i++ )
			{
				sub.TexCoords[i] = new float[TexCoordSetSize];
				for ( int j = 0; j < TexCoordSetSize; j++ )
				{
					sub.TexCoords[i][j] = Reader.ReadSingle();
				}
			}

			// Default our tex coords
			if ( TexCoordSets == 0 || TexCoordSetSize == 0 )
			{
				sub.TexCoords = new float[1][];
				sub.TexCoords[0] = new float[2];
				sub.TexCoords[0][0] = 0;
				sub.TexCoords[0][1] = 0;
			}

			Verts.Add( sub );
		}

		Log.Info( $"\t\tFound Verts" );
	}
}
