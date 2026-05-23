using System.Collections.Generic;

namespace Networking {
	public class IdentifyPlayer: PacketData {
		public readonly MatchPlayers Players;

		public IdentifyPlayer(byte[] pBytes, ref int pIdx) : base(pBytes, ref pIdx) {
			Players = new(pBytes, ref pIdx);
		}
		
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(Players.GetBytes());
			return result;
		}
	}
}