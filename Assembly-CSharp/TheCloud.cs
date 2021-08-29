using System;
using System.Collections.Generic;
using UnityEngine;

public class TheCloud : MonoBehaviour
{
	public bool CheckIfRealSite(string theURL = "")
	{
		string text = theURL.Replace("http://", string.Empty);
		text = text.Replace(".ann", string.Empty);
		return this.webSiteLookup.ContainsKey(text);
	}

	public void checkWebsite(out GameObject websiteObject, string websiteURL = "")
	{
		websiteObject = null;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		string text = string.Empty;
		if (!this.vistedSites.Contains(websiteURL))
		{
			text = websiteURL.Replace("http://", string.Empty);
			text = text.Replace(".ann", string.Empty);
			string[] array = text.Split(new string[]
			{
				"/"
			}, StringSplitOptions.None);
			if (this.webSiteLookup.ContainsKey(array[0]))
			{
				if (this.webSites[this.webSiteLookup[array[0]]].siteDefinition.hasWindow)
				{
					if (this.myTimeManager.isWindowOpen(this.webSites[this.webSiteLookup[array[0]]].siteDefinition) && !this.vistedSites.Contains(websiteURL))
					{
						this.vistedSites.Add(websiteURL);
					}
				}
				else if (!this.vistedSites.Contains(websiteURL))
				{
					this.vistedSites.Add(websiteURL);
				}
				this.checkDeepExp();
			}
		}
		if (websiteURL.Contains("http://") && websiteURL.Contains(".ann"))
		{
			text = websiteURL.Replace("http://", string.Empty);
			text = text.Replace(".ann", string.Empty);
			flag = true;
		}
		if (text == this.redRoomLink)
		{
			if (GameManager.GetGameModeManager().getCasualMode())
			{
				flag = false;
			}
			else
			{
				websiteObject = this.TheRedRoom;
				flag3 = true;
			}
		}
		if (flag)
		{
			for (int i = 0; i < this.wikiURLS.Count; i++)
			{
				if (text.Equals(this.wikiURLS[i]))
				{
					this.myWikiBeh.clearWikiLinks();
					this.myWikiBeh.buildWiki(i);
					websiteObject = this.wikiTemplate;
					flag2 = true;
					i = this.wikiURLS.Count;
				}
			}
		}
		if (!flag3 && !flag2)
		{
			if (flag)
			{
				if (text.Contains(".html"))
				{
					string[] array = text.Split(new string[]
					{
						"/"
					}, StringSplitOptions.None);
					if (this.webSiteLookup.ContainsKey(array[0]))
					{
						if (this.webSites[this.webSiteLookup[array[0]]].siteDefinition.hasWindow)
						{
							if (this.myTimeManager.isWindowOpen(this.webSites[this.webSiteLookup[array[0]]].siteDefinition))
							{
								if (this.webSites[this.webSiteLookup[array[0]]].siteHolder.haveSubPages)
								{
									GameObject gameObject;
									if (this.webSites[this.webSiteLookup[array[0]]].siteHolder.checkSubPage(array[1], out gameObject))
									{
										websiteObject = gameObject;
									}
									else
									{
										websiteObject = this.notFoundWebSite;
									}
								}
								else
								{
									websiteObject = this.notFoundWebSite;
								}
							}
							else
							{
								websiteObject = this.notFoundWebSite;
							}
						}
						else if (this.webSites[this.webSiteLookup[array[0]]].siteHolder.haveSubPages)
						{
							GameObject gameObject2;
							if (this.webSites[this.webSiteLookup[array[0]]].siteHolder.checkSubPage(array[1], out gameObject2))
							{
								websiteObject = gameObject2;
							}
							else
							{
								websiteObject = this.notFoundWebSite;
							}
						}
						else
						{
							websiteObject = this.notFoundWebSite;
						}
					}
					else
					{
						websiteObject = this.notFoundWebSite;
					}
				}
				else if (this.webSiteLookup.ContainsKey(text))
				{
					if (!this.webSites[this.webSiteLookup[text]].siteDefinition.siteIsSeized)
					{
						if (this.webSites[this.webSiteLookup[text]].siteDefinition.hasWindow)
						{
							if (this.myTimeManager.isWindowOpen(this.webSites[this.webSiteLookup[text]].siteDefinition))
							{
								websiteObject = this.webSites[this.webSiteLookup[text]].siteObject;
							}
							else
							{
								websiteObject = this.notFoundWebSite;
							}
						}
						else
						{
							websiteObject = this.webSites[this.webSiteLookup[text]].siteObject;
						}
					}
					else
					{
						websiteObject = this.seizedSite;
					}
				}
				else
				{
					websiteObject = this.notFoundWebSite;
				}
			}
			else
			{
				websiteObject = this.notFoundWebSite;
			}
		}
	}

