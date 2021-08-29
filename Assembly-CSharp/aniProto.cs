using System;
using UnityEngine;

public class aniProto : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		GameManager.GetTheBreatherManager().endOfAniClip(this.myCallSubAction);
	}

	public string myCallSubAction;
}
