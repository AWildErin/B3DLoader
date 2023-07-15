using B3DLoader.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3DLoader.Extensions;

public static class FlipExtensions
{
	/// <summary>
	/// Flips the Y and Z of the given vector
	/// </summary>
	/// <param name="i">Vector we want to flip</param>
	/// <returns>The flipped vector</returns>
	public static Vector3 Flip( this Vector3 i )
	{
		float y = i.Y;
		float z = i.Z;

		i.Y = z;
		i.Z = y;

		return i;
	}

	/// <summary>
	/// Flips the Y and Z of the given quaternion
	/// </summary>
	/// <param name="i">Quaternion we want to flip</param>
	/// <returns>The flipped quaternion</returns>
	public static Quaternion Flip( this Quaternion i )
	{
		float y = i.Y;
		float z = i.Z;

		i.Y = z;
		i.Z = y;

		return i;
	}
}
