using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Extensions;

public static class NumericExtensions
{
	public static float ToRadians( this float v )
	{
		return (float)(v * (Math.PI / 180f));
	}

	public static float ToDegrees( this float v )
	{
		return (float)(v * (180f / Math.PI));
	}
}
