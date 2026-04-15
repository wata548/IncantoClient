using Auth;
using Extension.Test;
using UnityEngine;

namespace DefaultNamespace {
	public static class Test {
		[TestMethod]
		private static void JsonConverterCheck() {
			var data = new SignUpInfo {
				Name = "wata",
				PassWord = "Test",
				Mail = "watashimokia@gmail.com"
			};
			Debug.Log(data.ToString());
		}
	}
}