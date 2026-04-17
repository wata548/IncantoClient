using System;
using Extension.SelectableUI;
using UI.Auth;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Auth {
	public class AuthPage : ModalBase {

		[SerializeField] private Button _signInPage;
		[SerializeField] private Button _signUpPage;
		[SerializeField] private string _signIn = "SignIn";
		[SerializeField] private string _signUp = "SingUp";

		protected override void Awake() {
			base.Awake();
			_signInPage.onClick.AddListener(() => ModalBase.GetModal(_signIn)?.Show());	
			_signUpPage.onClick.AddListener(() => ModalBase.GetModal(_signUp)?.Show());	
		}

		private void Start() {
			Show();
		}
	}
}
