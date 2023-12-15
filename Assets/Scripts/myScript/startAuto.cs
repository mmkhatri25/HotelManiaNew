using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startAuto : MonoBehaviour
{
    public string scenename;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(scenename);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
