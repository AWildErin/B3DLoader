using B3DLoader.Extensions;
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

		public Dictionary<int, Vector2> TexCoords { get; set; }

		/// <summary>
		/// List of advanced tex coords (4 components)
		/// </summary>
		/// <see cref="TexCoordSetSize"/>
		public Dictionary<int, Vector4> AdvancedTexCoords { get; set; }
	}

	public VertFlags Flags { get; set; }

	/// <summary>
	/// Texture coords per vertex (eg: 1 for simple U/V) max=8
	/// </summary>
	public int TexCoordSets { get; set; }

	/// <summary>
	/// Components per set (eg: 2 for simple U/V) max=4
	/// </summary>
	public int TexCoordSetSize { get; set; }

	/// <summary>
	/// Specifies whether or not we have more than 2 components per TexCoordSet.
	/// </summary>
	public bool HasAdvancedTexCoords { get; set; }

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

		if ( TexCoordSetSize > 2 )
		{
			HasAdvancedTexCoords = true;
		}

		while ( Chunk.TillNextChunk() )
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

			sub.AdvancedTexCoords = new Dictionary<int, Vector4>();
			sub.TexCoords = new Dictionary<int, Vector2>();

			for ( int i = 0; i < TexCoordSets; i++ )
			{
				if ( HasAdvancedTexCoords )
				{
					float p1 = Reader.ReadSingle();
					float p2 = Reader.ReadSingle();
					float p3 = Reader.ReadSingle();
					float p4 = Reader.ReadSingle();

					sub.AdvancedTexCoords.Add( i, new Vector4( p1, p2, p3, p4 ) );
				}
				else
				{
					float p1 = Reader.ReadSingle();
					float p2 = Reader.ReadSingle();

					sub.TexCoords.Add( i, new Vector2( p1, p2 ) );
				}
			}

			Verts.Add( sub );
		}

		Log.Info( $"\t\tFound Verts" );
	}
}
