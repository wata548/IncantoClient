using System;
using UnityEngine;

namespace Extension {
	
	/// <summary>
	/// <p>if you use Serialize field in singleton, override 'AllowAutoGen' -> false</p>
	/// <p>if you don't want to use DonDestroy, override 'IsNarrowSingleton' -> true</p>
	/// </summary>
	/// <typeparam name="T">T must be this(singleton type)</typeparam>
	public abstract class MonoSingleton<T> : MonoBehaviour 
		where T: MonoSingleton<T> {

		//==================================================||Fields		
		private static T _instance = null;

		//==================================================||Properties	
		public static T Instance {
			get {
				if (_instance != null)
					return _instance;
				var obj = new GameObject($"(AutoGen) {typeof(T).Name}");
				obj.AddComponent<T>();
				return _instance;
			}
		}

		protected virtual bool IsNarrowSingleton => false;
		protected virtual bool AllowAutoGen => true;
		
		//==================================================||Unity	
		protected virtual void OnEnable() {
            
			if (_instance != null) {
				if (_instance == this)
					return;
				
				Debug.Log(
					$"{gameObject.name} was deleted,\n" 
					+ $"(Current Singleton: {Instance.gameObject})"
				);
				Destroy(gameObject);
				return;
			}

			if (this is not T instance)
				throw new ArgumentException($"T must be {nameof(T)}. but {GetType().Name}");
			_instance = instance;
			if(!IsNarrowSingleton)
				DontDestroyOnLoad(gameObject);
		}

	}
}