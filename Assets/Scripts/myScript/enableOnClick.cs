using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableOnClick : MonoBehaviour
{
    public GameObject myObject;
    // Start is called before the first frame update
    public void myOnClickOn()
    {
        myObject.SetActive(true);
    }

    public void myOnClickOff()
    {
        myObject.SetActive(false);
    }
}
