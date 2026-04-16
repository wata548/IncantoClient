using System;
using Extension.SelectableUI;
using UI.Auth;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class AuthPage : MonoBehaviour {

		[SerializeField] private Button _signInPage;
		[SerializeField] private Button _signUpPage;
		[SerializeField] private ModalBase _signInPanel;
		[SerializeField] private ModalBase _signUpPanel;
		[SerializeField] private string _tag;

		private void Awake() {
			_signInPage.onClick.AddListener(_signInPanel.Show);	
			_signUpPage.onClick.AddListener(_signUpPanel.Show);	
		}

		private void Start() {
			SelectableUIManager.Instance.Open(_tag);
		}
	}
}
