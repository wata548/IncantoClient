using Auth;
using Extension.SelectableUI;
using TMPro;
using UI.Async;
using UI.Messsage;
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
			var task = AuthManager.SignIn(mail, password);
			AsyncLoading.Instance.Set(task);
		}
		
		protected override void Awake() {
			base.Awake();
			_mail.contentType = TMP_InputField.ContentType.EmailAddress;
			_password.contentType = TMP_InputField.ContentType.Password;
			_invoke.onClick.AddListener(Invoke);
			_back.onClick.AddListener(Hide);
		}
	}
}
