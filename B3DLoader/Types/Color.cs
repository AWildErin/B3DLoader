using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Color
{
	public float R { get; set; }
	public float G { get; set; }
	public float B { get; set; }
	public float A { get; set; }

	public Color( float r, float g, float b, float a )
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}

	public static readonly Color Black = new Color( 0, 0, 0, 1 );
	public static readonly Color White = new Color( 1, 1, 1, 1 );

	public override string ToString()
	{
		return $"R = {R}, G = {G}, B = {B}, A = {A}";
	}
}
