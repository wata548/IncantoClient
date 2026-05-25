using System;
using Auth;
using TMPro;
using UI.Async;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class MatchMaking: ModalBase {
		[SerializeField] private Button _quit;
		[SerializeField] private TMP_Text _text;
		
		private void QuitMatch() {
			var task = AuthConnection.Instance.QuitMatchMaking(AuthConnection.Instance.AccountToken);
			AsyncLoading.Instance.Set(task);
		}
		
		private void Start() {
			_quit.onClick.AddListener(QuitMatch);
		}

		private void Update() {

			if (!IsActive)
				return;
			_text.text = AuthConnection.Instance.ReadiedPlayerCnt != 0
				? $"Wait other player ({AuthConnection.Instance.ReadiedPlayerCnt} / {MatchPlayers.MatchPerPlayer})"
				: "Match making...";
		}
	}
}