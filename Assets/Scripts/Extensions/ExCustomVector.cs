using BVH;
using UnityEngine;

namespace Extensions {
	public static class ExCustomVector {
		public static Vector3 ToUnityVector(this Vector pVector) =>
			new(pVector.X, pVector.Y, pVector.Z);
		public static Vector ToCustomVector(this Vector3 pVector) =>
			new(pVector.x, pVector.y, pVector.z);
	}
}