using System;
using System.Collections.Generic;
using Networking;

namespace BVH {
	public class Vector : ConvertBytes {
		public static readonly Vector One = new(1, 1, 1);
		public float X;
		public float Y;
		public float Z;

		public Vector(byte[] pBytes, ref int pStart): base(pBytes, ref pStart) {
			X = BitConverter.ToSingle(pBytes, pStart);
			Y = BitConverter.ToSingle(pBytes, pStart += 4);
			Z = BitConverter.ToSingle(pBytes, pStart += 4);
			pStart += 4;
		}

		public float SqrMagnitude() => X * X + Y * Y + Z * Z;

		public Vector(float pX = 0, float pY = 0, float pZ = 0) =>
			(X, Y, Z) = (pX, pY, pZ);

		public override string ToString() =>
			$"{X}, {Y}, {Z}";

		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(BitConverter.GetBytes(X));
			result.AddRange(BitConverter.GetBytes(Y));
			result.AddRange(BitConverter.GetBytes(Z));
			return result;
		}

		public ConvertBytes Generate(byte[] pBytes, ref int pStart) {
			var temp = new Vector(pBytes, ref pStart);
			pStart += 12;
			return temp;
		}

#region Operators

		public static Vector operator +(Vector lhs, Vector rhs) => new(
			lhs.X + rhs.X,
			lhs.Y + rhs.Y,
			lhs.Z + rhs.Z
		);

		public static Vector operator +(Vector lhs, float rhs) => new(
			lhs.X + rhs,
			lhs.Y + rhs,
			lhs.Z + rhs
		);

		public static Vector operator -(Vector lhs, Vector rhs) => new(
			lhs.X - rhs.X,
			lhs.Y - rhs.Y,
			lhs.Z - rhs.Z
		);

		public static Vector operator -(Vector lhs, float rhs) => new(
			lhs.X - rhs,
			lhs.Y - rhs,
			lhs.Z - rhs
		);

		public static Vector operator *(Vector lhs, float rhs) => new(
			lhs.X * rhs,
			lhs.Y * rhs,
			lhs.Z * rhs
		);

		public static Vector operator /(Vector lhs, float rhs) => new(
			lhs.X / rhs,
			lhs.Y / rhs,
			lhs.Z / rhs
		);

		public float Dot(Vector rhs) =>
			X * rhs.X + Y * rhs.Y + Z * rhs.Z;

		public Vector Cross(Vector rhs) =>
			new(Y * rhs.Z - Z * rhs.Y,
				X * rhs.Z - Z * rhs.X,
				X * rhs.Y - Y * rhs.X);

#endregion
	}
}