using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Quaternion
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }
	public float W { get; set; }

	public Quaternion( float x, float y, float z, float w )
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}

	public static implicit operator System.Numerics.Quaternion( Quaternion i )
	{
		return new System.Numerics.Quaternion( i.X, i.Y, i.Z, i.W );
	}

	public static implicit operator System.Numerics.Vector4( Quaternion i )
	{
		return new System.Numerics.Vector4( i.X, i.Y, i.Z, i.W );
	}
}
