using System;
using System.Linq;
using CSVData;
using CSVData.Extensions;
using Extension.Test;
using Extensions;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace InGame.Drawing {
	[RequireComponent(typeof(Image))]
	public class Draw: MonoBehaviour {
		
		//==================================================Fields	
		[SerializeField] private int _size = 128;
		[SerializeField] private Color _color = Color.yellow;
		[SerializeField] private Color _backgroundColor = Color.white;
		private RenderTexture _texture;
		private Material _mat;
		
		//==================================================Methods	
		[TestMethod]
		public override string ToString() {
			var currentTexture = RenderTexture.active;
			RenderTexture.active = _texture;
			var tex = new Texture2D(
				_texture.width,
				_texture.height,
				TextureFormat.RGBA32,
				false
			);
			tex.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
			tex.Apply();
			RenderTexture.active = currentTexture;
			
			var colors = tex.GetPixels(0, 0, _texture.width, _texture.height);
			var temp = colors.Select(pixel =>
				Mathf.Approximately(pixel.r, _color.r) &&
				Mathf.Approximately(pixel.g, _color.g) &&
				Mathf.Approximately(pixel.b, _color.b)
			).ToArray();
			var result = temp.ToOptimizedString();
			Debug.Log(result);
			return result;
		}
		
		protected void Clear() {
			if(_texture is not  null)
				RenderTexture.ReleaseTemporary(_texture);
				
			_mat.SetColor("_Color", _color);
			_texture = RenderTexture.GetTemporary(
				_size,
				_size,
				0,
				RenderTextureFormat.ARGB32
			);
			_texture.filterMode = FilterMode.Point;
			_texture.antiAliasing = 1;
			_texture.Create();

			var prev = RenderTexture.active;
			RenderTexture.active = _texture;
			GL.Clear(true, true, _backgroundColor);
			RenderTexture.active = prev;
			
			_mat.SetTexture("_MainTex", _texture);
		}
		
		protected void DrawUpdate() {
			var temp = RenderTexture.GetTemporary(
				_texture.width,
				_texture.height
			);
			Graphics.Blit(_texture, temp);
			Graphics.Blit(temp, _texture, _mat);
			RenderTexture.ReleaseTemporary(temp);
		}

		protected void MoveCursor(Vector4 pPos) {
			_mat.SetVector("_Pos", pPos);
		}

		protected Vector3 GetCursor() => _mat.GetVector("_Pos");
		
		//==================================================Unity	
		protected virtual void Awake() {
			_mat = GetComponent<Image>().material;
			Clear();
		}
	}
}