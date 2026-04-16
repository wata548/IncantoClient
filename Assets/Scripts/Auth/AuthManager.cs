using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Extension;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Auth {
	
	
	public static class AuthManager {
		private static readonly HttpClient Client = new();
		private static readonly Uri ServerAddress = new("https://incanto.o-r.kr:7272");
		private static AccountToken AccountToken;
		private static AsyncDataBase<Result> _task = null;
		public static Result Result =>
			_task?.Value;

		private static AsyncDataBase<Result> Call(string pReq, HttpContent pArgs) {
			return new AsyncData<string, Result>(CallAsync(), JsonConvert.DeserializeObject<Result>);
			
			async Task<string> CallAsync() {
				var res = await Client.PostAsync(new Uri(ServerAddress,pReq), pArgs);
				var ouput = await res.Content.ReadAsStringAsync();
				return ouput;
			}
		}
		

		private static Result LoginProcess(Result pResult) {
			if (pResult.Status == Status.Success) {
				Debug.Log(pResult.Context);
				AccountToken = JsonConvert.DeserializeObject<AccountToken>(pResult.Context);
				pResult = new(Status.Success, "성공적으로 로그인 되었습니다.");
			}
			return pResult;
		} 
		
		
		public static AsyncDataBase<Result> SignIn(string pMail, string pPassword ) {
			var info = new SignUpInfo() {
				Mail = pMail,
				PassWord = pPassword
			};
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignIn", content);
			result.CallBackTToT += LoginProcess;
			return result;
		}

		public static AsyncDataBase<Result> Check2Fa(string pMail) {
			var info = new SignUpInfo() { Mail = pMail };
			var content = new StringContent(info.ToString(), Encoding.UTF8, "application/json");
			return Call("2fa", content);	
		}

		public static AsyncDataBase<Result> SignUp(SignUpInfo pInfo) {
			var content = new StringContent(pInfo.ToString(), Encoding.UTF8, "application/json");
			var result = Call("SignUp", content);	
			result.CallBackTToT += LoginProcess;
			return result;
		}
	}
}