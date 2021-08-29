using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class annBehavior : MonoBehaviour
{
	public void goBack()
	{
		if (this.currentWebsite != null)
		{
			this.forwardList.Add(this.currentWebsiteURL);
		}
		int index = this.backList.Count - 1;
		this.forceLoadURL(this.backList[index], true);
		this.backList.RemoveAt(index);
	}

	public void goForward()
	{
		int index = this.forwardList.Count - 1;
		this.forceLoadURL(this.forwardList[index], true);
	}

	public void forceLoadURL(string theURL, bool addToBackList = false)
	{
		if (this.loadingURL)
		{
			this.loadingURL = false;
			this.aniLoadingPageStop();
		}
		this.urlField.text = theURL;
		this.loadURL(theURL, addToBackList);
	}

	public void closeMe()
	{
		if (this.loadingURL)
		{
			this.loadingURL = false;
			this.aniLoadingPageStop();
		}
		this.contentRT.sizeDelta = new Vector2(this.contentRT.sizeDelta.x, 100f);
		if (this.currentWebsite != null)
		{
			this.currentWebsite.SetActive(false);
		}
		this.currentWebsiteURL = string.Empty;
		this.backList.Clear();
		this.forwardList.Clear();
	}

	public void homeBtnWasHit()
	{
		this.forwardList.Clear();
		this.backList.Clear();
		this.currentWebSiteAddToBack = false;
	}

	public void setPageLoadingTime(bool fasterValue)
	{
		this.pagesLoadFaster = fasterValue;
		this.pageLoadHandicapActive = true;
		GameManager.TimeSlinger.FireTimer(180f, new Action(this.resetPageLoadTime));
	}

	public void setKeyQueAlert(bool setValue)
	{
		this.keyQueAlertActive = true;
		this.keyQueStay = setValue;
		GameManager.TimeSlinger.FireTimer(300f, new Action(this.resetKeyQueAlert));
	}

	public bool isKeyQueAlertActive()
	{
		return this.keyQueAlertActive;
	}

	public bool getKeyQueStayStatus()
	{
		return this.keyQueStay;
	}

	private void generateWebsiteLoadTime()
	{
		this.websiteLoadTime = UnityEngine.Random.Range(this.websiteLoadTimeMin, this.websiteLoadTimeMax);
		if (this.pageLoadHandicapActive)
		{
			if (this.pagesLoadFaster)
			{
				this.websiteLoadTime = this.websiteLoadTimeMin;
			}
			else
			{
				this.websiteLoadTime = this.websiteLoadTimeMax;
			}
		}
		this.websiteLoadTimeStamp = Time.time;
		this.loadingURL = true;
		this.aniLoadingPage();
		if (this.currentWebsite.GetComponent<SiteHolder>() != null && this.currentWebsite.GetComponent<SiteHolder>().mySiteDeff.unMasksIP)
		{
			GameManager.TimeSlinger.FireTimer(this.websiteLoadTime + 0.5f, new Action(GameManager.GetTheHackerManager().UnMaskTheIP));
		}
	}

	private void loadURLFromBox(string theURL)
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.returnKeyHit, 1f, false);
		this.loadURL(theURL, true);
	}

	private void loadURL(string theURL, bool addToBackList = false)
	{
		if (this.forwardList.Count > 0)
		{
			int index = this.forwardList.Count - 1;
			if (theURL.Equals(this.forwardList[index]))
			{
				this.forwardList.RemoveAt(index);
			}
			else if (addToBackList)
			{
				this.forwardList.Clear();
			}
		}
		if (theURL != string.Empty && theURL != this.currentWebsiteURL)
		{
			string text = theURL.Replace("http://", string.Empty);
			text = text.Replace(".ann", string.Empty);
			if (text.Length > 0 && !this.loadingURL)
			{
				GameObject gameObject = null;
				this.theCloud.checkWebsite(out gameObject, theURL);
				if (this.currentWebsite != null)
				{
					this.currentWebsite.SetActive(false);
					if (this.currentWebSiteAddToBack)
					{
						this.backList.Add(this.currentWebsiteURL);
					}
				}
				this.currentWebsiteURL = theURL;
				this.currentWebSiteAddToBack = addToBackList;
				this.contentRT.sizeDelta = new Vector2(this.contentRT.sizeDelta.x, 100f);
				this.currentWebsite = gameObject;
				this.generateWebsiteLoadTime();
				this.HomeBTN.IAmLocked = true;
				this.backBTN.IAmLocked = true;
				this.fwdBTN.iAmLocked = true;
				this.urlField.enabled = false;
			}
		}
	}

	private void presentWebsite()
	{
		this.siteScrollRect.verticalNormalizedPosition = 1f;
		this.loadingURL = false;
		this.contentRT.sizeDelta = new Vector2(this.contentRT.sizeDelta.x, this.currentWebsite.GetComponent<RectTransform>().sizeDelta.y);
		this.currentWebsite.SetActive(true);
		if (this.currentWebsite.GetComponent<SiteHolder>() == null && this.currentWebsite.GetComponent<SubPageHolder>() == null)
		{
			this.aniLoadingPageStop();
		}
		this.HomeBTN.IAmLocked = false;
		this.backBTN.IAmLocked = false;
		this.fwdBTN.iAmLocked = false;
		this.urlField.enabled = true;
	}

	private void resetPageLoadTime()
	{
		this.pageLoadHandicapActive = false;
	}

	private void resetKeyQueAlert()
	{
		this.keyQueAlertActive = false;
		this.keyQueStay = false;
	}

	private void aniLoadingPage()
	{
		this.aniLoadingGlobeSeq = DOTween.Sequence();
		this.aniLoadingGlobeSeq.Insert(0f, DOTween.To(() => this.loadingGlobeBox.alpha, delegate(float x)
		{
			this.loadingGlobeBox.alpha = x;
		}, 0.3f, 0.5f));
		this.aniLoadingGlobeSeq.Insert(0.5f, DOTween.To(() => this.loadingGlobeBox.alpha, delegate(float x)
		{
			this.loadingGlobeBox.alpha = x;
		}, 1f, 0.5f));
		this.aniLoadingGlobeSeq.SetLoops(-1);
		this.aniLoadingGlobeSeq.Play<Sequence>();
		this.aniLoadingBarSeq = DOTween.Sequence();
		this.aniLoadingBarSeq.Insert(0f, DOTween.To(() => this.loadingBar.fillAmount, delegate(float x)
		{
			this.loadingBar.fillAmount = x;
		}, 1f, this.websiteLoadTime));
		this.aniLoadingBarSeq.Play<Sequence>();
		this.loadingText.gameObject.SetActive(true);
	}

	public void aniLoadingPageStop()
	{
		this.aniLoadingGlobeSeq.Kill(true);
		this.aniLoadingBarSeq.Kill(false);
		this.loadingText.gameObject.SetActive(false);
		this.loadingBar.fillAmount = 0f;
		this.loadingGlobeBox.alpha = 1f;
	}

	private void Awake()
	{
		GameManager.SetAnnBehavior(this);
	}

	private void Start()
	{
		this.loadingURL = false;
		this.backList = new List<string>();
		this.forwardList = new List<string>();
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
		submitEvent.AddListener(new UnityAction<string>(this.loadURLFromBox));
		this.urlField.onEndEdit = submitEvent;
	}

	private void Update()
	{
		if (this.loadingURL && Time.time - this.websiteLoadTimeStamp >= this.websiteLoadTime)
		{
			this.presentWebsite();
		}
		if (this.backList.Count > 0)
		{
			this.backBTN.setEnabled();
		}
		else
		{
			this.backBTN.setDisabled();
		}
		if (this.forwardList.Count > 0)
		{
			this.fwdBTN.setEnabled();
		}
		else
		{
			this.fwdBTN.setDisabled();
		}
	}

	[Range(1f, 3f)]
	public float websiteLoadTimeMin = 1f;

	[Range(3f, 12f)]
	public float websiteLoadTimeMax = 5f;

	public TheCloud theCloud;

	public RectTransform contentRT;

	public ScrollRect siteScrollRect;

	public homeBehavior HomeBTN;

	public InputField urlField;

	public CanvasGroup loadingGlobeBox;

	public Text loadingText;

	public Image loadingBar;

	public backBehavior backBTN;

	public forwardBehavior fwdBTN;

	public AudioClip returnKeyHit;

	private GameObject currentWebsite;

	private bool loadingURL;

	private float websiteLoadTimeStamp;

	private float websiteLoadTime;

	private string currentWebsiteURL = string.Empty;

	private bool currentWebSiteAddToBack;

	private Sequence aniLoadingGlobeSeq;

	private Sequence aniLoadingBarSeq;

	private List<string> backList;

	private List<string> forwardList;

	private bool pageLoadHandicapActive;

	private bool pagesLoadFaster;

	private bool keyQueAlertActive;

	private bool keyQueStay;
}
