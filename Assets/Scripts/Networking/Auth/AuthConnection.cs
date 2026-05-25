using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DefaultNamespace;
using Extension;
using InGame;
using Networking;
using Newtonsoft.Json;
using UnityEngine;

namespace Auth {
	public class AuthConnection: MonoSingleton<AuthConnection> {
		//==================================================Properties	
		public bool IsMatchMaking => _waitStatus.IsMatchMaking;
		public int ReadiedPlayerCnt => _waitStatus.ReadiedPlayerCnt;
		public AccountToken AccountToken { get; private set; }

		//==================================================Fields	
		private readonly HttpClient _client = new();
		private readonly Uri _serverAddress = new("https://incanto.o-r.kr:7272");
		private float _remainTime = ServerSetting.UpdateTerm;
		private readonly WaitStatus _waitStatus = new();
		
		//==================================================Methods
		public void LogOut() {
			AccountToken = null;
		}
		
		private AsyncDataBase<Result> Call(string pReq, HttpContent pArgs) {
			return new AsyncData<string, Result>(CallAsync(), JsonConvert.DeserializeObject<Result>);
			
			async Task<string> CallAsync() {
				var res = await _client.PostAsync(new Uri(_serverAddress,pReq), pArgs);
				var ouput = await res.Content.ReadAsStringAsync();
				return ouput;
			}
		}
		
		private Result LoginProcess(Result pResult) {
			if (pResult.Status == Status.Success) {
				Debug.Log(pResult.Context);
				AccountToken = JsonConvert.DeserializeObject<AccountToken>(pResult.Context);
				pResult = new(Status.Success, "성공적으로 로그인 되었습니다.");
			}
			return pResult;
		} 
		
		
		public AsyncDataBase<Result> SignIn(string pMail, string pPassword ) {
			var info = new SignUpInfo {
				Mail = pMail,
				PassWord = pPassword
			};
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignIn", content);
			result.CallBackTToT += LoginProcess;
			return result;
		}

		public AsyncDataBase<Result> Check2Fa(string pMail) {
			var info = new SignUpInfo { Mail = pMail };
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			return Call("2fa", content);	
		}

		public AsyncDataBase<Result> SignUp(SignUpInfo pInfo) {
			var content = new StringContent(pInfo.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignUp", content);	
			result.CallBackTToT += LoginProcess;
			return result;
		}
		
		public AsyncDataBase<Result> EnterMatchMaking(AccountToken pToken) {
			var content = new StringContent(pToken.ToString(), Encoding.UTF8, "application/json");
			var result = Call("JoinMatch", content);
			result.CallBackTToT += r => {
				if (r.Status == Status.Success)
					_waitStatus.EnterMatchMaking();
				return r;
			};
			return result;
		}

		public AsyncDataBase<Result> QuitMatchMaking(AccountToken pToken) {
			var content = new StringContent(pToken.ToString(), Encoding.UTF8, "application/json");
			var result = Call("ExitMatch", content);
			result.CallBackTToT += r => {
				if (r.Status == Status.Success)
					_waitStatus.QuitMatchMaking();
				return r;
			};
			return result;
		}

		private void WaitMatchMaking() {
			if (!IsMatchMaking)
				return;
			if (_remainTime > 0) {
				_remainTime -= Time.deltaTime;
				return;
			}
            
			_remainTime = ServerSetting.UpdateTerm;
			var natPunch = new PacketData {
				Command = PacketCommand.NATPunch,
				Id = AccountToken.Id
			};
			var bytes = natPunch.GetBytes().ToArray();
			LogicConnection.Instance.Send(bytes);
		}

		private void Receive(PacketData pPacketData) {
			switch(pPacketData.Command) {
				case PacketCommand.WaitOtherPlayer:
					_waitStatus.Ready(pPacketData.Id);
					break;
				case PacketCommand.IdentifyPlayer:
					if (pPacketData is not IdentifyPlayer identify)
						break;
					
					_waitStatus.StartMatch();
					Setting.Instance.StartMatch(identify.Match);
					break;
			}
		}

		//==================================================Unity
		private void Start() {
			LogicConnection.Instance.OnReceive += Receive;
		}
		
		private void Update() {
			WaitMatchMaking();		
		}

		private class WaitStatus {
			private bool _isFindingMatch = false;
			public int ReadiedPlayerCnt { get; private set; } = 0;
			public bool IsMatchMaking => _isFindingMatch || ReadiedPlayerCnt != 0;
			
			public void Ready(int pReadiedPlayerCnt) {
				if (!_isFindingMatch)
					return;
				ReadiedPlayerCnt = pReadiedPlayerCnt;
			}

			public void EnterMatchMaking() => _isFindingMatch = true;
			public void QuitMatchMaking() => _isFindingMatch = false;
			
			public void StartMatch() {
				_isFindingMatch = false;
				ReadiedPlayerCnt = 0;
			}
		}
	}
}