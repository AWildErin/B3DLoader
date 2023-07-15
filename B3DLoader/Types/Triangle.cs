using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Types;

public class Triangle
{
	public int Point1 { get; set; }
	public int Point2 { get; set; }
	public int Point3 { get; set; }

	public Triangle( int p1, int p2, int p3 )
	{
		Point1 = p1;
		Point2 = p2;
		Point3 = p3;
	}
}
