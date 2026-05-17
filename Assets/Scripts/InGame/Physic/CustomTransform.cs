using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class CustomTransform {
		
		//==================================================Properties	
		public Vector Velocity { get; private set; }
		public Vector Position { get; private set; }
		public float Rotation { get; private set; }
		private readonly Transform Transform;

		//==================================================Constructors	
		public CustomTransform(Transform pTransform) {
			Transform = pTransform;
			Velocity = new();
			Position = pTransform.position.ToCustomVector();
			Rotation = pTransform.rotation.y;
		}
		
		//==================================================Methods	
		public void ApplyMoveData(MoveData pData) {
			var pos = pData.Pos.ToUnityVector();
			pos += Transform.localScale / 2f;
			Transform.position = pos;
			Transform.rotation = Quaternion.Euler(0, pData.Rotation, 0);
			
			Velocity = pData.Velocity;
			Position = pData.Pos;
			Rotation = pData.Rotation;
		}

		public void Update() {
			var delta = Velocity * Time.deltaTime;
			Position += delta;
			Transform.position = Position.ToUnityVector();
			return;
		} 
	}
}