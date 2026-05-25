namespace Networking {
	public enum PacketCommand {
		//S -> C
		IdentifyPlayer,
		WaitOtherPlayer,
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