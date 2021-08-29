using System;
using UnityEngine;
using UnityEngine.UI;

public class SPKeyStatic : MonoBehaviour
{
	private void iWasTapped()
	{
		int num = 0;
		string empty = string.Empty;
		this.hashHolder.gameObject.SetActive(true);
		base.GetComponent<SubPageHolder>().getKeyIndex(out num, out empty);
		this.hashHolder.text = num.ToString() + " - " + empty;
	}

	private void OnEnable()
	{
		if (!GameManager.GetGameModeManager().getCasualMode() && base.GetComponent<SubPageHolder>().iWasTapped)
		{
			this.iWasTapped();
		}
	}

	private void OnDisable()
	{
	}

	public Text hashHolder;
}
