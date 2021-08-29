using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wikiBehavior : MonoBehaviour
{
	public void buildWiki(int setIndex = 0)
	{
		this.clearWikiLinks();
		float setX = 45f;
		float num = -58f;
		this.wikiList = new Dictionary<string, string>();
		this.wikiLinks = new List<GameObject>();
		this.wikiIndex = setIndex;
		this.wikiList = this.myCloud.getWikiList(setIndex);
		if (this.wikiIndex == 0)
		{
			this.wikiTitle.text = "The Deep Wiki I";
		}
		else if (this.wikiIndex == 1)
		{
			this.wikiTitle.text = "The Deep Wiki II";
		}
		else if (this.wikiIndex == 2)
		{
			this.wikiTitle.text = "The Deep Wiki III";
		}
		foreach (KeyValuePair<string, string> keyValuePair in this.wikiList)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.wikiLinkObject);
			gameObject.transform.SetParent(base.transform);
			gameObject.GetComponent<WikiLink>().buildMe(this.myUIManager, this.myAB, this, keyValuePair.Key, keyValuePair.Value, setX, num);
			this.wikiLinks.Add(gameObject);
			num = num - gameObject.GetComponent<RectTransform>().sizeDelta.y - 10f;
		}
		base.GetComponent<RectTransform>().sizeDelta = new Vector2(base.GetComponent<RectTransform>().sizeDelta.x, Mathf.Abs(num));
	}

	public void clearWikiLinks()
	{
		if (this.wikiLinks != null)
		{
			for (int i = 0; i < this.wikiLinks.Count; i++)
			{
				Object.Destroy(this.wikiLinks[i]);
			}
			this.wikiLinks.Clear();
		}
	}

	public TheCloud myCloud;

	public annBehavior myAB;

	public GameObject wikiLinkObject;

	public UIManager myUIManager;

	public int wikiIndex;

	public Text wikiTitle;

	private Dictionary<string, string> wikiList;

	private List<GameObject> wikiLinks;
}
