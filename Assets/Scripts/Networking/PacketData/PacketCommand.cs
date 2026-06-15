namespace Networking {
	public enum PacketCommand {
		//S -> C
		IdentifyPlayer,
		WaitOtherPlayer,
		PlayerData,
		GameStart,
		SpawnMagic,
		SendResult,
		Question, 
		QuestionResult,
        
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