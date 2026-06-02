namespace Networking {
	public enum PacketCommand {
		//S -> C
		IdentifyPlayer,
		WaitOtherPlayer,
		PlayerData,
		GameStart,
		SpawnMagic,
		SendResult,
        
		//C -> S
		NATPunch,
		Move,
		SelectMagic,
		JudgeMagic,
        
		//Shared
		Death,
		Rebirth,
        
	}
}