using Auth;
using Extension.SelectableUI;
using TMPro;
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
			var result = AuthManager.SignIn(mail, password);
			MessageManager.Instance.Add(result);
		}
		
		private void Awake() {
			_mail.contentType = TMP_InputField.ContentType.EmailAddress;
			_password.contentType = TMP_InputField.ContentType.Password;
			_invoke.onClick.AddListener(Invoke);
			_back.onClick.AddListener(Hide);
		}


		public override void Show() {
			base.Show();
			SelectableUIManager.Instance.Open(Tag);
		}

		public override void Hide() {
			base.Hide();
			SelectableUIManager.Instance.Close();
		}
	}
}