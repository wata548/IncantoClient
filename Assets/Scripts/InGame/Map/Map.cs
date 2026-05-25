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

        [SerializeField] private Transform[] _playerPositions;
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private ReceiveMovement _otherPlayerPrefab;

        public void SetPlayers(MatchPlayers pMatchPlayers, int pPlayerId) {
            var idx = -1;
            foreach (var (player, pos) in pMatchPlayers.Players.Zip(_playerPositions, (p,t) => (a: p,t.position))) {
                idx++;
                var newPlayer = player == pPlayerId 
                    ? Instantiate(_playerPrefab)
                    :Instantiate(_otherPlayerPrefab);
                newPlayer.Init(player);
                newPlayer.transform.position = pos;
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
        }
    }
}