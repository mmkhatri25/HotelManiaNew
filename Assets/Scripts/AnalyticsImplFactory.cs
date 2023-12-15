using System.Collections.Generic;
using UnityEngine;

public class AnalyticsImplFactory
{
	public List<BaseAnalyticsImpl> GetImpls()
	{
		List<BaseAnalyticsImpl> list = new List<BaseAnalyticsImpl>();
		if (Debug.isDebugBuild)
		{
			EditorDebugImpl editorDebugImpl = new EditorDebugImpl();
			editorDebugImpl.Init();
			list.Add(editorDebugImpl);
		}
		FirebaseImpl firebaseImpl = new FirebaseImpl();
		firebaseImpl.Init();
		list.Add(firebaseImpl);
		return list;
	}
}
