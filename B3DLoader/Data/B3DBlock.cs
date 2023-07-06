using System.IO;

namespace B3DLoader.Data
{
	public abstract class B3DBlock
	{
		public string Name { get; set; }
		public B3DChunk Chunk { get; set; }
		public BinaryReader Reader { get; set; }

		public B3DBlock( BinaryReader reader, B3DChunk chunk )
		{
			Chunk = chunk;
			Reader = reader;
		}

		public abstract void ReadBlock();
	}
}
