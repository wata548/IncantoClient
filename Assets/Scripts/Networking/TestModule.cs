using System;
using System.Net;
using Auth;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Networking {
    public class TestModule: MonoBehaviour {
        private DataModule _module;

        private void Awake() {
            _module = new();
            var message = "";
            #if UNITY_EDITOR
            message = Random.Range(0, 123456789).ToString().PadLeft(9, '0');
            #else
            message = AuthManager.AccountToken.Guid;
            #endif
            _module.SendRaw(message);
        }

        private void Update() {
            var temp = _module.LastMessage;
            if (!string.IsNullOrEmpty(temp)) 
                Debug.Log(temp);
        }
    }
}