using System;
using System.Collections.Generic;
using BVH;

namespace Networking {
	[Flags]
	public enum InputFlags {
		Forward    = 0b1,
		Backward   = 0b10,
		Left       = 0b100,
		Right      = 0b1000,
		Focus      = 0b10000,
		Jump       = 0b100000,
	}

	public class Data : PacketData {
		public float T { get; set; }
		public bool Check { get; set; }

		public Data(byte[] pData, ref int pIdx)
			: base(pData, ref pIdx) {
			T = GetSingle(pData, ref pIdx);
			Check = GetBoolean(pData, ref pIdx);
		}
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(BitConverter.GetBytes(T));
			result.AddRange(BitConverter.GetBytes(Check));
			return result;
		}
	} 
	
	public class MoveData: PacketData {
		public InputFlags Input { get; set; }
		public Vector Pos { get; set; }
		public Vector Rotation { get; set; }
		public Vector Velocity { get; set; }
		public Vector MouseDelta { get; set; }
		public bool IsPainting { get; set; }

		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(BitConverter.GetBytes((int)Input));
			result.AddRange(Pos.GetBytes());
			result.AddRange(Rotation.GetBytes());
			result.AddRange(Velocity.GetBytes());
			result.AddRange(MouseDelta.GetBytes());
			result.AddRange(BitConverter.GetBytes(IsPainting));
			return result;
		}

		public MoveData(){}
		public MoveData(byte[] pBytes, ref int pStart)
			:base(pBytes, ref pStart){
			Input = (InputFlags)GetInt(pBytes, ref pStart);
			Pos = new Vector(pBytes, ref pStart);
			Rotation = new Vector(pBytes, ref pStart);
			Velocity = new Vector(pBytes, ref pStart);
			MouseDelta = new Vector(pBytes, ref pStart);
			IsPainting = GetBoolean(pBytes, ref pStart);
		}
	} 
}