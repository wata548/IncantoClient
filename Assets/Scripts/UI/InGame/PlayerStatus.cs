using System;
using InGame.Map;
using InGame.Physic;
using TMPro;
using UnityEngine;

namespace UI.InGame {
	
	[RequireComponent(typeof(TMP_Text))]
	public class PlayerStatus: MonoBehaviour {

		//==================================================Fields;	
		[Header("FormatString, 0 = hp, 1 = mp")]
		[SerializeField] private string _format = "Hp: {0}, Mp: {1}";
		
		private TMP_Text _stateShower;
		private bool _needUpdate = false;  
		private bool _registerEvent = false;
		private ReceiveMovement _player;
		
		//==================================================Methods	
		private void RegisterEvent() {
			if (_registerEvent)
				return;
			if (Map.Instance == null || !Map.Instance.GameStarted)
				return;
			
			_registerEvent = true;
			_player = Map.Instance.Player;
			Action<int> action = _ => _needUpdate = true;
			_player.OnHeal += action;
			_player.OnDamaged += action;
			_player.OnHealMp += action;
			_player.OnUseMp += action;
			_needUpdate = true;
		}

		private void UpdateUI() {
			if (!_needUpdate)
				return;
			_stateShower.text = string.Format(_format, _player.Hp, _player.Mp);
			_needUpdate = false;
		} 
		
		//==================================================Unity		
		private void Awake() {
			_stateShower = GetComponent<TMP_Text>();
		}
		private void Update() {
			RegisterEvent();
			UpdateUI();
		}
	}
}