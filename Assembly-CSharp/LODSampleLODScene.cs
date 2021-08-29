using System;
using System.Collections.Generic;
using UnityEngine;

public class LODSampleLODScene : MonoBehaviour
{
	private void Start()
	{
		AutomaticLOD[] array = UnityEngine.Object.FindObjectsOfType<AutomaticLOD>();
		this.m_sceneLODObjects = new List<AutomaticLOD>();
		this.m_objectMaterials = new Dictionary<GameObject, Material[]>();
		this.m_nMaxLODLevels = 0;
		foreach (AutomaticLOD automaticLOD in array)
		{
			if (automaticLOD.IsRootAutomaticLOD())
			{
				this.m_sceneLODObjects.Add(automaticLOD);
				if (automaticLOD.GetLODLevelCount() > this.m_nMaxLODLevels)
				{
					this.m_nMaxLODLevels = automaticLOD.GetLODLevelCount();
				}
				this.AddMaterials(automaticLOD.gameObject, this.m_objectMaterials);
			}
		}
		if (this.SceneCameras != null && this.SceneCameras.Length > 0)
		{
			foreach (LODSampleLODScene.SceneCamera sceneCamera in this.SceneCameras)
			{
				sceneCamera.m_v3InitialCameraPosition = sceneCamera.m_camera.transform.position;
				sceneCamera.m_v3ViewDir = sceneCamera.m_camera.transform.forward;
			}
			this.SetActiveCamera(0);
		}
		this.m_bWireframe = false;
	}

	private void Update()
	{
		this.m_nCamMode = 0;
		if (Input.GetKey(KeyCode.I))
		{
			this.m_nCamMode = 1;
		}
		else if (Input.GetKey(KeyCode.O))
		{
			this.m_nCamMode = -1;
		}
		if (this.m_nCamMode != 0)
		{
			this.m_fCurrentDistanceSlider -= Time.deltaTime * 0.1f * (float)this.m_nCamMode;
			this.m_fCurrentDistanceSlider = Mathf.Clamp01(this.m_fCurrentDistanceSlider);
			this.UpdateCamera(this.m_fCurrentDistanceSlider);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			this.m_bWireframe = !this.m_bWireframe;
			this.SetWireframe(this.m_bWireframe);
		}
	}

