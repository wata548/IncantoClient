using System;
using System.Collections.Generic;
using System.Linq;
using BVH;

namespace Networking {
	public class MoveData: PacketData {
		public const int MouseMaxUpdateTerm = 4;
		public InputFlags Input { get; set; }
		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public Vector[] MouseDelta { get; set; } = new Vector[MouseMaxUpdateTerm];

		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(BitConverter.GetBytes((int)Input));
			result.AddRange(BitConverter.GetBytes(Yaw));
			result.AddRange(BitConverter.GetBytes(Pitch));
			result.AddRange(MouseDelta.SelectMany(v => v.GetBytes()));
			return result;
		}

		public MoveData(){}
		public MoveData(byte[] pBytes, ref int pStart)
			:base(pBytes, ref pStart){
			Input = (InputFlags)GetInt(pBytes, ref pStart);
			Yaw = GetSingle(pBytes, ref pStart);
			Pitch = GetSingle(pBytes, ref pStart);
			MouseDelta = new Vector[MouseMaxUpdateTerm];
			
			for (int i = 0; i < MouseMaxUpdateTerm; i++)
				MouseDelta[i] = new Vector(pBytes, ref pStart);
		}
	} 
}