	public string getWikiLink(int wikiIndex = 0)
	{
		if (wikiIndex < this.wikiURLS.Count)
		{
			return this.wikiURLS[wikiIndex];
		}
		return string.Empty;
	}

	public string getRandomKey()
	{
		string empty = string.Empty;
		int num = Random.Range(0, this.redRoomKeys.Count);
		return (num + 1).ToString() + " - " + this.redRoomKeys[num];
	}

	public Dictionary<string, string> getWikiList(int wikiIndex)
	{
		return this.wikis[wikiIndex];
	}

	public void addPlayerNote(string noteToAdd = "")
	{
		if (noteToAdd != string.Empty && noteToAdd.Length >= 3)
		{
			GameManager.FileSlinger.saveData.playerNotes.Add(noteToAdd);
		}
	}

	public void addTxtDoc(string txtDocTitle = "", string txtDocText = "")
	{
		float setX = Random.Range(150f, (float)Screen.width - 106f);
		float setY = -Random.Range(60f, (float)Screen.height - 180f);
		GameObject gameObject = Object.Instantiate<GameObject>(this.txtIconObject);
		gameObject.transform.SetParent(this.iconHolder);
		gameObject.GetComponent<txtIconBehavior>().mainController = this.mainController;
		gameObject.GetComponent<txtIconBehavior>().windowHolderRT = this.windowHolder;
		gameObject.GetComponent<txtIconBehavior>().dragPlane = this.dragPlane;
		gameObject.GetComponent<txtIconBehavior>().buildMe(setX, setY, txtDocTitle, txtDocText);
		GameManager.FileSlinger.saveData.txtDocs.Add(new TheCloud.txtDocData(setX, setY, txtDocTitle, txtDocText));
	}

	public void clearNotes()
	{
		GameManager.FileSlinger.saveData.playerNotes.Clear();
		this.playerNotes.Clear();
		GameManager.FileSlinger.saveFile("wttg2.gd");
		this.myNotesBeh.clearNotes();
	}

	public int getPlayerSkillPoints()
	{
		return this.playerSkillPoints1;
	}

	public int getPlayerSkillPoints2()
	{
		return this.playerSkillPoints2;
	}

	public int getPlayerSkillPoints3()
	{
		return this.playerSkillPoints3;
	}

	public void addPlayerSkillPoints(int amtToAdd = 0)
	{
		this.playerSkillPoints1 += amtToAdd;
		GameManager.FileSlinger.saveData.playerSkillPoints1 = this.playerSkillPoints1;
	}

	public void addPlayerSkillPoints2(int amtToAdd = 0)
	{
		this.playerSkillPoints2 += amtToAdd;
		GameManager.FileSlinger.saveData.playerSkillPoints2 = this.playerSkillPoints2;
	}

	public void addPlayerSkillPoints3(int amtToAdd = 0)
	{
		this.playerSkillPoints3 += amtToAdd;
		GameManager.FileSlinger.saveData.playerSkillPoints3 = this.playerSkillPoints3;
	}

	public void playerVisitedTappedSite(string siteKey = "")
	{
		Debug.Log("eneter tapped");
		if (siteKey != string.Empty && !this.visitedTappedSites.Contains(siteKey) && !GameManager.GetGameModeManager().getCasualMode())
		{
			this.visitedTappedSites.Add(siteKey);
			GameManager.FileSlinger.saveData.siteDataGood = this.visitedTappedSites;
			GameManager.GetTheBreatherManager().triggerBreatherCount(this.visitedTappedSites.Count);
			if (this.visitedTappedSites.Count == 3)
			{
				this.myTrackerManager.userCanNowBeTracked();
			}
			GameManager.FileSlinger.saveFile("wttg2.gd");
		}
	}

