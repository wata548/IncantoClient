using Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class SignIn: ModalBase {
		[SerializeField] private TMP_InputField _mail;
		[SerializeField] private TMP_InputField _password;
		[SerializeField] private Button _invoke;
		[SerializeField] private Button _back;

		private void Invoke() {
			var mail = _mail.text;
			var password = _password.text;
			AuthManager.SignIn(mail, password);
		}
		
		private void Awake() {
			_mail.contentType = TMP_InputField.ContentType.Standard;
			_password.contentType = TMP_InputField.ContentType.Password;
			_invoke.onClick.AddListener(Invoke);
			_back.onClick.AddListener(Hide);
		}
	}
}