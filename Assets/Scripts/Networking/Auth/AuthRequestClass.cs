using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auth {
	public class SignUpInfo {
		public string Name { get; set; }
		public string Mail { get; set; }
		public string PassWord { get; set; }
		public string TwoFactorAuth { get; set; }

		public override string ToString() =>
			JsonConvert.SerializeObject(this);
	}

	public class AccountToken {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Mail { get; set; }
		public string Guid { get; set; }
		
		public override string ToString() =>
			JsonConvert.SerializeObject(this);
	}
	
	[JsonConverter(typeof(StringEnumConverter), true)]
	public enum Status {
		Success,
		Fail,
	} 
	
	public class Result {

		public Status Status { get; set; }
		public string Context{ get; set; }
		
		public Result(Status pStatus, string pContext) {
			Status = pStatus;
			Context = pContext;
		}
		
	}

	public class Result<T> : Result {
		public Result(Status pStatus, T pContext): base(pStatus, JsonConvert.SerializeObject(pContext)) {}
	}
}