using System;
using UnityEngine;

namespace Extensions {
	public static class ExList {
		public static string ToOptimizedString(this bool[] pBuffer) {
			var length = (int)Math.Ceiling(pBuffer.Length / 8d);
			var result = new byte[length];
			for (int i = 0; i < pBuffer.Length; i++) {
				result[i / 8] = (byte)((result[i / 8] << 1) + (pBuffer[i] ? 1 : 0));
			}
			return Convert.ToBase64String(result);
		}
	}
}