	private void OnGUI()
	{
		int num = 400;
		if (this.SceneCameras == null)
		{
			return;
		}
		if (this.SceneCameras.Length == 0)
		{
			return;
		}
		GUI.Box(new Rect(0f, 0f, (float)(num + 10), 260f), string.Empty);
		GUILayout.Space(20f);
		GUILayout.Label("Select camera:", new GUILayoutOption[]
		{
			GUILayout.Width((float)num)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		for (int i = 0; i < this.SceneCameras.Length; i++)
		{
			if (GUILayout.Button(this.SceneCameras[i].m_camera.name, new GUILayoutOption[0]))
			{
				this.SetActiveCamera(i);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Camera distance:", new GUILayoutOption[]
		{
			GUILayout.Width((float)num)
		});
		GUI.changed = false;
		this.m_fCurrentDistanceSlider = GUILayout.HorizontalSlider(this.m_fCurrentDistanceSlider, 0f, 1f, new GUILayoutOption[0]);
		if (GUI.changed)
		{
			this.UpdateCamera(this.m_fCurrentDistanceSlider);
		}
		GUI.changed = false;
		this.m_bWireframe = GUILayout.Toggle(this.m_bWireframe, "Show wireframe", new GUILayoutOption[0]);
		if (GUI.changed)
		{
			this.SetWireframe(this.m_bWireframe);
		}
		GUILayout.Space(20f);
		GUILayout.Label("Select LOD:", new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button("Automatic LOD", new GUILayoutOption[0]))
		{
			foreach (AutomaticLOD automaticLOD in this.m_sceneLODObjects)
			{
				automaticLOD.SetAutomaticCameraLODSwitch(true);
			}
		}
		for (int j = 0; j < this.m_nMaxLODLevels; j++)
		{
			if (GUILayout.Button("LOD " + j, new GUILayoutOption[0]))
			{
				foreach (AutomaticLOD automaticLOD2 in this.m_sceneLODObjects)
				{
					automaticLOD2.SetAutomaticCameraLODSwitch(false);
					automaticLOD2.SwitchToLOD(j, true);
				}
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		int num2 = 0;
		int num3 = 0;
		foreach (AutomaticLOD automaticLOD3 in this.m_sceneLODObjects)
		{
			num2 += automaticLOD3.GetCurrentVertexCount(true);
			num3 += automaticLOD3.GetOriginalVertexCount(true);
		}
		GUILayout.Label(string.Concat(new object[]
		{
			"Vertex count: ",
			num2,
			"/",
			num3,
			" ",
			Mathf.RoundToInt(100f * ((float)num2 / (float)num3)).ToString(),
			"%"
		}), new GUILayoutOption[0]);
		GUILayout.Space(20f);
	}

	private void SetActiveCamera(int index)
	{
		foreach (LODSampleLODScene.SceneCamera sceneCamera in this.SceneCameras)
		{
			sceneCamera.m_camera.gameObject.SetActive(false);
		}
		this.m_selectedCamera = this.SceneCameras[index];
		this.m_selectedCamera.m_camera.gameObject.SetActive(true);
		this.m_selectedCamera.m_camera.transform.position = this.m_selectedCamera.m_v3InitialCameraPosition;
		this.m_fCurrentDistanceSlider = this.m_selectedCamera.m_near / (this.m_selectedCamera.m_near - this.m_selectedCamera.m_far);
	}

	private void UpdateCamera(float fPos)
	{
		Vector3 position = Vector3.Lerp(this.m_selectedCamera.m_v3InitialCameraPosition + this.m_selectedCamera.m_v3ViewDir * this.m_selectedCamera.m_near, this.m_selectedCamera.m_v3InitialCameraPosition + this.m_selectedCamera.m_v3ViewDir * this.m_selectedCamera.m_far, fPos);
		this.m_selectedCamera.m_camera.transform.position = position;
	}

	private void AddMaterials(GameObject theGameObject, Dictionary<GameObject, Material[]> dicMaterials)
	{
		Renderer component = theGameObject.GetComponent<Renderer>();
		AutomaticLOD component2 = theGameObject.GetComponent<AutomaticLOD>();
		if (component != null && component.sharedMaterials != null && component2 != null && component2 != null)
		{
			dicMaterials.Add(theGameObject, component.sharedMaterials);
		}
		for (int i = 0; i < theGameObject.transform.childCount; i++)
		{
			this.AddMaterials(theGameObject.transform.GetChild(i).gameObject, dicMaterials);
		}
	}

	private void SetWireframe(bool bEnabled)
	{
		this.m_bWireframe = bEnabled;
		foreach (KeyValuePair<GameObject, Material[]> keyValuePair in this.m_objectMaterials)
		{
			Renderer component = keyValuePair.Key.GetComponent<Renderer>();
			if (bEnabled)
			{
				Material[] array = new Material[keyValuePair.Value.Length];
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					array[i] = this.WireframeMaterial;
				}
				component.sharedMaterials = array;
			}
			else
			{
				component.sharedMaterials = keyValuePair.Value;
			}
		}
	}

	public LODSampleLODScene.SceneCamera[] SceneCameras;

	public Material WireframeMaterial;

	private Dictionary<GameObject, Material[]> m_objectMaterials;

	private LODSampleLODScene.SceneCamera m_selectedCamera;

	private bool m_bWireframe;

	private List<AutomaticLOD> m_sceneLODObjects;

	private int m_nMaxLODLevels;

	private float m_fCurrentDistanceSlider;

	private int m_nCamMode;

	[Serializable]
	public class SceneCamera
	{
		public Camera m_camera;

		public float m_near;

		public float m_far;

		[HideInInspector]
		public Vector3 m_v3InitialCameraPosition;

		[HideInInspector]
		public Vector3 m_v3ViewDir;
	}
}
