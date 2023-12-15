using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMultipleObjectWithSameName : MonoBehaviour
{
    public string myName;
    public GameObject[] children;
    public GameObject myPan;
    public bool checking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myPan.activeInHierarchy)
        {
            children = GameObject.FindGameObjectsWithTag(myName);
        for(int i = 0; i<children.Length; i++)
        {
            
                if(i>0)
                {
                    AdForMissionPopup  AdForMissionPopup = children[i].GetComponent<AdForMissionPopup>();
                    AdForMissionPopup.ClickedClose();
                }
            
        }

        checking = true;
        }
        else
        {
            checking = false;
        }
    }
}
