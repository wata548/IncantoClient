using Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class AccountPage: ModalBase {
		[SerializeField] private TMP_Text _name;
		[SerializeField] private Button _enterMatch;
		[SerializeField] private Button _logout;
		private const string NameFormat = "{0}\n<size=70%>{1}</size>";

		public override void Show() {
			base.Show();
			_name.text = string.Format(NameFormat,
				AuthManager.AccountToken.Name,
				AuthManager.AccountToken.Mail
			);
		}
		
		protected override void Awake() {
			base.Awake();
			_enterMatch.onClick.AddListener(null);		
			_logout.onClick.AddListener(AuthManager.LogOut);		
		}
	}
}