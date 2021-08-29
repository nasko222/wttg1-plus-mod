using System;
using System.Collections.Generic;
using UnityEngine;

public class contentFilterBehavior : MonoBehaviour
{
	private void filterContent()
	{
		for (int i = 0; i < this.filterObjects.Count; i++)
		{
			this.filterObjects[i].SetActive(true);
		}
	}

	private void OnEnable()
	{
		if (GameManager.FileSlinger.optData.contentFilter)
		{
			this.filterContent();
		}
	}

	public List<GameObject> filterObjects;
}
