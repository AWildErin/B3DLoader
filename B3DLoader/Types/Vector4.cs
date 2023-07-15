using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Vector4
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
	public float W { get; set; }

	public Vector4( float x, float y, float z, float w )
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public static implicit operator System.Numerics.Vector4( Vector4 i )
	{
		return new System.Numerics.Vector4( i.X, i.Y, i.Z, i.W );
	}
}
