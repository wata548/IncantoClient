using Auth;
using UI.Async;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class MatchMaking: ModalBase {
		[SerializeField] private Button _quit;

		private void QuitMatch() {
			var task = AuthManager.Instance.QuitMatchMaking(AuthManager.Instance.AccountToken);
			AsyncLoading.Instance.Set(task);
		}
		
		private void Start() {
			_quit.onClick.AddListener(QuitMatch);
		}
		
	}
}