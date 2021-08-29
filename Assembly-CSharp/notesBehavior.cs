﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class notesBehavior : MonoBehaviour
{
	public void buildNotes()
	{
		string text = string.Empty;
		if (this.theCloud.playerNotes.Count > 0)
		{
			for (int i = 0; i < this.theCloud.playerNotes.Count; i++)
			{
				if (i == 0)
				{
					text += this.theCloud.playerNotes[i];
				}
				else
				{
					text = text + "\n\n" + this.theCloud.playerNotes[i];
				}
			}
		}
		if (text != string.Empty)
		{
			this.resizeNoteObjects(this.getStrHeight(text));
			this.noteField.text = text;
		}
	}

	public void addNote(string noteToAdd = "")
	{
		GameManager.AudioSlinger.DealSound(AudioHubs.COMPUTER, AudioLayer.COMPUTERSFX, this.returnKeyHit, 1f, false);
		if (noteToAdd != string.Empty && noteToAdd.Length >= 3)
		{
			this.theCloud.addPlayerNote(noteToAdd);
			if (this.noteField.text.Length > 0)
			{
				noteToAdd = this.noteField.text + "\n\n" + noteToAdd;
			}
			this.resizeNoteObjects(this.getStrHeight(noteToAdd));
			this.noteField.text = noteToAdd;
			this.noteInput.text = string.Empty;
		}
	}

	public void clearNotes()
	{
		this.resizeNoteObjects(this.getStrHeight("foobar"));
		this.noteField.text = string.Empty;
		this.noteInput.text = string.Empty;
	}

	private void resizeNoteObjects(float newHeight)
	{
		this.noteField.GetComponent<RectTransform>().sizeDelta = new Vector2(this.noteField.GetComponent<RectTransform>().sizeDelta.x, newHeight);
		this.noteFieldText.rectTransform.sizeDelta = new Vector2(this.noteFieldText.rectTransform.sizeDelta.x, newHeight);
		this.noteHolder.sizeDelta = new Vector2(this.noteHolder.sizeDelta.x, Mathf.Abs(newHeight + 10f));
	}

	private float getStrHeight(string theString)
	{
		TextGenerationSettings textGenerationSettings = default(TextGenerationSettings);
		TextGenerator textGenerator = new TextGenerator();
		textGenerationSettings.textAnchor = 0;
		textGenerationSettings.generateOutOfBounds = true;
		textGenerationSettings.generationExtents = new Vector2(this.noteField.GetComponent<RectTransform>().sizeDelta.x, this.noteField.GetComponent<RectTransform>().sizeDelta.y);
		textGenerationSettings.pivot = Vector2.zero;
		textGenerationSettings.richText = false;
		textGenerationSettings.font = this.noteFont;
		textGenerationSettings.fontSize = 14;
		textGenerationSettings.fontStyle = 0;
		textGenerationSettings.lineSpacing = 1f;
		textGenerationSettings.scaleFactor = 1f;
		textGenerationSettings.verticalOverflow = 1;
		textGenerationSettings.horizontalOverflow = 0;
		return textGenerator.GetPreferredHeight(theString, textGenerationSettings);
	}

	private void Start()
	{
		this.buildNotes();
		InputField.SubmitEvent submitEvent = new InputField.SubmitEvent();
		submitEvent.AddListener(new UnityAction<string>(this.addNote));
		this.noteInput.onEndEdit = submitEvent;
	}

	private void Update()
	{
	}

	public TheCloud theCloud;

	public RectTransform noteHolder;

	public InputField noteField;

	public InputField noteInput;

	public Text noteFieldText;

	public Font noteFont;

	public AudioClip returnKeyHit;
}
