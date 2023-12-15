using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableOnLoad : MonoBehaviour
{
    public GameObject myObject;
    public float myTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(myEnable());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator myEnable()
    {
        yield return new WaitForSeconds(myTime);
        myObject.SetActive(true);
    }
}