	public string getRedRoomKey(int index = 0)
	{
		return this.redRoomKeys[index];
	}

	public int getRedRoomKeyVistCount()
	{
		if (this.visitedTappedSites != null)
		{
			return this.visitedTappedSites.Count;
		}
		return 0;
	}

	public bool wasThisSiteVisted(string siteTocheck = "")
	{
		return this.vistedSites.Contains(siteTocheck);
	}

	public bool isThisTheRedRoomURL(string urlToCheck)
	{
		return urlToCheck == "http://" + this.redRoomLink + ".ann";
	}

	private void prepWebSites()
	{
		if (this.WebSiteGameObjects != null)
		{
			for (int i = 0; i < this.WebSiteGameObjects.Count; i++)
			{
				this.webSites.Add(new TheCloud.SiteData(GameManager.MagicSlinger.md5It(this.WebSiteGameObjects[i].GetComponent<SiteHolder>().mySiteDeff.PageTitle), this.WebSiteGameObjects[i], this.WebSiteGameObjects[i].GetComponent<SiteHolder>().mySiteDeff, this.WebSiteGameObjects[i].GetComponent<SiteHolder>()));
				if (!this.hasSavedData)
				{
					this.webSites[i].siteDefinition.iWasPicked = false;
				}
			}
			for (int j = 0; j < this.FakeSiteObjects.Count; j++)
			{
				this.fakeSites.Add(new TheCloud.FakeData(GameManager.MagicSlinger.md5It(this.FakeSiteObjects[j].PageTitle), this.FakeSiteObjects[j]));
			}
			if (this.webSites.Count > 0 && this.fakeSites.Count > 0)
			{
				this.webSitesLoaded = true;
			}
		}
		this.prepURLS();
	}

	private void prepURLS()
	{
		if (this.webSitesLoaded)
		{
			if (this.hasSavedData)
			{
				for (int i = 0; i < this.webSites.Count; i++)
				{
					this.webSites[i].siteDefinition.PageURL = GameManager.FileSlinger.saveData.siteDataAlpha[this.webSites[i].myKey];
					this.webSiteLookup.Add(this.webSites[i].siteDefinition.PageURL, i);
				}
				for (int j = 0; j < this.fakeSites.Count; j++)
				{
					this.fakeSites[j].fakeDefinition.PageURL = GameManager.FileSlinger.saveData.siteDataBeta[this.fakeSites[j].myKey];
				}
			}
			else
			{
				for (int k = 0; k < this.webSites.Count; k++)
				{
					if (!this.webSites[k].siteDefinition.isStatic)
					{
						this.webSites[k].siteDefinition.PageURL = GameManager.MagicSlinger.md5It(this.webSites[k].siteDefinition.PageTitle + Random.Range(0, 9999).ToString());
						this.webSiteLookup.Add(this.webSites[k].siteDefinition.PageURL, k);
						GameManager.FileSlinger.saveData.siteDataAlpha.Add(this.webSites[k].myKey, this.webSites[k].siteDefinition.PageURL);
					}
					else
					{
						this.webSiteLookup.Add(this.webSites[k].siteDefinition.staticURL, k);
						GameManager.FileSlinger.saveData.siteDataAlpha.Add(this.webSites[k].myKey, this.webSites[k].siteDefinition.staticURL);
					}
				}
				for (int l = 0; l < this.fakeSites.Count; l++)
				{
					this.fakeSites[l].fakeDefinition.PageURL = GameManager.MagicSlinger.md5It(this.fakeSites[l].fakeDefinition.PageTitle + Random.Range(0, 9999).ToString());
					GameManager.FileSlinger.saveData.siteDataBeta.Add(this.fakeSites[l].myKey, this.fakeSites[l].fakeDefinition.PageURL);
				}
				GameManager.FileSlinger.saveFile("wttg2.gd");
			}
		}
		this.prepWikis();
	}

