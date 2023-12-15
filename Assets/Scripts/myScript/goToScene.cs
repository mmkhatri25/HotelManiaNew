using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goToScene : MonoBehaviour
{
    public string scenename;
    public bool Auto;
    // Start is called before the first frame update
    void Start()
    {
        if(Auto)
        {
            StartCoroutine(jump());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goTo()
    {
        StartCoroutine(jump());
    }

    IEnumerator jump()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scenename);
    }
}
