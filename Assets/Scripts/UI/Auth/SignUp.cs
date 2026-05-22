using Auth;
using TMPro;
using UI.Async;
using UI.Messsage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class SignUp: ModalBase {
		[SerializeField] private TMP_InputField _name;	
		[SerializeField] private TMP_InputField _mail;	
		[SerializeField] private TMP_InputField _2fa;	
		[SerializeField] private TMP_InputField _password;	
		[SerializeField] private TMP_InputField _passwordCheck;	
		[SerializeField] private Button _2faInvoke;	
		[SerializeField] private Button _send;	
		[SerializeField] private Button _back;	
		
		public void Send() {
			
			//This process can be skipped; server process this check.
			//but remain DDos threat, so I check temporary.
			if (_password.text != _passwordCheck.text) {
				MessageManager.Instance.Add(new(Status.Fail, "비밀번호 체크에 실패했습니다."));
				return;
			}
			if (_name.text.Length < 3) {
				MessageManager.Instance.Add(new(Status.Fail, "이름이 너무 짧습니다. (최소 3)"));
				return;
			}

			if (_password.text.Length < 8) {
				MessageManager.Instance.Add(new(Status.Fail,  "비밀번호가 너무 짧습니다. (최소 8)"));
				return;
			}
			var args = new SignUpInfo{
				Name = _name.text,
				Mail = _mail.text,
				PassWord = _password.text,
				TwoFactorAuth = _2fa.text
			};
			var task = AuthManager.Instance.SignUp(args);
			AsyncLoading.Instance.Set(task);
		}

		public void TwoFactorAuthorization() {
			var task = AuthManager.Instance.Check2Fa(_mail.text);
			AsyncLoading.Instance.Set(task);
		}
		
		
		protected override void Awake() {
			base.Awake();
			_mail.contentType = TMP_InputField.ContentType.EmailAddress;
			_password.contentType = TMP_InputField.ContentType.Password;
			_passwordCheck.contentType = TMP_InputField.ContentType.Password;
			_2fa.contentType = TMP_InputField.ContentType.IntegerNumber;
			
			_send.onClick.AddListener(Send);
			_2faInvoke.onClick.AddListener(TwoFactorAuthorization);
			_back.onClick.AddListener(Hide);
		}
	}
}