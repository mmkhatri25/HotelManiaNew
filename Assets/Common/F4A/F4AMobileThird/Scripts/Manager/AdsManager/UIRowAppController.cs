using UnityEngine;
using UnityEngine.UI;

namespace com.F4A.MobileThird
{
    public class UIRowAppController : MonoBehaviour
    {
        public event System.Action OnDownload = delegate { };
        [SerializeField]
        private RawImage backgroundApp = null;
        [SerializeField]
        private RawImage iconApp = null;
        [SerializeField]
	    private Text textNameApp = null, textDescriptionApp = null;
	    [SerializeField]
	    private Button btnDownload = null;
	    
	   
        
	 
    }
}