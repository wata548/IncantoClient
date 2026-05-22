using Auth;
using TMPro;
using UI.Async;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Auth {
	public class AccountPage: ModalBase {
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _id;
		[SerializeField] private Button _enterMatch;
		[SerializeField] private Button _logout;
		private const string NameFormat = "{0}\n<size=70%>{1}</size>";

		public override void Show() {
			base.Show();
			_name.text = string.Format(NameFormat,
				AuthConnection.Instance.AccountToken.Name,
				AuthConnection.Instance.AccountToken.Mail
			);
			_id.text = $"#{AuthConnection.Instance.AccountToken.Id:D10}";
		}

		private void EnterMatch() {
			var task =AuthConnection.Instance.EnterMatchMaking(AuthConnection.Instance.AccountToken);
			AsyncLoading.Instance.Set(task);
		}
		
		protected override void Awake() {
			base.Awake();
			_enterMatch.onClick.AddListener(EnterMatch);		
			_logout.onClick.AddListener(AuthConnection.Instance.LogOut);		
		}
	}
}