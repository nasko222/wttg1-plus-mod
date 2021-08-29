using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WikiLink : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IEventSystemHandler
{
	public void buildMe(UIManager setUIM, annBehavior setAB, wikiBehavior setWB, string setTitle, string setURL, float setX, float setY)
	{
		this.myUIManager = setUIM;
		this.myAB = setAB;
		this.myWB = setWB;
		this.siteTitle.text = setTitle;
		this.myURL = "http://" + setURL + ".ann";
		this.myX = setX;
		this.myY = setY;
		if (GameManager.GetTheCloud().wasThisSiteVisted(this.myURL))
		{
			this.defaultColor = this.visitedColor;
			this.siteTitle.color = this.visitedColor;
		}
		base.transform.localPosition = new Vector3(this.myX, this.myY, 0f);
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		float num = LayoutUtility.GetPreferredWidth(this.siteTitle.rectTransform) + this.siteTitle.rectTransform.localPosition.x;
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(num, base.GetComponent<RectTransform>().sizeDelta.y);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		this.siteTitle.color = this.hoverColor;
		this.myUIManager.setHoverCursor();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		this.siteTitle.color = this.defaultColor;
		this.myUIManager.setDefaultCursor();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.myUIManager.setDefaultCursor();
		if (GameManager.GetTheCloud().CheckIfRealSite(this.myURL))
		{
			this.myAB.forceLoadURL(this.myURL, true);
			this.myWB.clearWikiLinks();
		}
		else
		{
			int num = Random.Range(0, 10);
			if (num == 2)
			{
				GameManager.GetTheHackerManager().launchHack();
			}
			else
			{
				this.myAB.forceLoadURL(this.myURL, true);
				this.myWB.clearWikiLinks();
			}
		}
	}

	public UIManager myUIManager;

	public annBehavior myAB;

	public wikiBehavior myWB;

	public Text siteTitle;

	public float myX;

	public float myY;

	public Color defaultColor;

	public Color hoverColor;

	public Color visitedColor;

	private string myURL;
}
