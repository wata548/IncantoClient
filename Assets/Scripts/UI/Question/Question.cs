using System.Collections;
using InGame.Map;
using Networking;
using TMPro;
using UnityEngine;

namespace UI.Question {
	public class Question: MonoBehaviour {
		[SerializeField] private TMP_Text _context;
		[SerializeField] private TMP_Text _correct;
		private Coroutine _coroutine;

		private void ReceivePacket(PacketData pPacket) {
			if (pPacket is QuestionData q) {
				_context.text = q.Context;
			}
			else if (pPacket.Command is PacketCommand.QuestionResult) {
				_context.text = "";
				if(_coroutine != null)
					StopCoroutine(_coroutine);
				if(Map.Instance.Player.Idx == pPacket.Id)
					_coroutine = StartCoroutine(CorrectEffect());
			}
		}

		private IEnumerator CorrectEffect(float pDuration = 1f) {
			var time = 0f;
			while (time < pDuration) {
				time += Time.deltaTime;
				yield return null;
				_correct.alpha = time / pDuration;
			}
			_correct.alpha = 1;

			while (time > 0) {
				time -= Time.deltaTime;
				yield return null;
				_correct.alpha = time / pDuration;
			}
			_correct.alpha = 0;
		}

		private void Start() {
			LogicConnection.Instance.OnReceiveInGame += ReceivePacket;
		}
	}
}