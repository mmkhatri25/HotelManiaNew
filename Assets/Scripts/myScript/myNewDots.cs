using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myNewDots : MonoBehaviour
{
    public int myLabel;
    RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myLabel==CataloguePopup.myPageNumber)
        {
            RectTransform.localScale = Vector3.Lerp(RectTransform.localScale,new Vector3(1.75f,1.75f,1.75f),Time.deltaTime*10f);
        }
        else
        {
            RectTransform.localScale = Vector3.Lerp(RectTransform.localScale,new Vector3(1f,1f,1f),Time.deltaTime*10f);
        }
    }
}
