using System;
using UnityEngine;

public class ShutDownBehavior : MonoBehaviour
{
	private void yesHit()
	{
		this.myUIManger.saveAndQuit();
	}

	private void noHit()
	{
		this.myUIManger.hideShutDown();
	}

	private void OnEnable()
	{
		this.yesBTN.hasAction = true;
		this.yesBTN.setAction = new Action(this.yesHit);
		this.noBTN.hasAction = true;
		this.noBTN.setAction = new Action(this.noHit);
	}

	public UIManager myUIManger;

	public btnBehavior yesBTN;

	public btnBehavior noBTN;
}
