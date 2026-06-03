using System;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	public class ReceiveMovement: MonoBehaviour {
		//==================================================||Fields	

		protected const float GravityScale = -23.75f;
		protected Vector _velocity = new();
		protected int _id;
		private int _hp;
		private int _mp;
		
		//==================================================Properties	
		public int Hp {
			get => _hp;
			private set {
				var delta = value - _hp;
				if (delta > 0) {
					OnHeal?.Invoke(delta);
				}
				else if (delta < 0) {
					OnDamaged?.Invoke(-delta);
				}

				_hp = value;
				if(_hp == 0)
					OnDeath?.Invoke(delta);
			} 
		}
		public int Mp {
			get => _mp; 
			private set {
				var delta = value - _mp;
				if (delta > 0) {
					OnHealMp?.Invoke(delta);
				}
				else if (delta < 0) {
					OnUseMp?.Invoke(-delta);
				}

				_mp = value;
			} 
		}
		
		//0 ~ 360
		protected virtual float Yaw {
			get => transform.rotation.eulerAngles.y;
			set {
				var rotation = transform.rotation.eulerAngles;
				rotation.y = value;
				transform.rotation = Quaternion.Euler(rotation);
			}
		}

		protected virtual float Pitch { get; set;}

		//==================================================||Methods	

		public event Action<int> OnHealMp;
		public event Action<int> OnUseMp;
		public event Action<int> OnHeal;
		public event Action<int> OnDamaged;
		public event Action<int> OnDeath;
		
		public void Init(int pId) {
			_id = pId;
			LogicConnection.Instance.OnReceiveInGame += DataReceive;
		}
		
		private void DataReceive(PacketData pPacket) {
			if (_id != pPacket.Id)
				return;
			if (pPacket.Command != PacketCommand.PlayerData)
				return;
			if (pPacket is not PlayerPacketData player)
				return;
			var pos = player.Pos.ToUnityVector();
			transform.position = pos;
			_velocity = player.Velocity;
			Hp = player.Hp;
			Mp = player.Mp;
		}

		//==================================================||Unity	

		protected virtual void Update() {
			
			_velocity.Y += GravityScale * Time.deltaTime;
			var velo = _velocity;
			velo.Y = MathF.Max(0, velo.Y);
			transform.position += velo.ToUnityVector() * Time.deltaTime;
		}
	}
}