using System;
using System.Collections.Generic;
using UnityEngine;

public class nBehavior : MonoBehaviour
{
	private void tapALink()
	{
		int index = UnityEngine.Random.Range(0, this.linkObjs.Count);
		this.linkObjs[index].deadUnlessTapped = false;
		this.linkObjs[index].hasTapAction = true;
		this.linkObjs[index].tapAction = new Action(this.generateNDoc);
	}

	private void generateNDoc()
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.nClickSound, 0.8f, false);
		string text = this.nIndex.ToString() + "/4";
		if (this.nIndex == 4)
		{
			text += " - Don't you have a Red Room to find?";
		}
		GameManager.GetTheCloud().addTxtDoc("N" + this.nIndex.ToString() + ".txt", text);
		switch (this.nIndex)
		{
		case 1:
			GameManager.FileSlinger.saveData.N1 = true;
			break;
		case 2:
			GameManager.FileSlinger.saveData.N2 = true;
			break;
		case 3:
			GameManager.FileSlinger.saveData.N3 = true;
			break;
		case 4:
			GameManager.FileSlinger.saveData.N4 = true;
			break;
		}
		GameManager.FileSlinger.saveFile("wttg2.gd");
	}

	private void OnEnable()
	{
		this.tapALink();
	}

	public int nIndex;

	public List<LinkObject> linkObjs;

	public AudioClip nClickSound;
}
