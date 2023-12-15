using UnityEngine;

namespace Alg
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T instance;

		private static bool isInit;

		private static bool isDestroying;

		private bool isDuplicate;

		public static T Instance
		{
			get
			{
				if ((Object)instance == (Object)null && !isDestroying)
				{
					GameObject gameObject = new GameObject(typeof(T).ToString());
					instance = gameObject.AddComponent<T>();
					Object.DontDestroyOnLoad(gameObject);
					if (!isInit)
					{
						instance.Init();
					}
				}
				return instance;
			}
		}

		public static void KillGameObject()
		{
			if ((Object)Instance != (Object)null)
			{
				UnityEngine.Object.Destroy(instance.gameObject);
				instance = null;
			}
		}

		public void Awake()
		{
			T y = (T)this;
			if ((Object)instance == (Object)null)
			{
				instance = y;
				//Object.DontDestroyOnLoad(base.gameObject);
				if (!isInit)
				{
					Init();
				}
			}
			else if ((Object)instance != (Object)y)
			{
				isDuplicate = true;
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		protected virtual void Init()
		{
			isInit = true;
		}

		private void OnDestroy()
		{
			if (!isDuplicate)
			{
				isDestroying = true;
			}
		}
	}
}
