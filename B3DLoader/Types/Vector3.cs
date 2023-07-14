using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Vector3
{
	public float X { get; set; }
	public float Y { get; set; }
	public float Z { get; set; }

	public Vector3( float x, float y, float z )
	{
		X = x;
		Y = y;
		Z = z;
	}

	public static readonly Vector3 Zero = new Vector3( 0, 0, 0 );
	public static readonly Vector3 One = new Vector3( 1, 1, 1 );
}
