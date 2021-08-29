using System;
using UnityEngine;

[Serializable]
public class SiteDefinition : Definition
{
	public bool siteIsSeized;

	public bool cantBeTapped;

	public bool onFirstWiki;

	public bool onSecondWiki;

	public bool onThirdWiki;

	public bool iWasPicked;

	public string PageTitle;

	public string PageURL;

	public bool isStatic;

	public string staticURL;

	public bool hasWindow;

	[Range(0f, 6f)]
	public int windowUp;

	[Range(1f, 6f)]
	public int windowDown;

	public bool unMasksIP;
}
