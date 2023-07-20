using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using B3DLoader.Extensions;
using B3DLoader.Types;
using static B3DLoader.Data.B3DVertData;

namespace B3DLoader.Data;

public class B3DKeysData : B3DBlock
{
	[Flags]
	public enum KeysFlags
	{
		None = 0,
		HasPosition = 1,
		HasScale = 2,
		HasRotation = 4
	}

	public class SubData
	{
		public float Frame { get; set; }
		public Vector3 Position { get; set; }
		public Vector3 Scale { get; set; }
		public Quaternion Rotation { get; set; }
	}

	public KeysFlags Flags { get; set; }

	public List<SubData> Keys { get; set; }

	public B3DKeysData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
		Keys = new List<SubData>();
	}

	public override void ReadBlock()
	{
		Flags = (KeysFlags)Reader.ReadInt32();

		while ( Chunk.TillNextChunk() )
		{
			var sub = new SubData();

			sub.Frame = Reader.ReadSingle();

			if ( Flags.HasFlag( KeysFlags.HasPosition ) )
			{
				// These vectors are X Z Y rather than X Y Z
				float posX = Reader.ReadSingle();
				float posY = Reader.ReadSingle();
				float posZ = Reader.ReadSingle();

				sub.Position = new Vector3( posX, posY, posZ ).Flip();
			}

			if ( Flags.HasFlag( KeysFlags.HasScale ) )
			{
				float scaleX = Reader.ReadSingle();
				float scaleY = Reader.ReadSingle();
				float scaleZ = Reader.ReadSingle();

				sub.Scale = new Vector3( scaleX, scaleY, scaleZ ).Flip();
			}

			if ( Flags.HasFlag( KeysFlags.HasRotation ) )
			{
				// For some reason their quaternion is inverted, also YZ is flipped
				float rotW = Reader.ReadSingle();
				float rotZ = Reader.ReadSingle();
				float rotY = Reader.ReadSingle();
				float rotX = Reader.ReadSingle();

				sub.Rotation = new Quaternion( rotX, rotY, rotZ, rotW ).Flip();
			}

			Keys.Add( sub );
		}
	}
}
