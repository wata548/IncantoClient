using Auth;
using Extension;
using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame {
    public class Setting: MonoSingleton<Setting> {
        private MatchPlayers _match;
        private int _playIdx = 0;
            
        public void SetData(MatchPlayers pMatchInfo) {
            
            _match = pMatchInfo;
            _playIdx = 0;
            foreach (var player in _match.Players) {
                if (player == AuthConnection.Instance.AccountToken.Id)
                    break;
                _playIdx++;
            }
            LogicConnection.Instance.GameStart();
            SceneManager.LoadScene("Main");
        }
    }
}