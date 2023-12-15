using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boosterBuyPan : MonoBehaviour
{
    public Text booster1;
    public Text booster2;
    public int coin;
    public CatalogueFloor CatalogueFloor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		PlayerData playerData = PlayerDataManager.GetPlayerData();
		booster1.text = playerData.booster1Amount.ToString();
		//playerData = PlayerDataManager.GetPlayerData();
		booster2.text = playerData.booster2Amount.ToString();
        coin=PlayerDataManager.Coins;
    }

    public void addBoosterButton1()
    {
        if(coin>=250)
        {
            PlayerData playerData = PlayerDataManager.GetPlayerData();
            PlayerDataManager.SetBooster1(playerData.booster1Amount+=1);
            PlayerDataManager.DeductCoins(250);
        }
        else
        {
            ShopPopup.GetInstance(CataloguePopup.Instance.transform.parent);
        }
        
    }

    public void addBoosterButton2()
    {
        if(coin>=250)
        {
            PlayerData playerData = PlayerDataManager.GetPlayerData();
            PlayerDataManager.SetBooster2(playerData.booster2Amount+=1);
            PlayerDataManager.DeductCoins(250);
        }
        else
        {
            ShopPopup.GetInstance(CataloguePopup.Instance.transform.parent);
        }
        
    }
}
