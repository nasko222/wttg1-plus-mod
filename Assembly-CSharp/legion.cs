using System;
using UnityEngine;
using UnityEngine.UI;

public class legion : MonoBehaviour
{
	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		this.hashHolder.text = num.ToString() + " - " + empty;
		this.hashHolder.gameObject.SetActive(true);
	}

	private void OnEnable()
	{
		if (base.GetComponent<SiteHolder>().wasITapped())
		{
			this.iWasTapped();
		}
	}

	private void OnDisable()
	{
		this.hashHolder.gameObject.SetActive(false);
	}

	public Text hashHolder;
}
