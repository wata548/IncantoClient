using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Auth;
using Extension.Test;
using Networking;
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

		[TestMethod]
		private static void DataSendTest() {
			/*
			var data = new PacketData() {
				Id = 4,
				Args = new[] { new ValueData<int> { Value = 1 } }
			};
			var bytes = PacketData.Serialize(data);
			Debug.Log(Encoding.UTF8.GetString(bytes));
			*/
		}

		[TestMethod]
		private static void TestUDPAccuracy(int pCnt) {
			var a = new DataModule();
			for (int i = 1; i <= pCnt; i++) {
				a.SendRaw(i);
			}
		}
		
		[TestMethod]
		private static void Circle(int pSize) {
			const string Biomes = "⬛⬜";
			var radius = pSize / 2f;
			var list = new bool[pSize * pSize];

			var result = new StringBuilder();
			for (var i = 0; i < pSize; i++) {
				for (var j = 0; j < pSize; j++) {
					var idx = IndexOf(i, j);
					list[idx] = IsInCircle(i, j);
					result.Append(Biomes[list[idx] ? 1 : 0]);
				}

				result.AppendLine();
			}
			Debug.Log(result);
			
			return;

			bool IsInCircle(int pX, int pY) {
				var x = pX - radius;
				var y = pY - radius;
				var r = (int)Math.Sqrt(x * x + y * y);
				Debug.Log($"{x}, {y} = {x * x + y * y}");
				return radius >= r;
			}
			int IndexOf(int pX, int pY) => pX + pY * pSize;
		}
	}
}