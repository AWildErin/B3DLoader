using B3DLoader.Extensions;
using B3DLoader.Types;
using System.Collections.Generic;
using System.IO;

namespace B3DLoader.Data;

public class B3DTexData : B3DBlock
{
	public class SubData
	{
		public string Name { get; set; }
		public int Flags { get; set; }
		public int Blend { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 Scale { get; set; }
		public float Rotation { get; set; }
	}

	public List<SubData> TexData { get; set; }

	public B3DTexData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		TexData = new List<SubData>();
	}

	public override void ReadBlock()
	{
		while ( Chunk.TillNextChunk() )
		{
			var sub = new SubData();
			sub.Name = Reader.ReadNullTerminatedString();
			sub.Flags = Reader.ReadInt32();
			sub.Blend = Reader.ReadInt32();

			float posX = Reader.ReadSingle();
			float posY = Reader.ReadSingle();

			sub.Position = new Vector2( posX, posY );

			float scaleX = Reader.ReadSingle();
			float scaleY = -Reader.ReadSingle();
			sub.Scale = new Vector2( scaleX, scaleY );

			sub.Rotation = Reader.ReadSingle();

			Log.Info( $"\tFound texture: {sub.Name}" );

			TexData.Add( sub );
		}
	}
}
