using System;
using Auth;
using UnityEngine;

namespace UI.Auth {
	public class LogInChecker: MonoBehaviour {
		private bool _login = false;
		[SerializeField] private string _defaultScreen = "AuthMain";
		[SerializeField] private string _accountScreen = "AccountPage";

		private void Update() {
			var temp = AuthManager.AccountToken != null;
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
	}
}