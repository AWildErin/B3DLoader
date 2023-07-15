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

public class B3DNodeData : B3DBlock
{
	public class SubData
	{
		public string Name { get; set; }

		public Vector3 Position { get; set; }
		public Vector3 Scale { get; set; }

		// Rotation as quaternion
		public Quaternion Rotation { get; set; }
	}

	public SubData Data { get; set; }

	public B3DNodeData( BinaryReader Reader, B3DChunk chunk ) : base( Reader, chunk )
	{
	}

	public override void ReadBlock()
	{
		while ( Chunk.NextChunk() )
		{
			var sub = new SubData();
			sub.Name = Reader.ReadNullTerminatedString();
			Name = sub.Name;

			// These vectors are X Z Y rather than X Y Z
			float posX = Reader.ReadSingle();
			float posY = Reader.ReadSingle();
			float posZ = Reader.ReadSingle();

			sub.Position = new Vector3( posX, posY, posZ ).Flip();

			float scaleX = Reader.ReadSingle();
			float scaleY = Reader.ReadSingle();
			float scaleZ = Reader.ReadSingle();

			sub.Scale = new Vector3( scaleX, scaleY, scaleZ ).Flip();

			// For some reason their quaternion is inverted, also YZ is flipped
			float rotW = Reader.ReadSingle();
			float rotZ = Reader.ReadSingle();
			float rotY = Reader.ReadSingle();
			float rotX = Reader.ReadSingle();

			sub.Rotation = new Quaternion( rotX, rotY, rotZ, rotW ).Flip();

			Log.Info( $"\tFound Node: {sub.Name}" );

			// Read child chunks
			while ( Chunk.NextChunk() )
			{
				var subchunk = new B3DChunk( Reader, Chunk.Model );
				subchunk.ReadChunk( Chunk );
				subchunk.ProcessChunk();
			}

			Data = sub;
		}
	}
}
