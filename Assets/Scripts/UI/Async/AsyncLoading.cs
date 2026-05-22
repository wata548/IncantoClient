using System;
using System.Collections.Generic;
using System.Linq;
using Auth;
using Extension;
using TMPro;
using UI.Messsage;
using UnityEngine;

namespace UI.Async {
	public class AsyncLoading: MonoSingleton<AsyncLoading> {
		protected override bool IsNarrowSingleton => true;
		protected override bool AllowAutoGen => false;

		//==================================================|| Fields	
		[SerializeField] private string _tag;
		[SerializeField] private float _updateInterval = 0.2f;
		[SerializeField] private GameObject _panel;
		[SerializeField] private TMP_Text _loadingText;
		private AsyncDataBase<Result> _task;
		private Queue<AsyncDataBase<Result>> _processQueue = new();
		private string LoadingText = "Now Loading{0}\n<size=70%>Status: {1}</size>";
		private int _idx = 1;
		private float _term; 

		//==================================================|| Methods	
		public void Set(AsyncDataBase<Result> pData) {
			_processQueue.Enqueue(pData);
		}

		private bool OnOffCheck() {
			if (_task != null) return true;
			
			if (!_processQueue.TryDequeue(out _task)) {
				_panel.SetActive(false);
				return false;
			}
			
			_panel.SetActive(true);
			_term = _updateInterval;
			return true;
		}

		private void LoadingMessageUpdate() {
			if ((_term -= Time.deltaTime) >= 0)
				return;

			_term += _updateInterval;
			_idx = (_idx + 1) % 4;
			_loadingText.text = string.Format(
				LoadingText,
				new string(Enumerable.Repeat('.', _idx).ToArray()), 
				_task.Status
			);
		}

		private void MessageUpdate() {
			try {
				if (_task.Value == null)
					return;

				MessageManager.Instance.Add(_task.Value);
				_task = null;
			}
			catch {
				_task = null;
				MessageManager.Instance.Add(new(Status.Fail, "오류가 발생했습니다."));
			}
			
		}
		
		
		//==================================================|| Unity	
		private void Update() {

			if (!OnOffCheck())
				return;

			LoadingMessageUpdate();
			MessageUpdate();
		}
	}
}