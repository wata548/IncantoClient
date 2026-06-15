using Extension;
using Networking;
using UnityEngine.SceneManagement;

namespace InGame {
    public class Setting: MonoSingleton<Setting> {
        public MatchPlayers Match { get; private set; }
        public bool IsMatch { get; private set; } = false;

        public void MatchEnd() {
            IsMatch = false; 
        }
        
        public void StartMatch(MatchPlayers pMatchInfo) {

            if (IsMatch) return;
            IsMatch = true;
            Match = pMatchInfo;
            SceneManager.LoadScene("Main");
        }
    }
}