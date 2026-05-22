using Auth;
using UI.Async;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Login: MonoBehaviour {
	[SerializeField] private string _mail;
	[SerializeField] private string _password;
	[SerializeField] private Button _button;

	private void ExampleLogin() =>
		AsyncLoading.Instance.Set(AuthManager.Instance.SignIn(_mail, _password));

	private void Awake() =>
		_button.onClick.AddListener(ExampleLogin);
}