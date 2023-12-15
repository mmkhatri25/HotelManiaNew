using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coinsOrBucks : MonoBehaviour
{
    public Text myText;
    public string myStringPublic;
    public static string myString;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myText!=null)
        {
            myText.text = myString.ToString();
        }
    }

    public void myButton()
    {
        myString = myStringPublic;
    }
}
