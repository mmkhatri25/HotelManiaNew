using System.IO;
using UnityEngine;
using System;
using DG.Tweening;

namespace com.F4A.MobileThird
{
	using System.Collections;
#if UNITY_EDITOR
	using UnityEditor;
#endif
#if UNITY_2018_3_OR_NEWER
	using UnityEngine.Networking;
	using UnityEngine.UI;
#endif

	public static class F4AUtils
	{
		// CoreData.Instance.infoData.SavePlayerCoin\((.+)\);
		// CoreData.Instance.infoData.PlayerCoin = $1;

		public const string KeyRemoveAds = "RemoveAds";

		public const string KeySfx = "KeySfx", KeyMusic = "KeyMusic", KeyVibrate = "KeyVibrate";

		//public static T[] Fill(this T[] array, T value) where T : struct
		//{
		//    // number of loop is Log(length arry, 2). ex: 1 -> 2 -> 4 -> 6
		//    // Log return by value is not pow of 2 is a double (we need int), so we need round up it
		//    double numberOfLoop = System.Math.Ceiling(System.Math.Log(array.Length, 2));
		//    // Set value to first element
		//    array[0] = value;
		//    // Get size of value type
		//    int sizeTypeValue = Marshal.SizeOf(value.GetType());
		//    // start copy elements from 1 to log(array lenght, 2)
		//    int currentPos = 1;            while (numberOfLoop > 1)
		//    {
		//        // Copy previous data to next data
		//        Buffer.BlockCopy(array, 0, array, currentPos  sizeTypeValue, currentPos  sizeTypeValue);
		//        // After copy, we shift left currentPos to next posion
		//        currentPos = currentPos << 1;
		//        numberOfLoop--;
		//    }
		//    // Copy last items with leghth is diff of array lenghth and log(array lenght, 2)
		//    System.Array.Copy(array, 0, array, currentPos, array.Length - currentPos);
		//    return array;
		//}


		public static string GetStreamingAssetsPath()
		{
#if UNITY_EDITOR
			//return Application.streamingAssetsPath; // Use the build output folder directly.
			return Path.Combine(Directory.GetCurrentDirectory(), "StreamingAssets");
#elif UNITY_WEBGL
			return Path.GetDirectoryName(Application.absoluteURL).Replace("\\", "/")+ "/StreamingAssets";
#elif UNITY_STANDALONE
            //return Application.streamingAssetsPath;
            return Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IOS
            return Application.dataPath + "/Raw";
#else
            return "file://" + Application.streamingAssetsPath;
#endif
		}

		public static void CreateDirectory(string path) {
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (!directoryInfo.Parent.Exists)
			{
				CreateDirectory(directoryInfo.Parent.FullName);
			}
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public static T? ParseEnum<T>(string value) where T : struct, IConvertible
		{
			T? item = null;

			foreach (T type in Enum.GetValues(typeof(T))) {
				if (type.ToString().ToLower().Equals(value.Trim().ToLower())) {
					item = type;
					break;
				}
			}

			return item;
		}

		#region Double
		public static void SetDouble(string key, double value)
		{
			PlayerPrefs.SetString(key, DoubleToString(value));
		}

		public static double GetDouble(string key, double defaultValue)
		{
			string defaultVal = DoubleToString(defaultValue);
			return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
		}

		public static double GetDouble(string key)
		{
			return GetDouble(key, 0d);
		}

		private static string DoubleToString(double target)
		{
			return target.ToString("R");
		}

		private static double StringToDouble(string target)
		{
			if (string.IsNullOrEmpty(target))
				return 0d;

			return double.Parse(target);
		}
		#endregion


		public static double GetCurrentTime()
		{
			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
			return span.TotalSeconds;
		}

		public static double GetCurrentTimeInDays()
		{
			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
			return span.TotalDays;
		}

		public static double GetCurrentTimeInMills()
		{
			TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
			return span.TotalMilliseconds;
		}

		public static T GetRandom<T>(params T[] arr)
		{
			return arr[UnityEngine.Random.Range(0, arr.Length)];
		}

		public static bool IsActionAvailable(String action, int time, bool availableFirstTime = true)
		{
			if (!PlayerPrefs.HasKey(action + "_time")) // First time.
			{
				if (availableFirstTime == false)
				{
					SetActionTime(action);
				}
				return availableFirstTime;
			}

			int delta = (int)(GetCurrentTime() - GetActionTime(action));
			return delta >= time;
		}

		public static double GetActionDeltaTime(String action)
		{
			if (GetActionTime(action) == 0)
				return 0;
			return GetCurrentTime() - GetActionTime(action);
		}

		public static void SetActionTime(String action)
		{
			SetDouble(action + "_time", GetCurrentTime());
		}

		public static void SetActionTime(String action, double time)
		{
			SetDouble(action + "_time", time);
		}

		public static double GetActionTime(String action)
		{
			return GetDouble(action + "_time");
		}

		public static void OpenURL(string url)
		{
			if (!string.IsNullOrEmpty(url))
			{
#if UNITY_EDITOR
				Application.OpenURL(url);
#elif UNITY_WEBGL
				//Application.ExternalEval("window.open(\"" + url + "\",\"_blank\")");
				openWindow(url);
#else
				Application.OpenURL(url);
#endif
			}
		}

#if UNITY_WEBGL
		[DllImport("__Internal")]
		private static extern void openWindow(string url);
#endif



		#region Get Data From Server
		public static IEnumerator AsyncGetDataFromUrl(
#if UNITY_2018_3_OR_NEWER
		//Action<UnityWebRequest> action
#else
			Action<WWW> action
#endif
		)
		{
			if (true)
			{
#if UNITY_2018_3_OR_NEWER
				// UnityWebRequest www = UnityWebRequest.Get(url);
				// www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif


			}
			yield return null;
		}

		public static IEnumerator AsyncGetDataFromUrl(string url, Action<string> action)
		{
			string textData = "";
			if (!string.IsNullOrEmpty(url))
			{
#if UNITY_2018_3_OR_NEWER
				// UnityWebRequest www = UnityWebRequest.Get(url);
				// www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif


			}
			if (action != null) action(textData);
			yield return null;
		}

		public static IEnumerator AsyncGetDataFromUrl(string url, TextAsset textAssetDefault, Action<string> action)
		{
			string textData = "";
			if (!string.IsNullOrEmpty(url))
			{
#if UNITY_2018_3_OR_NEWER
				//UnityWebRequest www = UnityWebRequest.Get(url);
				//www.SendWebRequest();
#else
				WWW www = new WWW(url);
#endif



				if (string.IsNullOrEmpty(textData) && textAssetDefault)
				{
					textData = textAssetDefault.text;
				}

				if (action != null) action(textData);
				yield return null;
			}

			#endregion

#if UNITY_EDITOR
			#region Defines



			#endregion
#endif


			#region F4A
			
			#endregion
		}
	}
}