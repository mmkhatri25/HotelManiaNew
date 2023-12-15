using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CataloguePopup : MonoBehaviour
{
	public static CataloguePopup Instance;

	public Image fadeImage;

	public Text coinsText;

	public Text hardCurrencyText;

	public Text unlockedRooms;

	public Text totalRooms;

	public GameObject floorButtonPrefab;

	public Transform viewportContent;

	public Button addHardCurrencyButton;

	public Button backButton;

	public Button roomsButton;

	public GameObject roomsActiveTab;

	public Button boostersButton;

	public GameObject boostersActiveTab;

	public GameObject floorCatalogue;

	public GameObject boosterCatalogue;

	public GameObject dot;

	[SerializeField]
	public Transform dotContainer;

	public Sprite[] dotsRenderers ;//= new List<Image>();

	[SerializeField]
	private static CataloguePopup prefab;

	[SerializeField]
	private Transform buttonContainer;

	public List<FloorSO> orderedFloorList = new List<FloorSO>();

	public int currentListIndex;

	public GameObject leftButton;

	public GameObject rightButton;

	[Header("Info Panel")]
	public GameObject infoPanel;

	public Text floorName;

	public TextMeshProUGUI floorDesc;

	public Image floorImage;

	public Image floorChar1;

	public Image floorChar2;

	public Image floorChar3;

	public Button infoPanelClose;

	public Button GoToGachaButton;
	public GameManager GameManager;

	public int myLabelValue;
	public static int myPageNumber;


	public Image[] ImagesofCatlogue;
    public Text[] Name_Lables;
	public CatalogueFloor[] FloorList;
    public GameObject[] sections;
    public int currentSectionIndex = 0;

	public GameObject MainPanelMove, nextButton, prevButton;



    public static Action setZeroPage;

	public int myPublicPage;

	public static CataloguePopup GetInstance(Transform parent)
	{
		if (prefab == null)
		{
			prefab = Resources.Load<CataloguePopup>("Popups/Screen.Catalogue2");
			Resources.UnloadUnusedAssets();
		}
		return Instance = UnityEngine.Object.Instantiate(prefab, parent);
	}

	public void Init(int tabIndex)
	{


		setZeroPage = MoveAtZero;
		GameManager = GameObject.Find("AppManagers").GetComponent<GameManager>();
		coinsText.text = PlayerDataManager.Coins.ToString();
		hardCurrencyText.text = PlayerDataManager.HardCurrency.ToString();
		PlayerDataManager.OnHardCurrencyChanged += HardCurrencyChanged;
		addHardCurrencyButton.onClick.AddListener(delegate
		{
			ShopPopup.GetInstance(base.transform);
		});
		backButton.onClick.AddListener(ClosePopupPressed);
		orderedFloorList = new List<FloorSO>(GameManager.Instance.lockedFloors);
		//orderedFloorList.AddRange(GameManager.Instance.unlockedFloors);
		infoPanelClose.onClick.AddListener(HideInfoPanel);
		totalRooms.text = string.Concat("/" + (GameManager.Instance.unlockedFloors.Count + GameManager.Instance.lockedFloors.Count));
		unlockedRooms.text = GameManager.Instance.unlockedFloors.Count.ToString();
		boostersButton.transform.gameObject.SetActive(value: false);
		GoToGachaButton.onClick.AddListener(GachaButtonClicked);
		if (tabIndex == 0)
		{
			OpenFloorCatalogueTab();
		}
		else
		{
			OpenBoostersCatalogueTab();
		}

		dotsRenderers = new Sprite[orderedFloorList.Count];

		for(int i =0; i<orderedFloorList.Count ; i++ )
		{
			dotsRenderers[i] = orderedFloorList[i].floorSprite;
		}
		myPageNumber=0;
		//PlayerDataManager.AddHardCurrency(1000);
        //PlayerDataManager.AddCoins(1000);

	

		for (int k = 0; k < FloorList.Length-1; k++)
		{
			FloorList[k].floorSO = orderedFloorList[k];
          
            
            if (PlayerPrefs.GetInt(FloorList[k].floorSO.name) == 1)/*FloorList[k].floorSO.isBought*/
            {
				FloorList[k].GetComponent<CatalogueFloor>().buyButton.gameObject.SetActive(false);
				FloorList[k].GetComponent<CatalogueFloor>().lockImage.gameObject.SetActive(false);
            }
        }

        for (int j = 0; j < Name_Lables.Length; j++)
		{
            if (Name_Lables[j].gameObject.activeInHierarchy)
            {
                Name_Lables[j].text = orderedFloorList[j].displayName;

            }
        }


		for (int i = 0; i < ImagesofCatlogue.Length; i++)
		{
			if (ImagesofCatlogue[i].gameObject.activeInHierarchy)
			{
                ImagesofCatlogue[i].sprite = dotsRenderers[i];

            }
			
        }

        ShowCurrentSection();


    }

    public void OnRightButtonClicked()
    {
		//if (currentSectionIndex < sections.Length - 1)
		//{
		currentSectionIndex++;
		ShowCurrentSection();
        //}
    }

    public void OnLeftButtonClicked()
    {
		//if (currentSectionIndex > 0)
		//{
		currentSectionIndex--;
		ShowCurrentSection();
        //}
    }

    void ShowCurrentSection()
    {
		//switch (currentSectionIndex)
		//{
		//	case 0:
		//		MainPanelMove.transform.DOLocalMoveX(1770f, 0.5f);
		//		prevButton.SetActive(false);
		//              nextButton.SetActive(true);
		//              break;
		//          case 1:
		//              MainPanelMove.transform.DOMoveX(1350f, 0.5f);
		//              prevButton.SetActive(true);
		//              nextButton.SetActive(true);
		//              break;
		//          case 2:
		//              MainPanelMove.transform.DOMoveX(690f, 0.5f);
		//              prevButton.SetActive(true);
		//              nextButton.SetActive(true);
		//              break;
		//          case 3:
		//              MainPanelMove.transform.DOMoveX(20f, 0.5f);
		//              prevButton.SetActive(true);
		//              nextButton.SetActive(true);
		//              break;
		//          case 4:
		//              MainPanelMove.transform.DOMoveX(-645f, 0.5f);
		//              prevButton.SetActive(true);
		//              nextButton.SetActive(true);
		//              break;
		//          case 5:
		//              MainPanelMove.transform.DOMoveX(-1315f, 0.5f);
		//              prevButton.SetActive(true);
		//		nextButton.SetActive(false);
		//              break;
		//          default:
		//              MainPanelMove.transform.DOLocalMoveX(1770f, 0.5f);

		//              break;
		//}

		for (int i = 0; i < sections.Length; i++)
		{
			if (i == currentSectionIndex)
			{
				sections[i].SetActive(true);
			}
			else
			{
				sections[i].SetActive(false);
			}
		}
		if (currentSectionIndex <= 0)
		{
			prevButton.SetActive(false);
			print("currentListIndex " + currentSectionIndex);
		}
		else
		{
            prevButton.SetActive(true);
            print("currentListIndex "+ currentSectionIndex);


        }
        if (currentSectionIndex >= 5)

        {
            nextButton.SetActive(false);
            print("currentListIndex "+ currentSectionIndex);

        }

        else
        {
            nextButton.SetActive(true);
            print("currentListIndex "+ currentSectionIndex);


        }
    }

    void Update()
	{
		//return;
		//label_one.sprite= dotsRenderers[0 + myLabelValue];
	 //   label_two.sprite = dotsRenderers[1 + myLabelValue];
	 //   label_three.sprite = dotsRenderers[2 + myLabelValue];

		//if((3 + myLabelValue) >= 0 && (3 + myLabelValue) < dotsRenderers.Length)
		//{
  //          label_four.sprite = dotsRenderers[3 + myLabelValue];
  //          text_four.text = orderedFloorList[3 + myLabelValue].displayName;
  //          ListItemRoom4.floorSO = orderedFloorList[3 + myLabelValue];

  //      }
  //      else
		//{
  //          ListItemRoom4.gameObject.SetActive(false);

  //      }

  //      text_one.text = orderedFloorList[0 + myLabelValue].displayName;
		//text_two.text = orderedFloorList[1 + myLabelValue].displayName;
		//text_three.text = orderedFloorList[2 + myLabelValue].displayName;

        //ListItemRoom3.floorSO = orderedFloorList[2 + myLabelValue];
        //print("myLabelValue - "+ myLabelValue);
		//if (myLabelValue <22)
		//{
  //          ListItemRoom1.gameObject.SetActive(true);
  //          ListItemRoom2.gameObject.SetActive(true);
  //          print("inside myLabelValue - " + myLabelValue);
           

  //          ListItemRoom1.floorSO = orderedFloorList[0 + myLabelValue];
  //          ListItemRoom2.floorSO = orderedFloorList[1 + myLabelValue];
  //      }
		//else
		//{
		//	ListItemRoom1.gameObject.SetActive(false);
  //          ListItemRoom2.gameObject.SetActive(false);
  //      }
		

		//if (myLabelValue+5 <= orderedFloorList.Count)
		//{
		//	rightButton.SetActive(true);
		//}
		//else
		//{
		//	rightButton.SetActive(false);
		//	myPageNumber = dotsControl.lastPage;
		//}

		
		//if(myLabelValue>0)
		//{
		//	leftButton.SetActive(true);
		//}
		//else
		//{
		//	leftButton.SetActive(false);
		//	myPageNumber = 0;
		//}

		myPublicPage = myPageNumber;
		
	}

	private void HardCurrencyChanged()
	{
		hardCurrencyText.text = PlayerDataManager.HardCurrency.ToString();
	}

	public void OpenFloorCatalogueTab()
	{
		SetupCatalogueList();
		roomsActiveTab.SetActive(value: true);
		roomsButton.transform.gameObject.SetActive(value: false);
		floorCatalogue.SetActive(value: true);
	}

	public void OpenBoostersCatalogueTab()
	{
		UnityEngine.Debug.Log("OpenBoostersCatalogueTab");
		roomsActiveTab.SetActive(value: false);
		roomsButton.transform.gameObject.SetActive(value: true);
		floorCatalogue.SetActive(value: false);
	}

	public void SetupCatalogueList()
	{
		for (int i = 0; i < GameManager.Instance.lockedFloors.Count; i++)
		{
		//]]	Object.Instantiate(floorButtonPrefab, viewportContent).GetComponent<CatalogueFloor>().SetUp(GameManager.Instance.lockedFloors[i]);
		}
		for (int j = 0; j < GameManager.Instance.unlockedFloors.Count; j++)
		{
		//	Object.Instantiate(floorButtonPrefab, viewportContent).GetComponent<CatalogueFloor>().SetUp(GameManager.Instance.unlockedFloors[j]);
		}
	}

	public void ClosePopupPressed()
	{
		ClosePopup();
	}

	public void ClosePopup()
	{
		PlayerDataManager.OnHardCurrencyChanged -= HardCurrencyChanged;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void GachaButtonClicked()
	{
		fadeImage.gameObject.SetActive(value: true);
		fadeImage.DOFade(1f, 0.7f).OnComplete(delegate
		{
			GameManager.Instance.ChangeScene("GachaScene");
		});
	}

	public void ShowInfoPanel(FloorSO floorSO)
	{
		floorName.text = floorSO.displayName;
		floorDesc.text = floorSO.displayDesc;
		floorImage.sprite = floorSO.floorSprite;
		floorChar1.sprite = floorSO.char1;
		floorChar2.sprite = floorSO.char2;
		floorChar3.sprite = floorSO.char3;
		infoPanel.SetActive(value: true);
	}

	public void HideInfoPanel()
	{
		infoPanel.SetActive(value: false);
	}

	public void MoveRight()
	{
		if(myLabelValue+9 <= orderedFloorList.Count)
		{
			myLabelValue+=4;
			myPageNumber+=1;
		}
		else
		{
			myLabelValue = orderedFloorList.Count - 4;
		}
		
	}

	public void MoveLeft()
	{
		if(myLabelValue >= 22)
		{
            myLabelValue = 0;
            CataloguePopup.setZeroPage?.Invoke();
			return;
		}
		else
		{
            if (myLabelValue - 4 >= 0)
            {
                myLabelValue -= 4;
                myPageNumber -= 1;
            }
            else
            {
                myLabelValue = 0;
            }
        }
		
		
	}
	public void MoveAtZero()
	{
		myLabelValue = 0;
	}

}
