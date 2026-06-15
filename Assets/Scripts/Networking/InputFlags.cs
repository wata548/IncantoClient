using System;
using System.Collections.Generic;
using BVH;
using UnityEngine;

namespace Networking {
	[Flags]
	public enum InputFlags {
		None       = 0b0,
		Forward    = 0b1,
		Backward   = 0b10,
		Left       = 0b100,
		Right      = 0b1000,
		Focus      = 0b10000,
		Jump       = 0b100000,
		Shoot      = 0b1000000,
	}

	public static class ExInputFlags {
		public const float GravityScale = -23.75f;
		public const float JumpScale = 9.5f;
		private static IReadOnlyDictionary<InputFlags, Vector3> _keys = 
			new Dictionary<InputFlags, Vector3> {
				{ InputFlags.Forward, Vector3.forward },
				{ InputFlags.Backward, Vector3.back },
				{ InputFlags.Left, Vector3.left },
				{ InputFlags.Right, Vector3.right },
				{ InputFlags.Jump, Vector3.up * JumpScale },
			};

		public static Vector3 GetVector(this InputFlags pFlag, float pSpeed) {
			var ground = Vector3.zero;
			var y = Vector3.zero;
			foreach (var (k, v) in _keys) {
				if((pFlag & k) != k)
					continue;
				if (v.y == 0)
					ground += v;
				else
					y += v;
			}

			ground = ground.normalized * pSpeed;
			return ground + y ;
		}

	}
}