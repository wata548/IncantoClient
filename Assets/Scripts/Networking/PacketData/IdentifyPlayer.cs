using System.Collections.Generic;

namespace Networking {
	public class IdentifyPlayer: PacketData {
		public readonly MatchPlayers Match;

		public IdentifyPlayer(byte[] pBytes, ref int pIdx) : base(pBytes, ref pIdx) {
			Match = new(pBytes, ref pIdx);
		}
		
		public override IEnumerable<byte> GetBytes() {
			var result = new List<byte>();
			result.AddRange(base.GetBytes());
			result.AddRange(Match.GetBytes());
			return result;
		}
	}
}