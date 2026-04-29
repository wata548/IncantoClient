using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace InGame.Drawing {
	[RequireComponent(typeof(Image))]
	public class Draw: MonoBehaviour {
		private RenderTexture _texture;
		private Material _mat;
		[SerializeField] private int _size;
		 
		private void Awake() {
			_mat = GetComponent<Image>().material;
			Clear();
		}

		private void Clear() {
			if(_texture is not  null)
				RenderTexture.ReleaseTemporary(_texture);
				
			_texture = RenderTexture.GetTemporary(
				_size,
				_size,
				0,
				RenderTextureFormat.ARGB32
			);
			_texture.Create();

			var prev = RenderTexture.active;
			RenderTexture.active = _texture;
			GL.Clear(true, true, Color.clear);
			RenderTexture.active = prev;
			
			_mat.SetTexture("_MainTex", _texture);
		}
			
		
		private void DrawUpdate() {
			var temp = RenderTexture.GetTemporary(
				_texture.width,
				_texture.height
			);
			Graphics.Blit(_texture, temp);
			Graphics.Blit(temp, _texture, _mat);
			RenderTexture.ReleaseTemporary(temp);
		}
		
		private void Update() {
			if(Input.GetKeyDown(KeyCode.Escape))
				Clear();
			if (Input.GetKeyDown(KeyCode.Space)) {
				var random = new Random();
				var x = (float)random.NextDouble();
				var y = (float)random.NextDouble();
				_mat.SetVector("_Pos", new(x, y));
				DrawUpdate();
			}
			
		}
	}
}