using System;
using InGame.Map;
using Networking;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.InGame {
	public class Result: ModalBase {

		[SerializeField] private TMP_Text _shower;
		[SerializeField] private Button _quit;
		private bool _isInit = false; 
		
		private void Set(ResultData pData) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			var rank = pData.GetRank(Map.Instance.Player.Idx);
			_shower.text = rank switch {
				1 => "1st",
				2 => "2nd",
				3 => "3rd",
				4 => "4th",
				_ => "Error"
			};
		}

		private void QuitGame() {
			SceneManager.LoadScene("Login");
		}

		protected override void Awake() {
			base.Awake();
			_quit.onClick.AddListener(QuitGame);
		}

		private void Update() {
			if (_isInit) return;
			var resultData = Map.Instance.GameResult;
			if (resultData != null) {
				_isInit = true;
				Set(resultData);
				Show();
				Debug.Log("Match End");
			}
		}
	}
}