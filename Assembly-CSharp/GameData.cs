using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	public GameData()
	{
		this.siteDataAlpha = new Dictionary<string, string>();
		this.siteDataBeta = new Dictionary<string, string>();
		this.siteDataCharlie = new List<Dictionary<string, string>>();
		this.gameDay = 1;
		this.playerNotes = new List<string>();
		this.playerSkillPoints1 = 0;
		this.playerSkillPoints2 = 0;
		this.playerSkillPoints3 = 0;
		this.txtDocs = new List<TheCloud.txtDocData>();
		this.siteDataDelta = string.Empty;
		this.siteDataEcho = new List<string>();
		this.siteDataFox = new Dictionary<string, int>();
		this.siteDataGood = new List<string>();
		this.siteDataHex = new Dictionary<string, int>();
		this.siteRateDisabled = false;
		this.N1 = false;
		this.N2 = false;
		this.N3 = false;
		this.N4 = false;
		this.playerResetStats = false;
	}

	public void resetData()
	{
		this.siteDataAlpha = new Dictionary<string, string>();
		this.siteDataBeta = new Dictionary<string, string>();
		this.siteDataCharlie = new List<Dictionary<string, string>>();
		this.gameDay = 1;
		this.playerNotes = new List<string>();
		this.playerSkillPoints1 = 0;
		this.playerSkillPoints2 = 0;
		this.playerSkillPoints3 = 0;
		this.txtDocs = new List<TheCloud.txtDocData>();
		this.siteDataDelta = string.Empty;
		this.siteDataEcho = new List<string>();
		this.siteDataFox = new Dictionary<string, int>();
		this.siteDataGood = new List<string>();
		this.siteDataHex = new Dictionary<string, int>();
		this.siteRateDisabled = false;
		this.N1 = false;
		this.N2 = false;
		this.N3 = false;
		this.N4 = false;
		this.playerResetStats = false;
	}

	public Dictionary<string, string> siteDataAlpha;

	public Dictionary<string, string> siteDataBeta;

	public List<Dictionary<string, string>> siteDataCharlie;

	public int gameDay;

	public List<string> playerNotes;

	public int playerSkillPoints1;

	public int playerSkillPoints2;

	public int playerSkillPoints3;

	public List<TheCloud.txtDocData> txtDocs;

	public string siteDataDelta;

	public List<string> siteDataEcho;

	public Dictionary<string, int> siteDataFox;

	public List<string> siteDataGood;

	public Dictionary<string, int> siteDataHex;

	public bool siteRateDisabled;

	public bool N1;

	public bool N2;

	public bool N3;

	public bool N4;

	public bool playerResetStats;
}
