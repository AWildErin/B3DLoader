using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Vector2
{
	public float X { get; set; }
	public float Y { get; set; }

	public Vector2( float x, float y )
	{
		X = x;
		Y = y;
	}

	public static implicit operator System.Numerics.Vector2( Vector2 i )
	{
		return new System.Numerics.Vector2( i.X, i.Y );
	}
}
