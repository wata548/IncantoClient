using System;
using System.Linq;
using Auth;
using Extension;
using InGame.Physic;
using UnityEngine;

namespace InGame.Map {
	public class Map: MonoSingleton<Map> {
		protected override bool AllowAutoGen => false;
		protected override bool IsNarrowSingleton => true;
		public bool GameStarted { get; private set; } = false;
		public Player Player { get; private set; }

		[SerializeField] private Player _playerPrefab;
		[SerializeField] private ReceiveMovement _otherPlayerPrefab;

		public void SetPlayers(MatchPlayers pMatchPlayers, int pPlayerId) {
			foreach (var player in pMatchPlayers.Players) {
				var newPlayer = null as ReceiveMovement;
				if (player == pPlayerId) {
					Player = Instantiate(_playerPrefab);
					newPlayer = Player;
				}
				else newPlayer = Instantiate(_otherPlayerPrefab);
                    
				newPlayer.Init(player);
			}
		}

		private void Start() {
			//TODO: Test Code
			if (Setting.Instance.Match == null) {
				Instantiate(_playerPrefab)
					.Init(-1);
				return;
			}
            
			SetPlayers(
				Setting.Instance.Match, 
				AuthConnection.Instance.AccountToken.Id
			);
			GameStarted = true;
		}
	}
}