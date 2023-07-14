using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Triangle
{
	public float Point1 { get; set; }
	public float Point2 { get; set; }
	public float Point3 { get; set; }

	public Triangle( float p1, float p2, float p3 )
	{
		Point1 = p1;
		Point2 = p2;
		Point3 = p3;
	}
}
