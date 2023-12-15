using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private TextMeshProUGUI pTextMeshPro;

	public void Start()
	{
		pTextMeshPro = GetComponent<TextMeshProUGUI>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, UnityEngine.Input.mousePosition, null);
		if (num == -1)
		{
			return;
		}
		TMP_LinkInfo tMP_LinkInfo = pTextMeshPro.textInfo.linkInfo[num];
		string linkID = tMP_LinkInfo.GetLinkID();
		if (!(linkID == "zentris"))
		{
			if (!(linkID == "juice"))
			{
				if (linkID == "dream")
				{
					Application.OpenURL("https://play.google.com/store/apps/details?id=ppl.unity.dontfall");
				}
			}
			else
			{
				Application.OpenURL("https://play.google.com/store/apps/details?id=ppl.unity.JuiceCubesBeta");
			}
		}
		else
		{
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.ppl.isoblocks");
		}
	}
}
