using System;
using UnityEngine;

public class cursorManager : MonoBehaviour
{
	public void setCursorMoveable(bool setMove = false)
	{
		if (setMove)
		{
			Cursor.lockState = 0;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = 1;
			Cursor.visible = false;
		}
	}

	public void enableCursor()
	{
		Cursor.visible = true;
		Cursor.lockState = 0;
		this.cursorIsDisabled = false;
	}

	public void disableCursor()
	{
		Cursor.visible = false;
		Cursor.lockState = 1;
		this.cursorIsDisabled = true;
	}

	private void Start()
	{
		if (this.hideCursorOnStart)
		{
			Cursor.visible = false;
			Cursor.lockState = 1;
			this.cursorIsDisabled = true;
		}
	}

	public Texture2D customCursor;

	public bool hideCursorOnStart;

	public bool cursorIsDisabled;
}
