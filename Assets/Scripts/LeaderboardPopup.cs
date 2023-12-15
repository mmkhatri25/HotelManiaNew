using Alg;
//using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPopup : MonoBehaviour
{
	public Button aroundPlayerButton;

	public Button top100Button;

	public Button topFriendsButton;

	public GameObject _top100ButtonActive;

	public GameObject _topFriendsButtonActive;

	public GameObject LeaderBoardEntryPrefab;

	public LeaderboardEntry playerLeaderboardEntry;

	public GameObject focusOnPlayer;

	private LeaderboardType currentLeaderboardType = LeaderboardType.NotSet;

	private List<LeaderboardEntry> top100Entries = new List<LeaderboardEntry>();

	private List<LeaderboardEntry> aroundPlayerEntries = new List<LeaderboardEntry>();

	private List<LeaderboardEntry> friendsEntries = new List<LeaderboardEntry>();

	private List<LeaderboardEntry> currentEntries;

	private int userPosOnListTop100;

	private int userPosOnListAroundPlayer;

	private int userPosOnListFriends;

	public GameObject top100Leaderboard;

	public GameObject aroundPlayerLeaderboard;

	public GameObject friendsLeaderBoard;

	public RectTransform contentTop100RectTransform;

	public RectTransform contentAroundPlayerRectTransform;

	public RectTransform contentFriendsRectTransform;

	private RectTransform currentContent;

	[SerializeField]
	public Button closeButton;

	private static LeaderboardPopup prefab;

	private List<GameObject> leaderboardEntryPool = new List<GameObject>();

	public event Action OnPopupClosed;

	[SerializeField]
	public static LeaderboardPopup GetInstance()
	{
		if (prefab == null)
		{
			prefab = Resources.Load<LeaderboardPopup>("Popups/Popup.Leaderboard");
			Resources.UnloadUnusedAssets();
		}
		return UnityEngine.Object.Instantiate(prefab);
	}

	public void Init(Action restartButtonCallback = null, Action homeButtonCallback = null)
	{
		Singleton<DBConnection_Ctl>.Instance._loadingPopUpRef = LoadingPopup.GetInstance(base.transform).gameObject;
		closeButton.onClick.AddListener(ClosePopup);
		aroundPlayerButton.onClick.AddListener(ShowAroundPlayerFriends);
		top100Button.onClick.AddListener(ShowTop100);
		topFriendsButton.onClick.AddListener(ShowFriends);
		_top100ButtonActive.SetActive(value: true);
		_topFriendsButtonActive.SetActive(value: false);
		currentContent = contentTop100RectTransform;
		currentEntries = top100Entries;
		//PlayfabManager.Instance.GetLeaderboardInfo();
	}

	private void ClosePopup()
	{
		if (this.OnPopupClosed != null)
		{
			this.OnPopupClosed();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ShowTop100()
	{
		HideAllEntries();
		currentEntries = top100Entries;
		currentContent = contentTop100RectTransform;
		_topFriendsButtonActive.SetActive(value: false);
		_top100ButtonActive.SetActive(value: true);
		contentTop100RectTransform.parent.parent.gameObject.SetActive(value: true);
		contentAroundPlayerRectTransform.parent.parent.gameObject.SetActive(value: false);
		contentFriendsRectTransform.parent.parent.gameObject.SetActive(value: false);
		SnapInTop100();
		FadeInAllEntriesInCurrentList();
	}

	public void ShowAroundPlayerFriends()
	{
		HideAllEntries();
		currentEntries = aroundPlayerEntries;
		currentContent = contentAroundPlayerRectTransform;
		_topFriendsButtonActive.SetActive(value: false);
		_top100ButtonActive.SetActive(value: true);
		contentTop100RectTransform.parent.parent.gameObject.SetActive(value: false);
		contentAroundPlayerRectTransform.parent.parent.gameObject.SetActive(value: true);
		contentFriendsRectTransform.parent.parent.gameObject.SetActive(value: false);
		SnapInAroundPlayer();
		FadeInAllEntriesInCurrentList();
	}

	public void ShowFriends()
	{
		HideAllEntries();
		currentEntries = friendsEntries;
		currentContent = contentFriendsRectTransform;
		_topFriendsButtonActive.SetActive(value: true);
		_top100ButtonActive.SetActive(value: false);
		contentTop100RectTransform.parent.parent.gameObject.SetActive(value: false);
		contentAroundPlayerRectTransform.parent.parent.gameObject.SetActive(value: false);
		contentFriendsRectTransform.parent.parent.gameObject.SetActive(value: true);
		SnapInFriends();
		FadeInAllEntriesInCurrentList();
	}

	public IEnumerator CreateLeaderBoards()
	{
		ClearValuesWithScore0();
		yield return StartCoroutine(CreateTop100LeaderBoardEntriesIE());
		yield return StartCoroutine(CreateAroundPlayerLeaderBoardEntriesIE());
		yield return StartCoroutine(CreateFriendLeaderBoardEntriesIE());
		Singleton<DBConnection_Ctl>.Instance.CloseLoadingPopup();
		ShowTop100();
	}

	private void ClearValuesWithScore0()
	{
		
	}

	private void ClearValuesWithScore0FromLeaderboard()
	{
		
	}

	private IEnumerator CreateTop100LeaderBoardEntriesIE()
	{
		if (currentLeaderboardType != 0)
		{
			
			yield return new WaitForSeconds(0.1f);
			currentLeaderboardType = LeaderboardType.Top100;
		}
	}

	private void SnapInTop100()
	{
		//playerLeaderboardEntry.Init(PlayfabManager.Instance.userLeaderboardEntry);
		playerLeaderboardEntry.positionText.text = "#" + playerLeaderboardEntry.positionText.text;
		playerLeaderboardEntry.FadeInEntry();
		playerLeaderboardEntry.transform.parent.gameObject.SetActive(value: true);
		if (userPosOnListTop100 > 100)
		{
			if (true)
			{
				focusOnPlayer.SetActive(value: false);
			}
			SnapTo(currentContent.GetChild(0).GetComponent<RectTransform>(), 0);
		}
		else
		{
			currentContent.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
			SnapTo(currentContent.GetChild(userPosOnListTop100).GetComponent<RectTransform>(), userPosOnListTop100);
		}
	}

	private void CreateFriendLeaderBoardEntries()
	{
		StartCoroutine(CreateFriendLeaderBoardEntriesIE());
	}

	private IEnumerator CreateFriendLeaderBoardEntriesIE()
	{
		if (currentLeaderboardType != LeaderboardType.Friends)
		{
			
			//PopulateLayoutWithEntries(PlayfabManager.Instance.aroundPlayerFriendLeaderboardEntries, usePlayfabPos: false, LeaderboardType.Friends);
			yield return new WaitForSeconds(0.1f);
			currentLeaderboardType = LeaderboardType.Friends;
		}
	}

	private void SnapInFriends()
	{
		currentContent.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		SnapTo(currentContent.GetChild(userPosOnListFriends).GetComponent<RectTransform>(), userPosOnListFriends);
	}

	private IEnumerator CreateAroundPlayerLeaderBoardEntriesIE()
	{
		if (currentLeaderboardType != LeaderboardType.AroundPlayer)
		{
			//PopulateLayoutWithEntries(PlayfabManager.Instance.aroundPlayerLeaderboardEntries, usePlayfabPos: true, LeaderboardType.AroundPlayer);
			yield return new WaitForSeconds(0.1f);
			currentLeaderboardType = LeaderboardType.AroundPlayer;
		}
	}

	private void SnapInAroundPlayer()
	{
		currentContent.parent.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
		SnapTo(currentContent.GetChild(userPosOnListAroundPlayer).GetComponent<RectTransform>(), userPosOnListAroundPlayer);
	}

	private void PopulateLayoutWithEntries( bool usePlayfabPos, LeaderboardType leaderboardType)
	{
		switch (leaderboardType)
		{
		case LeaderboardType.Top100:
			currentContent = contentTop100RectTransform;
			currentEntries = top100Entries;
			break;
		case LeaderboardType.AroundPlayer:
			currentContent = contentAroundPlayerRectTransform;
			currentEntries = aroundPlayerEntries;
			break;
		case LeaderboardType.Friends:
			currentContent = contentFriendsRectTransform;
			currentEntries = friendsEntries;
			break;
		}
		
	}

	public void SnapTo(RectTransform target, int index)
	{
		Canvas.ForceUpdateCanvases();
		float num = RectTransformUtility.PixelAdjustRect(currentContent.parent.GetComponent<RectTransform>(), currentContent.parent.parent.parent.parent.GetComponent<Canvas>()).height / target.sizeDelta.y;
		float num2 = index;
		if (num2 > num / 2f)
		{
			float y = (num2 - num / 2f) * 150f + 75f;
			currentContent.anchoredPosition = new Vector2(currentContent.anchoredPosition.x, y);
			Canvas.ForceUpdateCanvases();
		}
	}

	private void FadeInAllEntriesInCurrentList()
	{
		for (int i = 0; i < currentEntries.Count; i++)
		{
			currentEntries[i].FadeInEntry();
		}
	}

	private void HideAllEntries()
	{
		for (int num = top100Entries.Count - 1; num >= 0; num--)
		{
			top100Entries[num].HideEntry();
		}
		for (int num2 = aroundPlayerEntries.Count - 1; num2 >= 0; num2--)
		{
			aroundPlayerEntries[num2].HideEntry();
		}
		for (int num3 = friendsEntries.Count - 1; num3 >= 0; num3--)
		{
			friendsEntries[num3].HideEntry();
		}
	}

	private float Transtale01RangeToMinus11Range(float value)
	{
		return value + (1f - value) * -1f;
	}
}
