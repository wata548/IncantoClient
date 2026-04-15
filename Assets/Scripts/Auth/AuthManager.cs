using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Auth {
	public static class AuthManager {
		private static readonly HttpClient Client = new();
		private static readonly Uri ServerAddress = new("https://incanto.o-r.kr:7272");
		private static AccountToken AccountToken;

		private static Result Call(string pReq, HttpContent pArgs) {
			var task = Client.PostAsync(new Uri(ServerAddress,pReq), pArgs);
			task.Wait();
			var res = task.Result;
			var outputTask = res.Content.ReadAsStringAsync();
			outputTask.Wait();
			return JsonConvert.DeserializeObject<Result>(outputTask.Result);
		}

		private static Result LoginProcess(Result pResult) {
			if (pResult.Status == Status.Success) {
				Debug.Log(pResult.Context);
				AccountToken = JsonConvert.DeserializeObject<AccountToken>(pResult.Context);
				pResult = new(Status.Success, "성공적으로 로그인 되었습니다.");
			}
			return pResult;
		} 
		
		
		public static Result SignIn(string pMail, string pPassword ) {
			var info = new SignUpInfo() {
				Mail = pMail,
				PassWord = pPassword
			};
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignIn", content);
			return LoginProcess(result);
		}

		public static Result Check2Fa(string pMail) {
			var info = new SignUpInfo() { Mail = pMail };
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			return Call("2fa", content);	
		}

		public static Result SignUp(SignUpInfo pInfo) {
			var content = new StringContent(pInfo.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignUp", content);	
			return LoginProcess(result);
		}
	}
}