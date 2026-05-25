using Auth;
using Extension;
using Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame {
    public class Setting: MonoSingleton<Setting> {
        public MatchPlayers Match { get; private set; }
            
        public void StartMatch(MatchPlayers pMatchInfo) {
            
            Match = pMatchInfo;
            LogicConnection.Instance.GameStart();
            SceneManager.LoadScene("Main");
        }
    }
}