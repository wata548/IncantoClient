using System;
using BVH;
using Extensions;
using Networking;
using UnityEngine;

namespace InGame.Physic {
	[RequireComponent(typeof(MeshRenderer))]
	public class ReceiveMovement: MonoBehaviour {
		//==================================================||Fields	
		private static readonly Vector3 _deadPos = new(0, 15, 0);
		protected Vector _velocity = new();
		private int _hp = 1;
		private int _mp = 1;
		
		//==================================================Properties	
		public int Idx { get; private set; }
		public bool IsDead => Hp <= 0;
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
				if(_hp <= 0)
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
			Idx = pId;
			LogicConnection.Instance.OnReceiveInGame += DataReceive;
			OnDeath += OnDeathCallback;
		}

		private void OnDeathCallback(int pV) {
			GetComponent<MeshRenderer>().enabled = false;
			transform.position = _deadPos;
		}
		
		private void DataReceive(PacketData pPacket) {
			if (IsDead) return;
			
			if (Idx != pPacket.Id)
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
			if (IsDead) return;
			_velocity.Y += ExInputFlags.GravityScale * Time.deltaTime;
			var velo = _velocity;
			velo.Y = MathF.Max(0, velo.Y);
			transform.position += velo.ToUnityVector() * Time.deltaTime;
		}
	}
}