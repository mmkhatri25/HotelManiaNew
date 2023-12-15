using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagDetectorGen : MonoBehaviour
{
    public GameObject LagDetector;
    // Start is called before the first frame update
    private void Start()
    {
        // Find the GameObject with the specified tag
        GameObject targetObject = GameObject.FindWithTag("LagDetector");

        if (targetObject != null)
        {
            // Do something with the found object
            Debug.Log("Found GameObject with tag: " + targetObject.name);
        }
        else
        {
            Instantiate(LagDetector,transform.position,transform.rotation);
            // No GameObject found with the specified tag
            Debug.Log("No GameObject found with tag: LagDetector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
