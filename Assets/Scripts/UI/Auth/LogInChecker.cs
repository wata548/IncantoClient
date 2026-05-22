using System;
using Auth;
using UnityEngine;

namespace UI.Auth {
	public class LogInChecker: MonoBehaviour {
		private bool _login = false;
		private bool _waitMatchMaking = false;
		[SerializeField] private string _defaultScreen = "AuthMain";
		[SerializeField] private string _accountScreen = "AccountPage";
		[SerializeField] private string _matchMakingScreen = "MatchMaking";

		private void UpdateLoginUI() {
			var temp = AuthManager.Instance.AccountToken != null;
			if (_login == temp)
				return;
			_login = temp;
			if (_login) {
				ModalBase.Clear();
				ModalBase.GetModal(_accountScreen).Show();
			}
			else {
				ModalBase.Clear();
				ModalBase.GetModal(_defaultScreen).Show();
			}
		}
		private void UpdateMatchMakingUI() {
			if (_waitMatchMaking == AuthManager.Instance.IsMatchMaking)
				return;

			_waitMatchMaking = !_waitMatchMaking;
			if (_waitMatchMaking)
				ModalBase.GetModal(_matchMakingScreen).Show();
			else
				ModalBase.Close();
		}
		
		private void Update() {
			UpdateLoginUI();
			UpdateMatchMakingUI();
		}
	}
}