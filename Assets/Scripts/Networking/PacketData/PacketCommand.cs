namespace Networking {
	public enum PacketCommand {
		//S -> C
		IdentifyPlayer,
		GameStart,
		SpawnMagic,
		SendResult,
        
		//C -> S
		NATPunch,
		SelectMagic,
		JudgeMagic,
        
		//Shared
		Move,
		Death,
		Rebirth,
        
	}
}