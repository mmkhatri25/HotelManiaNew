using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPage : MonoBehaviour
{
    public GameObject page;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnableButton()
    {
        page.SetActive(true);
    }

    public void OnDisableButton()
    {
        page.SetActive(false);
    }
}
