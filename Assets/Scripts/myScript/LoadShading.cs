using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadShading : MonoBehaviour
{
    public float myTime = 40f;
    public int point = 0;
    public GameObject[] shade;
    public GameObject[] bulb;
    public int currentShade;
    public int currentBulb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myTime > 0)
        {
            myTime -= Time.deltaTime;
        }
        else
        {
            if (!shade[0].activeInHierarchy && !shade[1].activeInHierarchy && !shade[2].activeInHierarchy && !shade[3].activeInHierarchy) 
            {
                GenShade();
            }
        }

        if(shade[0].activeInHierarchy || shade[1].activeInHierarchy || shade[2].activeInHierarchy || shade[3].activeInHierarchy)
        {
            if(!bulb[0].activeInHierarchy && !bulb[1].activeInHierarchy && !bulb[2].activeInHierarchy && !bulb[3].activeInHierarchy) 
            {
                GenBulb();
            }
        }
    }

    public void bulbHit()
    {
        point+=1;

        if(point >= 3)
        {
            point = 0;
            myTime = 40;
            shade[currentShade].SetActive(false);
        }
        else
        {
           // GenBulb();
        }
    }

    public void GenBulb()
    {
        currentBulb = Random.Range(0, 4);
        bulb[currentBulb].SetActive(true);
    }

    public void GenShade()
    {
        currentShade = Random.Range(0, 4);
        shade[currentShade].SetActive(true);
        GenBulb();
    }
}
