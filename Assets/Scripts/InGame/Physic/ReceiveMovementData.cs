using Entity;
using Extensions;
using Networking;
using UnityEngine;

namespace Physic {
	public class ReceiveMovementData {
		
		private void Receive(PacketData pData, EntityData pEntity) {
			if (pData.Command != PacketCommand.Move)
				return;
			var moveData = (pData as MoveData)!;
			pEntity.Transform.ApplyMoveData(moveData);
		}
	}
}