	private void prepWikis()
	{
		if (this.webSitesLoaded)
		{
			if (this.hasSavedData)
			{
				this.wikis = GameManager.FileSlinger.saveData.siteDataCharlie;
				for (int i = 0; i < this.wikis.Count; i++)
				{
					bool flag = false;
					foreach (KeyValuePair<string, string> keyValuePair in this.wikis[i])
					{
						if (!flag)
						{
							this.wikiURLS.Add(GameManager.MagicSlinger.md5It(keyValuePair.Key + ":" + keyValuePair.Value));
							flag = true;
						}
					}
				}
			}
			else
			{
				int num = 3;
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				for (int j = 0; j < num; j++)
				{
					Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
					Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
					List<string> list = new List<string>();
					int k = 0;
					int l = 0;
					if (j == 0)
					{
						for (int m = 0; m < this.webSites.Count; m++)
						{
							if (this.webSites[m].siteDefinition.onFirstWiki)
							{
								dictionary2.Add(this.webSites[m].siteDefinition.PageTitle, this.webSites[m].siteDefinition.PageURL);
								dictionary.Add(this.webSites[m].siteDefinition.PageTitle, this.webSites[m].siteDefinition.PageURL);
								l++;
								k++;
							}
						}
					}
					if (j == 1)
					{
						for (int n = 0; n < this.webSites.Count; n++)
						{
							if (this.webSites[n].siteDefinition.onSecondWiki)
							{
								dictionary2.Add(this.webSites[n].siteDefinition.PageTitle, this.webSites[n].siteDefinition.PageURL);
								dictionary.Add(this.webSites[n].siteDefinition.PageTitle, this.webSites[n].siteDefinition.PageURL);
								l++;
								k++;
							}
						}
					}
					if (j == 2)
					{
						for (int num2 = 0; num2 < this.webSites.Count; num2++)
						{
							if (this.webSites[num2].siteDefinition.onThirdWiki)
							{
								dictionary2.Add(this.webSites[num2].siteDefinition.PageTitle, this.webSites[num2].siteDefinition.PageURL);
								dictionary.Add(this.webSites[num2].siteDefinition.PageTitle, this.webSites[num2].siteDefinition.PageURL);
								l++;
								k++;
							}
						}
					}
					while (l < this.realLinksPerWiki)
					{
						int index = Random.Range(0, this.webSites.Count);
						if (!dictionary.ContainsKey(this.webSites[index].siteDefinition.PageTitle) && !this.webSites[index].siteDefinition.onFirstWiki && !this.webSites[index].siteDefinition.onSecondWiki && !this.webSites[index].siteDefinition.onThirdWiki && !this.webSites[index].siteDefinition.isStatic)
						{
							this.webSites[index].siteDefinition.iWasPicked = true;
							dictionary2.Add(this.webSites[index].siteDefinition.PageTitle, this.webSites[index].siteDefinition.PageURL);
							dictionary.Add(this.webSites[index].siteDefinition.PageTitle, this.webSites[index].siteDefinition.PageURL);
							l++;
							k++;
						}
					}
					while (k < this.linksPerWiki)
					{
						int index2 = Random.Range(0, this.fakeSites.Count);
						if (!dictionary.ContainsKey(this.fakeSites[index2].fakeDefinition.PageTitle))
						{
							dictionary2.Add(this.fakeSites[index2].fakeDefinition.PageTitle, this.fakeSites[index2].fakeDefinition.PageURL);
							dictionary.Add(this.fakeSites[index2].fakeDefinition.PageTitle, this.fakeSites[index2].fakeDefinition.PageURL);
							k++;
						}
					}
					foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
					{
						list.Add(keyValuePair2.Key);
					}
					list.Sort();
					for (int num3 = 0; num3 < list.Count; num3++)
					{
						dictionary3.Add(list[num3], dictionary2[list[num3]]);
					}
					this.wikis.Add(dictionary3);
				}
				for (int num4 = 0; num4 < this.wikis.Count; num4++)
				{
					bool flag2 = false;
					foreach (KeyValuePair<string, string> keyValuePair3 in this.wikis[num4])
					{
						if (!flag2)
						{
							this.wikiURLS.Add(GameManager.MagicSlinger.md5It(keyValuePair3.Key + ":" + keyValuePair3.Value));
							flag2 = true;
						}
					}
				}
				GameManager.FileSlinger.saveData.siteDataCharlie = this.wikis;
				GameManager.FileSlinger.saveFile("wttg2.gd");
			}
		}
	}

