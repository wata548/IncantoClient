using System;
using System.Collections.Generic;

namespace Networking {
	public class QuestionData: PacketData {
		private int _length;
		public string Context { get; private set; }

		public QuestionData(byte[] pBytes, ref int pStart) : base(pBytes, ref pStart) {
			_length = GetInt(pBytes, ref pStart);
			Context = GetString(pBytes, ref pStart, _length);
		}
		
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(BitConverter.GetBytes(_length));
			foreach (var c in Context) {
				result.Add((byte)c);
			}
			return result;
		}
	}
}