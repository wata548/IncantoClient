using System;
using System.Collections.Generic;

namespace Networking {
	public class PacketData: ConvertBytes {
		public int Id { get; set; }
		public PacketCommand Command { get; set; }
		public PacketData(){}

		public static PacketData Generate(byte[] pBytes) {
			var command = (PacketCommand)BitConverter.ToInt32(pBytes);
			var idx = 0;
			return command switch {
				PacketCommand.Move => new MoveData(pBytes, ref idx),
				PacketCommand.Rebirth => new Data(pBytes, ref idx)
			};
		}
		
		protected PacketData(byte[] pBytes, ref int pStart)
			: base(pBytes, ref pStart) {
			Command = (PacketCommand)GetInt(pBytes, ref pStart);
			Id = GetInt(pBytes, ref pStart);
		}
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(BitConverter.GetBytes((int)Command));
			result.AddRange(BitConverter.GetBytes(Id));
			return result;
		}
	}
	
	public abstract class ConvertBytes {
		//==================================================Constructors	
		protected ConvertBytes(byte[] pBytes, ref int pStart){}
		protected ConvertBytes(){}
		
		//==================================================Methods	
		public abstract IEnumerable<byte> GetBytes();

		protected int GetInt(byte[] pBytes, ref int pStart) {
			var temp = BitConverter.ToInt32(pBytes, pStart);
			pStart += 4;
			return temp;
		}

		protected float GetSingle(byte[] pBytes, ref int pStart) {
			var temp = BitConverter.ToSingle(pBytes, pStart);
			pStart += 4;
			return temp;
		}
		protected bool GetBoolean(byte[] pBytes, ref int pStart) {
			var temp = BitConverter.ToBoolean(pBytes, pStart);
			pStart++;
			return temp;
		}
	}
}