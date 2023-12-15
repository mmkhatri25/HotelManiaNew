using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace com.F4A.MobileThird
{
	[CustomEditor(typeof(F4ACoreManager))]
	[CanEditMultipleObjects]
	public class F4ACoreEditor : Editor
	{
		F4ACoreManager _coreMobile = null;
		
		private void OnEnable(){
			_coreMobile = (F4ACoreManager)target;

       //   string defines = DMCMobileUtils.GetDefines();
         // string[] array = defines.Split(';');

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Set Defines"))
            {
              //string defines = DMCMobileUtils.GetDefines();
              //string definesBefore = defines;
              //var array = defines.Split(';');

     
             // defines = CheckDefine(defines, array, _coreMobile.DefineMixPanel, DMCMobileUtils.DEFINE_MIXPANEL);

                
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Add AdsManager"))
            {
                _coreMobile.AddAdsManager();
            }
            if (GUILayout.Button("Add SocialManager"))
            {
                _coreMobile.AddSocialManager();
            }
            if (GUILayout.Button("Add IAPManager"))
            {
                _coreMobile.AddIAPManager();
            }
            if (GUILayout.Button("Add BuildManager"))
            {
                _coreMobile.AddBuildManager();
            }
            if (GUILayout.Button("Add ShephertzManager"))
            {
                _coreMobile.AddShephertzManager();
            }
            if (GUILayout.Button("AddPlayFabManager"))
            {
                _coreMobile.AddPlayFabManager();
            }
            if (GUILayout.Button("Add All MobileThird"))
            {
                _coreMobile.AddAllMobileThird();
            }
            serializedObject.Update();
        }

        private string CheckDefine(string defines, string[] arrayDefine, bool enableDefine, string defineString)
        {
            if (enableDefine && !arrayDefine.Contains(defineString))
            {
                defines += ";" + defineString;
            }
            else if (!enableDefine && arrayDefine.Contains(defineString))
            {
                defines = defines.Replace(";" + defineString, "");
                defines = defines.Replace(defineString + ";", "");
            }

            return defines;
        }
    }
}
