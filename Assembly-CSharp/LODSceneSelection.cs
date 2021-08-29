using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LODSceneSelection : MonoBehaviour
{
	private void OnGUI()
	{
		Rect position = new Rect((float)(Screen.width / 2 - this.BoxWidth / 2), 0f, (float)this.BoxWidth, (float)this.BoxHeight);
		Rect screenRect = new Rect(position.x + (float)this.MarginH, position.y + (float)this.MarginV, (float)(this.BoxWidth - this.MarginH * 2), (float)(this.BoxHeight - this.MarginV * 2));
		GUI.Box(position, string.Empty);
		GUI.Box(position, string.Empty);
		GUILayout.BeginArea(screenRect);
		GUILayout.Label("Scene selection:", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		foreach (LODSceneSelection.SceneOption sceneOption in this.SceneOptions)
		{
			if (GUILayout.Button(sceneOption.m_sceneDisplayName, new GUILayoutOption[0]))
			{
				SceneManager.LoadScene(sceneOption.m_sceneName);
			}
		}
		if (GUILayout.Button("Exit", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public int BoxWidth = 300;

	public int BoxHeight = 50;

	public int MarginH = 20;

	public int MarginV = 20;

	public LODSceneSelection.SceneOption[] SceneOptions;

	[Serializable]
	public class SceneOption
	{
		public string m_sceneName;

		public string m_sceneDisplayName;
	}
}
