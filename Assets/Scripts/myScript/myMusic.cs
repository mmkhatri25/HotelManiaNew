using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myMusic : MonoBehaviour
{
    AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("myMusic")==0)
		{
			aud.enabled = true;
		}
		else
		{
			aud.enabled = false;
		}
    }
}
