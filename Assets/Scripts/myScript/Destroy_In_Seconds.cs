using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_In_Seconds : MonoBehaviour
{
    public float myTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,myTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
