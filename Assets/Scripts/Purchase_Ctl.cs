using com.F4A.MobileThird;
using System;
using UnityEngine;

public class Purchase_Ctl : MonoBehaviour
{
	public static Purchase_Ctl Instance;

	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

    private void OnEnable()
    {
        IAPManager.OnBuyPurchaseSuccessed += IAPManager_OnBuyPurchaseSuccessed;
        IAPManager.OnBuyPurchaseFailed += IAPManager_OnBuyPurchaseFailed;
    }

    private void OnDisable()
    {
        IAPManager.OnBuyPurchaseSuccessed -= IAPManager_OnBuyPurchaseSuccessed;
        IAPManager.OnBuyPurchaseFailed -= IAPManager_OnBuyPurchaseFailed;
    }

    private bool IsInitialized()
	{
		return IAPManager.Instance.IsInitialized();
	}

	public void BuyConsumable(string consumableID)
	{
		BuyProductID(consumableID);
	}

	public void BuyNonConsumable()
	{
	}

	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			IAPManager.Instance.BuyProductByID(productId);
		}
		else
		{
			UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
        else
        {
			IAPManager.Instance.RestorePurchases();
        }
	}

    private void IAPManager_OnBuyPurchaseFailed(string id, string error)
    {
    }

    private void IAPManager_OnBuyPurchaseSuccessed(string id, bool modeTest, string receipt)
    {
		var product = IAPManager.Instance.GetProductInfoById(id);
		if(product != null)
        {
			if(product.IsConsumable())
            {
				PlayerDataManager.AddHardCurrency(product.Coin);
            }
			else if(product.IsNonConsumable())
            {

            }
        }
    }

	public string GetPriceString(string id)
    {
		return IAPManager.Instance.GetProductPriceStringById(id);
    }
}
