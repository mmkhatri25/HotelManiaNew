using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotsControl : MonoBehaviour
{
    public CataloguePopup CataloguePopup;
    public GameObject[] myDots;
    public int Length;
    public static int lastPage;
    // Start is called before the first frame update
    void Start()
    {
        Length = CataloguePopup.dotsRenderers.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(CataloguePopup.dotsRenderers.Length>28)
        {
            lastPage = 7;
            myDots[7].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=28 && CataloguePopup.dotsRenderers.Length>24)
        {
            lastPage = 6;
            myDots[6].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=24 && CataloguePopup.dotsRenderers.Length>20)
        {
            lastPage = 5;
            myDots[5].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=20 && CataloguePopup.dotsRenderers.Length>16)
        {
            lastPage = 4;
            myDots[4].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=16 && CataloguePopup.dotsRenderers.Length>12)
        {
            lastPage = 3;
            myDots[3].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=12 && CataloguePopup.dotsRenderers.Length>8)
        {
            lastPage = 2;
            myDots[2].SetActive(true);
            myDots[0].SetActive(false);
            myDots[1].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=8 && CataloguePopup.dotsRenderers.Length>4)
        {
            lastPage = 1;
            myDots[1].SetActive(true);
            myDots[0].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
        else if(CataloguePopup.dotsRenderers.Length<=4)
        {
            lastPage = 0;
            myDots[0].SetActive(true);
            myDots[1].SetActive(false);
            myDots[2].SetActive(false);
            myDots[3].SetActive(false);
            myDots[4].SetActive(false);
            myDots[5].SetActive(false);
            myDots[6].SetActive(false);
            myDots[7].SetActive(false);

        }
    }
}
