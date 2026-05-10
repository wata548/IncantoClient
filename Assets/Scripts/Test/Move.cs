using System;
using UnityEngine;

public class Move: MonoBehaviour {

    [SerializeField] private float _speed = 10;
    [SerializeField] private float _mouseSence = 0.1f;
    private void Update() {
        var pos = Camera.main.transform.position;
        var direction = Camera.main.transform.rotation;
        var delta = Vector3.zero;
        var mouseDelta = Input.mousePositionDelta * _mouseSence;
        (mouseDelta.x, mouseDelta.y) = (-mouseDelta.y, mouseDelta.x);
        direction = Quaternion.Euler(direction.eulerAngles + mouseDelta);
        Camera.main.transform.rotation = direction;
        
        if (Input.GetKey(KeyCode.W)) delta.z--;
        if (Input.GetKey(KeyCode.S)) delta.z++;
        if (Input.GetKey(KeyCode.D)) delta.x--;
        if (Input.GetKey(KeyCode.A)) delta.x++; 
        pos -= direction * delta * (_speed * Time.deltaTime);
        Camera.main.transform.position = pos;
    }
}