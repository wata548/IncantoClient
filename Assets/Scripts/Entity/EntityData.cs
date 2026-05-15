using System;
using Entity;
using Networking;
using Physic;
using UnityEngine;

namespace Entity {
	public class EntityData {
        
		//==================================================||Events 
		public event Action<int, EntityData> OnHealReceive;
		public event Action<int, EntityData> OnHealSend;
		public event Action<int, EntityData> OnDamageReceive;
		public event Action<int, EntityData> OnDamageSend;
		public event Action<int> OnDeath;
		//==================================================|| Constructors
       
		public EntityData(int pId, int pMaxHp, Transform transform) {
			Id = pId;
			MaxHp = pMaxHp;
			Transform = new(transform);
		}
        
		//==================================================||Properties 
		public readonly int Id;
		public readonly CustomTransform Transform;
		public int MaxHp { get; private set; }
		public int Hp { get; private set; }
        
		//==================================================||Methods 
		private void ReceiveDamage(int pAmount, EntityData pExecutor) {
			OnDamageReceive?.Invoke(pAmount, pExecutor);
			Hp = Mathf.Max(Hp - pAmount, 0);
			if(Hp == 0)
				OnDeath?.Invoke(pAmount);
		}

		public void Attack(int pAmount, EntityData pTarget) {
			OnDamageSend?.Invoke(pAmount, pTarget);
			pTarget.ReceiveDamage(pAmount, this);
		}

		private void ReceiveHeal(int pAmount, EntityData pExecutor) {
			OnHealReceive?.Invoke(pAmount, pExecutor);
			Hp = Mathf.Min(Hp + pAmount, MaxHp);
		}

		public void Heal(int pAmount, EntityData pTarget) {
			OnHealSend?.Invoke(pAmount, pTarget);
			pTarget.ReceiveHeal(pAmount, this);
		}

		public void SetMaxHp(int pAmount) =>
			MaxHp = pAmount;
	}
}