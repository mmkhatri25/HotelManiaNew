using UnityEngine;
using UnityEngine.UI;

public class CatalogueFloor : MonoBehaviour
{
	public FloorSO floorSO;

	public Text roomNameText;

	public Text priceText;

	public Image floorArt;

	public GameObject lockedContainer;

	public GameObject textUnlockFor;

	public Button buyButton;

	public Button infoButton;

	public Image lockImage;

	public Image BoughtImage;
	public GameObject youBoughtItemPan;

	public int TotalMoney;
	public int price;

	private void Start()
	{
		infoButton.onClick.AddListener(FloorInfoButon);
       // print("CatalogueFloor - " + this.gameObject.name + ", SetUp floor - " + floorSO.displayName);

    }

    void Update()
	{
		price = GameManager.Instance.gameVars.floorHardCurrencyPrice;
		TotalMoney = PlayerDataManager.HardCurrency;
	}

	public void SetUp(FloorSO floorSO =null)
	{
		//this.floorSO = floorSO;
		base.gameObject.SetActive(value: true);
		roomNameText.text = floorSO.displayName;
		floorArt.sprite = floorSO.floorSprite;
		priceText.text = GameManager.Instance.gameVars.floorHardCurrencyPrice.ToString();
		infoButton.gameObject.SetActive(value: true);
		
        print("CatalogueFloor - "+ this.gameObject.name + ", SetUp floor - " + floorSO.displayName);

        SetupBuyButton();
	}

	public void BuyFloor()
	{
		//PlayerDataManager.AddHardCurrency(4000);

        if (PlayerDataManager.HardCurrency >= 500)//GameManager.Instance.gameVars.floorHardCurrencyPrice)
		{
			PlayerDataManager.UnlockFloor(floorSO);

			floorSO.isBought = true;
            PlayerPrefs.SetInt(floorSO.name, 1);

            PlayerDataManager.DeductHardCurrency(500);//GameManager.Instance.gameVars.floorHardCurrencyPrice);
			SetupBuyButton();
			CataloguePopup.Instance.Init(0);
			BoughtImage.sprite = floorArt.sprite;
			buyButton.gameObject.SetActive(false);
			lockImage.gameObject.SetActive(false);

            youBoughtItemPan.SetActive(true);
			//CataloguePopup.setZeroPage?.Invoke();

		}
		else
		{
			ShopPopup.GetInstance(CataloguePopup.Instance.transform);
		}
	}

	public void SetupBuyButton()
	{
		buyButton.onClick.RemoveListener(BuyFloor);
		if (PlayerDataManager.IsUnlocked(floorSO))
		{
			//lockedContainer.SetActive(value: false);
			infoButton.onClick.AddListener(FloorInfoButon);
			//textUnlockFor.SetActive(value: false);
		}
		else if (floorSO.isSecret)
		{
			//lockedContainer.SetActive(value: false);
			//textUnlockFor.SetActive(value: false);
			floorArt.color = Color.black;
			roomNameText.text = "Secret";
			infoButton.gameObject.SetActive(value: false);
		}
		else
		{
			lockedContainer.SetActive(value: true);
		}
		buyButton.onClick.AddListener(BuyFloor);
		
	}

	private void FloorInfoButon()
	{
		CataloguePopup.Instance.ShowInfoPanel(floorSO);
	}

	public void Disable()
	{
		base.gameObject.SetActive(value: false);
	}
}
