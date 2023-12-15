using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterCorrection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData playerData = PlayerDataManager.GetPlayerData();
        if(playerData.booster1Amount<=0)
        {
            PlayerDataManager.SetBooster1(playerData.booster1Amount+=1);
        }

        if(playerData.booster2Amount<=0)
        {
            PlayerDataManager.SetBooster2(playerData.booster1Amount+=1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