	private void prepRedRoom()
	{
		if (this.hasSavedData)
		{
			this.redRoomLink = GameManager.FileSlinger.saveData.siteDataDelta;
			this.redRoomKeys = GameManager.FileSlinger.saveData.siteDataEcho;
			this.tappedWebsites = GameManager.FileSlinger.saveData.siteDataFox;
			this.visitedTappedSites = GameManager.FileSlinger.saveData.siteDataGood;
			GameManager.GetTheBreatherManager().triggerBreatherCount(this.visitedTappedSites.Count);
			if (this.visitedTappedSites.Count >= 3)
			{
				this.myTrackerManager.userCanNowBeTracked();
			}
		}
		else
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string text = GameManager.MagicSlinger.md5It("welcomeT0TheGame" + Random.Range(0, 99999).ToString() + "lol" + Random.Range(0, 9999).ToString());
			List<string> list = new List<string>();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				list.Add(text.Substring(num, 4));
				num += 4;
			}
			while (!flag)
			{
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, string> keyValuePair in this.wikis[num3])
				{
					list2.Add(keyValuePair.Value);
				}
				int index = Random.Range(0, list2.Count);
				if (this.CheckIfRealSite(list2[index]))
				{
					TheCloud.SiteData siteData = this.webSites[this.webSiteLookup[list2[index]]];
					if (siteData.siteDefinition.iWasPicked && !siteData.siteDefinition.cantBeTapped && !dictionary.ContainsKey(siteData.myKey))
					{
						dictionary.Add(siteData.myKey, this.webSiteLookup[list2[index]]);
						num2++;
						num4++;
					}
				}
				if (num4 >= 2)
				{
					num3++;
					num4 = 0;
				}
				if (num3 > 2)
				{
					num3 = Random.Range(0, 3);
				}
				if (num2 > 6)
				{
					flag = true;
				}
			}
			this.redRoomLink = text;
			this.redRoomKeys = list;
			this.tappedWebsites = dictionary;
			this.visitedTappedSites = new List<string>();
			GameManager.FileSlinger.saveData.siteDataDelta = this.redRoomLink;
			GameManager.FileSlinger.saveData.siteDataEcho = this.redRoomKeys;
			GameManager.FileSlinger.saveData.siteDataFox = this.tappedWebsites;
			GameManager.FileSlinger.saveData.siteDataGood = this.visitedTappedSites;
			GameManager.FileSlinger.saveFile("wttg2.gd");
		}
		int num5 = 0;
		foreach (KeyValuePair<string, int> keyValuePair2 in this.tappedWebsites)
		{
			this.webSites[keyValuePair2.Value].siteObject.GetComponent<SiteHolder>().tapMe(num5 + 1, this.redRoomKeys[num5]);
			num5++;
		}
	}

	private void prepNotes()
	{
		if (this.hasSavedData)
		{
			this.playerNotes = GameManager.FileSlinger.saveData.playerNotes;
		}
	}

	private void prepTxtDocs()
	{
		if (this.hasSavedData && GameManager.FileSlinger.saveData.txtDocs != null)
		{
			List<TheCloud.txtDocData> txtDocs = GameManager.FileSlinger.saveData.txtDocs;
			for (int i = 0; i < txtDocs.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.txtIconObject);
				if (txtDocs[i].myX > (float)Screen.width)
				{
					TheCloud.txtDocData value = txtDocs[i];
					value.myX = Random.Range(150f, (float)Screen.width - 106f);
					txtDocs[i] = value;
				}
				if (Mathf.Abs(txtDocs[i].myY) > (float)Screen.height)
				{
					TheCloud.txtDocData value2 = txtDocs[i];
					value2.myY = -Random.Range(60f, (float)Screen.height - 180f);
					txtDocs[i] = value2;
				}
				gameObject.transform.SetParent(this.iconHolder);
				gameObject.GetComponent<txtIconBehavior>().mainController = this.mainController;
				gameObject.GetComponent<txtIconBehavior>().windowHolderRT = this.windowHolder;
				gameObject.GetComponent<txtIconBehavior>().dragPlane = this.dragPlane;
				gameObject.GetComponent<txtIconBehavior>().buildMe(txtDocs[i].myX, txtDocs[i].myY, txtDocs[i].myTitle, txtDocs[i].myText);
			}
		}
	}

	private void prepPlayerSkillPoints()
	{
		if (this.hasSavedData)
		{
			this.playerSkillPoints1 = GameManager.FileSlinger.saveData.playerSkillPoints1;
			this.playerSkillPoints2 = GameManager.FileSlinger.saveData.playerSkillPoints2;
			this.playerSkillPoints3 = GameManager.FileSlinger.saveData.playerSkillPoints3;
		}
		else
		{
			this.playerSkillPoints1 = 0;
			this.playerSkillPoints2 = 0;
			this.playerSkillPoints3 = 0;
		}
	}

	private void checkDeepExp()
	{
		if (this.vistedSites.Count == this.webSiteLookup.Count)
		{
			GameManager.SteamSlinger.triggerSteamAchievement(GameManager.SteamSlinger.ACHIEVEMENT_DEEP_EXPLORER, true);
		}
	}

	private void Awake()
	{
		if (GameManager.FileSlinger.deleteTheFile)
		{
			GameManager.FileSlinger.deleteFile("wttg2.gd");
			GameManager.FileSlinger.saveData.resetData();
			GameManager.FileSlinger.deleteTheFile = false;
		}
		this.hasSavedData = GameManager.FileSlinger.loadFile("wttg2.gd");
		GameManager.SetTheCloud(this);
		GameManager.AudioSlinger.ClearAudioHubs();
	}

	private void Start()
	{
		this.webSites = new List<TheCloud.SiteData>();
		this.fakeSites = new List<TheCloud.FakeData>();
		this.wikiURLS = new List<string>();
		this.playerNotes = new List<string>();
		this.vistedSites = new List<string>();
		this.wikis = new List<Dictionary<string, string>>();
		this.webSiteLookup = new Dictionary<string, int>();
		this.myWikiBeh = this.wikiTemplate.GetComponent<wikiBehavior>();
		this.prepWebSites();
		this.prepNotes();
		this.prepTxtDocs();
		this.prepPlayerSkillPoints();
		this.prepRedRoom();
	}

	[Range(10f, 50f)]
	public int linksPerWiki;

	[Range(5f, 25f)]
	public int realLinksPerWiki;

	public TimeManager myTimeManager;

	public TrackerManager myTrackerManager;

	public GameObject wikiTemplate;

	public GameObject notFoundWebSite;

	public GameObject seizedSite;

	public GameObject TheRedRoom;

	public notesBehavior myNotesBeh;

	public curtianScrub myCurScrub;

	public GameObject txtIconObject;

	public mainController mainController;

	public RectTransform iconHolder;

	public RectTransform windowHolder;

	public RectTransform dragPlane;

	public List<GameObject> WebSiteGameObjects;

	public List<FakeDefinition> FakeSiteObjects;

	public List<string> playerNotes;

	public int playerSkillPoints1;

	public int playerSkillPoints2;

	public int playerSkillPoints3;

	private List<TheCloud.SiteData> webSites;

	private List<TheCloud.FakeData> fakeSites;

	private List<Dictionary<string, string>> wikis;

	private List<string> wikiURLS;

	private Dictionary<string, int> webSiteLookup;

	private bool webSitesLoaded;

	private bool hasSavedData;

	private string redRoomLink;

	private List<string> redRoomKeys;

	private Dictionary<string, int> tappedWebsites;

	private List<string> visitedTappedSites;

	private List<string> vistedSites;

	private wikiBehavior myWikiBeh;

	public struct SiteData
	{
		public SiteData(string setKey, GameObject setSiteObject, SiteDefinition setSiteDef, SiteHolder setSiteHolder)
		{
			this.myKey = setKey;
			this.siteObject = setSiteObject;
			this.siteDefinition = setSiteDef;
			this.siteHolder = setSiteHolder;
		}

		public string myKey;

		public GameObject siteObject;

		public SiteDefinition siteDefinition;

		public SiteHolder siteHolder;
	}

	public struct FakeData
	{
		public FakeData(string setKey, FakeDefinition setFakeDef)
		{
			this.myKey = setKey;
			this.fakeDefinition = setFakeDef;
		}

		public string myKey;

		public FakeDefinition fakeDefinition;
	}

	[Serializable]
	public struct txtDocData
	{
		public txtDocData(float setX, float setY, string setTitle, string setText)
		{
			this.myX = setX;
			this.myY = setY;
			this.myTitle = setTitle;
			this.myText = setText;
		}

		public float myX;

		public float myY;

		public string myTitle;

		public string myText;
	}
}
