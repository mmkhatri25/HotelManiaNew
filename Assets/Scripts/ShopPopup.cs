using com.F4A.MobileThird;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
	public Button pack1Button;

	public Button pack2Button;

	public Button pack3Button;

	public Button pack4Button;

	public Button pack5Button;

	public Button closeButton;

	public Text pack1Title;

	public Text pack2Title;

	public Text pack3Title;

	public Text pack4Title;

	public Text pack5Title;

	public Text pack1Desc;

	public Text pack2Desc;

	public Text pack3Desc;

	public Text pack4Desc;

	public Text pack5Desc;

	public Text item1Price;

	public Text item2Price;

	public Text item3Price;

	public Text item4Price;

	public Text item5Price;

	public Text item1HardCurrencyValue;

	public Text item2HardCurrencyValue;

	public Text item3HardCurrencyValue;

	public Text item4HardCurrencyValue;

	public Text item5HardCurrencyValue;

	[SerializeField]
	private static ShopPopup prefab;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private AudioSource menuOutAudioSource;

	private List<string> packageIds = new List<string>();

	public static ShopPopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<ShopPopup>("Popups/Popup.Shop");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab, parent);
	}

	private void Start()
	{
		pack1Button.onClick.AddListener(BuyPack1Pressed);
		pack2Button.onClick.AddListener(BuyPack2Pressed);
		pack3Button.onClick.AddListener(BuyPack3Pressed);
		pack4Button.onClick.AddListener(BuyPack4Pressed);
		pack5Button.onClick.AddListener(BuyPack5Pressed);
		closeButton.onClick.AddListener(ClosePopupButton);


		pack1Title.text = "Tiny bag of Bucks";
		pack1Desc.text = "";

		pack2Title.text = "Small bag of Bucks";
		pack2Desc.text = "";

		pack3Title.text = "Medium bag of Bucks";
		pack3Desc.text = "";

		pack4Title.text = "Large bag of Bucks";
		pack4Desc.text = "";

		pack5Title.text = "Extra large bag of Bucks";
		pack5Desc.text = "";

		List<Text> listDes = new List<Text>();
		listDes.Add(pack1Desc);
		listDes.Add(pack2Desc);
		listDes.Add(pack3Desc);
		listDes.Add(pack4Desc);
        listDes.Add(pack5Desc);

        List<Text> listPrice = new List<Text>();
		listPrice.Add(item2Price);
		listPrice.Add(item2Price);
		listPrice.Add(item3Price);
		listPrice.Add(item4Price);
        listPrice.Add(item5Price);

        List<Text> listCurrency = new List<Text>();
		listCurrency.Add(item1HardCurrencyValue);
		listCurrency.Add(item2HardCurrencyValue);
		listCurrency.Add(item3HardCurrencyValue);
		listCurrency.Add(item4HardCurrencyValue);
        listCurrency.Add(item5HardCurrencyValue);

		packageIds.Clear();
		var products = IAPManager.Instance.GetAllProductInfo();
		int counter = 0;
        foreach (var product in products)
        {
			if(product.IsConsumable())
            {
				listCurrency[counter].text = product.Coin.ToString();
				listPrice[counter].text = Purchase_Ctl.Instance.GetPriceString(product.Id);
				packageIds.Add(product.Id);

				counter++;
				if (counter >= 5) break;
			}
        }
	}

	public void BuyPack1Pressed()
	{
		Purchase_Ctl.Instance.BuyConsumable(packageIds[0]);
	}

	public void BuyPack2Pressed()
	{
		Purchase_Ctl.Instance.BuyConsumable(packageIds[1]);
	}

	public void BuyPack3Pressed()
	{
		Purchase_Ctl.Instance.BuyConsumable(packageIds[2]);
	}

	public void BuyPack4Pressed()
	{
		Purchase_Ctl.Instance.BuyConsumable(packageIds[3]);
	}

	public void BuyPack5Pressed()
	{
		Purchase_Ctl.Instance.BuyConsumable(packageIds[4]);
	}

	private void ClosePopupButton()
	{
		menuOutAudioSource.Play();
		_animator.SetTrigger("Out");
	}

	public void ClosePopup()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
