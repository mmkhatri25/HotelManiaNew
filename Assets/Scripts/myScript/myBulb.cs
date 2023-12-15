using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBulb : MonoBehaviour
{
    public LoadShading LoadShading;
    public GameObject Bulb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        LoadShading.bulbHit();
        Bulb.SetActive(false);
    }
}
