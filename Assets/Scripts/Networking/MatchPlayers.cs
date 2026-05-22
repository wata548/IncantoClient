using System;
using System.Collections.Generic;
using Networking;

public class MatchPlayers: ConvertBytes {

	//==================================================||Properties	
	
	public const int MatchPerPlayer = 4;
	public MatchPlayers(){}
	
	//==================================================||Methods	
	public MatchPlayers(byte[] pBytes, ref int pStart) {
		var result = new int[4];
		result[0] = GetInt(pBytes, ref pStart);
		result[1] = GetInt(pBytes, ref pStart);
		result[2] = GetInt(pBytes, ref pStart);
		result[3] = GetInt(pBytes, ref pStart);
		Players = result;
	}
	
	public IReadOnlyCollection<int> Players { get; private set; }

	//==================================================||Methods	
	public override string ToString() {
		return string.Join(", ", Players);
	}

	public override IEnumerable<byte> GetBytes() {
		var result = new List<byte>();
		foreach (var player in Players) {
			result.AddRange(BitConverter.GetBytes(player));
		}

		return result;
	}
}