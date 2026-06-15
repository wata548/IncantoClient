using System;
using System.Collections.Generic;
using System.Linq;

namespace Networking {
    public class ResultData: PacketData {
        private int[] _rank;
        
        public ResultData(IEnumerable<int> pRank) {
            Id = -1;
            Command = PacketCommand.SendResult;
            _rank = pRank.ToArray();
            if (_rank.Length != MatchPlayers.MatchPerPlayer)
                throw new ArgumentOutOfRangeException($"Rank data's length must be {MatchPlayers.MatchPerPlayer}. but {_rank}.");
        }

        public ResultData(byte[] pBytes, ref int pIdx) : base(pBytes, ref pIdx) {
            _rank = new int[MatchPlayers.MatchPerPlayer];
            for (int i = 0; i < MatchPlayers.MatchPerPlayer; i ++) {
                _rank[i] = GetInt(pBytes, ref pIdx);
            }
        }

        public int GetRank(int pIdx) {
            for (int i = 0; i < _rank.Length; i++) {
                if(pIdx != _rank[i]) continue;
                return i + 1;
            }
            return -1;
        }
        public override IEnumerable<byte> GetBytes() {
            var result = new List<byte>();
            result.AddRange(base.GetBytes());
            foreach (var p in _rank) {
                result.AddRange(BitConverter.GetBytes(p));
            }
            return result;
        }
    }
}
