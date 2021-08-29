using System;
using UnityEngine;

public class cursorManager : MonoBehaviour
{
	public void setCursorMoveable(bool setMove = false)
	{
		if (setMove)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	public void enableCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		this.cursorIsDisabled = false;
	}

	public void disableCursor()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		this.cursorIsDisabled = true;
	}

	private void Start()
	{
		if (this.hideCursorOnStart)
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			this.cursorIsDisabled = true;
		}
	}

	public Texture2D customCursor;

	public bool hideCursorOnStart;

	public bool cursorIsDisabled;
}
