using System;
using UnityEngine;
using UnityEngine.UI;

public class SiteKeyStatic : MonoBehaviour
{
	public void tapTheSite()
	{
		if (!GameManager.GetGameModeManager().getCasualMode())
		{
			this.iWasTapped();
		}
	}

	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		this.hashHolder.gameObject.SetActive(true);
		base.GetComponent<SiteHolder>().getHashInfo(out num, out empty);
		this.hashHolder.text = num.ToString() + " - " + empty;
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public Text hashHolder;
}
