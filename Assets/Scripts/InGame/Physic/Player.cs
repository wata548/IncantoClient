using System;
using Networking;
using UnityEngine;

namespace InGame.Physic {
    public class Player: SendMovement {
        [SerializeField] private float _sensibility = 1f;
        
        protected Camera _camera;
        private InputChecker _input = new();
        
       //==================================================||Methods 
       protected override float Pitch {
           get => _camera.transform.rotation.eulerAngles.y;
           set {
               var rotation = _camera.transform.rotation.eulerAngles;
               rotation.y = value;
               _camera.transform.rotation = Quaternion.Euler(rotation);
           }
       }

       protected override float Yaw {
           get => _camera.transform.rotation.eulerAngles.x;
           set {
               var rotation = _camera.transform.rotation.eulerAngles;
               rotation.x = value;
               _camera.transform.rotation = Quaternion.Euler(rotation);       
           }
       }

       protected override InputFlags GetInput() => _input.GetInput();

        private void CameraPositionUpdate() {
            var delta = Input.mousePositionDelta * _sensibility;
            var rotation = _camera.transform.rotation.eulerAngles;
            rotation.x += -delta.y;
            rotation.y += delta.x;
            _camera.transform.rotation = Quaternion.Euler(rotation);
        }
        
       //==================================================||Unity 
        protected override void Update() {
            CameraPositionUpdate();
            base.Update();
        }
        
        private void Awake() {
            _camera = Camera.main!;
        }
    }
}