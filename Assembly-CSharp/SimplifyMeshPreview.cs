using System;
using System.Collections;
using System.Collections.Generic;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

public class SimplifyMeshPreview : MonoBehaviour
{
	private void Start()
	{
		if (this.ShowcaseObjects != null && this.ShowcaseObjects.Length > 0)
		{
			for (int i = 0; i < this.ShowcaseObjects.Length; i++)
			{
				this.ShowcaseObjects[i].m_description = this.ShowcaseObjects[i].m_description.Replace("\\n", Environment.NewLine);
			}
			this.SetActiveObject(0);
		}
		Simplifier.CoroutineFrameMiliseconds = 20;
	}

	private void Progress(string strTitle, string strMessage, float fT)
	{
		int num = Mathf.RoundToInt(fT * 100f);
		if (num != this.m_nLastProgress || this.m_strLastTitle != strTitle || this.m_strLastMessage != strMessage)
		{
			this.m_strLastTitle = strTitle;
			this.m_strLastMessage = strMessage;
			this.m_nLastProgress = num;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(282))
		{
			this.m_bGUIEnabled = !this.m_bGUIEnabled;
		}
		if (Input.GetKeyDown(119))
		{
			this.m_bWireframe = !this.m_bWireframe;
			this.SetWireframe(this.m_bWireframe);
		}
		if (this.m_selectedMeshSimplify != null)
		{
			if (Input.GetMouseButton(0) && Input.mousePosition.y > 100f)
			{
				Vector3 vector = this.ShowcaseObjects[this.m_nSelectedIndex].m_rotationAxis * -((Input.mousePosition.x - this.m_fLastMouseX) * this.MouseSensitvity);
				this.m_selectedMeshSimplify.transform.Rotate(vector, 1);
			}
			else if (Input.GetMouseButtonUp(0) && Input.mousePosition.y > 100f)
			{
				this.m_fRotationSpeed = -(Input.mousePosition.x - this.m_fLastMouseX) * this.MouseReleaseSpeed;
			}
			else
			{
				Vector3 vector2 = this.ShowcaseObjects[this.m_nSelectedIndex].m_rotationAxis * (this.m_fRotationSpeed * Time.deltaTime);
				this.m_selectedMeshSimplify.transform.Rotate(vector2, 1);
			}
		}
		this.m_fLastMouseX = Input.mousePosition.x;
	}

	private void OnGUI()
	{
		int num = 150;
		int num2 = 50;
		int num3 = 20;
		int num4 = 10;
		Rect rect;
		rect..ctor((float)(Screen.width / 2 - num / 2), 0f, (float)num, (float)num2);
		Rect rect2;
		rect2..ctor(rect.x + (float)num3, rect.y + (float)num4, (float)(num - num3 * 2), (float)(num2 - num4 * 2));
		GUILayout.BeginArea(rect2);
		if (GUILayout.Button("Exit", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.EndArea();
		if (!this.m_bGUIEnabled)
		{
			return;
		}
		int num5 = 400;
		if (this.ShowcaseObjects == null)
		{
			return;
		}
		bool flag = true;
		if (!string.IsNullOrEmpty(this.m_strLastTitle) && !string.IsNullOrEmpty(this.m_strLastMessage))
		{
			flag = false;
		}
		GUI.Box(new Rect(0f, 0f, (float)(num5 + 10), 400f), string.Empty);
		GUILayout.Label("Select model:", new GUILayoutOption[]
		{
			GUILayout.Width((float)num5)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		for (int i = 0; i < this.ShowcaseObjects.Length; i++)
		{
			if (GUILayout.Button(this.ShowcaseObjects[i].m_meshSimplify.name, new GUILayoutOption[0]) && flag)
			{
				if (this.m_selectedMeshSimplify != null)
				{
					Object.DestroyImmediate(this.m_selectedMeshSimplify.gameObject);
				}
				this.SetActiveObject(i);
			}
		}
		GUILayout.EndHorizontal();
		if (this.m_selectedMeshSimplify != null)
		{
			GUILayout.Space(20f);
			GUILayout.Label(this.ShowcaseObjects[this.m_nSelectedIndex].m_description, new GUILayoutOption[0]);
			GUILayout.Space(20f);
			GUI.changed = false;
			this.m_bWireframe = GUILayout.Toggle(this.m_bWireframe, "Show wireframe", new GUILayoutOption[0]);
			if (GUI.changed && this.m_selectedMeshSimplify != null)
			{
				this.SetWireframe(this.m_bWireframe);
			}
			GUILayout.Space(20f);
			int simplifiedVertexCount = this.m_selectedMeshSimplify.GetSimplifiedVertexCount(true);
			int originalVertexCount = this.m_selectedMeshSimplify.GetOriginalVertexCount(true);
			GUILayout.Label(string.Concat(new object[]
			{
				"Vertex count: ",
				simplifiedVertexCount,
				"/",
				originalVertexCount,
				" ",
				Mathf.RoundToInt((float)simplifiedVertexCount / (float)originalVertexCount * 100f).ToString(),
				"% from original"
			}), new GUILayoutOption[0]);
			GUILayout.Space(20f);
			if (!string.IsNullOrEmpty(this.m_strLastTitle) && !string.IsNullOrEmpty(this.m_strLastMessage))
			{
				GUILayout.Label(this.m_strLastTitle + ": " + this.m_strLastMessage, new GUILayoutOption[]
				{
					GUILayout.MaxWidth((float)num5)
				});
				GUI.color = Color.blue;
				Rect lastRect = GUILayoutUtility.GetLastRect();
				GUI.Box(new Rect(10f, lastRect.yMax + 5f, 204f, 24f), string.Empty);
				GUI.Box(new Rect(12f, lastRect.yMax + 7f, (float)(this.m_nLastProgress * 2), 20f), string.Empty);
			}
			else
			{
				GUILayout.Label("Vertices: " + (this.m_fVertexAmount * 100f).ToString("0.00") + "%", new GUILayoutOption[0]);
				this.m_fVertexAmount = GUILayout.HorizontalSlider(this.m_fVertexAmount, 0f, 1f, new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				});
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Space(3f);
				if (GUILayout.Button("Compute simplified mesh", new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				}))
				{
					base.StartCoroutine(this.ComputeMeshWithVertices(this.m_fVertexAmount));
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}
	}

	private void SetActiveObject(int index)
	{
		this.m_nSelectedIndex = index;
		MeshSimplify meshSimplify = Object.Instantiate<MeshSimplify>(this.ShowcaseObjects[index].m_meshSimplify);
		meshSimplify.transform.position = this.ShowcaseObjects[index].m_position;
		meshSimplify.transform.rotation = Quaternion.Euler(this.ShowcaseObjects[index].m_angles);
		this.m_selectedMeshSimplify = meshSimplify;
		this.m_objectMaterials = new Dictionary<GameObject, Material[]>();
		this.AddMaterials(meshSimplify.gameObject, this.m_objectMaterials);
		this.m_bWireframe = false;
	}

	private void AddMaterials(GameObject theGameObject, Dictionary<GameObject, Material[]> dicMaterials)
	{
		Renderer component = theGameObject.GetComponent<Renderer>();
		MeshSimplify component2 = theGameObject.GetComponent<MeshSimplify>();
		if (component != null && component.sharedMaterials != null && component2 != null)
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

	private IEnumerator ComputeMeshWithVertices(float fAmount)
	{
		foreach (KeyValuePair<GameObject, Material[]> pair in this.m_objectMaterials)
		{
			MeshSimplify meshSimplify = pair.Key.GetComponent<MeshSimplify>();
			MeshFilter meshFilter = pair.Key.GetComponent<MeshFilter>();
			SkinnedMeshRenderer skin = pair.Key.GetComponent<SkinnedMeshRenderer>();
			if (meshSimplify && (meshFilter != null || skin != null))
			{
				Mesh newMesh = null;
				if (meshFilter != null)
				{
					newMesh = Object.Instantiate<Mesh>(meshFilter.sharedMesh);
				}
				else if (skin != null)
				{
					newMesh = Object.Instantiate<Mesh>(skin.sharedMesh);
				}
				if (meshSimplify.GetMeshSimplifier() != null)
				{
					meshSimplify.GetMeshSimplifier().CoroutineEnded = false;
					base.StartCoroutine(meshSimplify.GetMeshSimplifier().ComputeMeshWithVertexCount(pair.Key, newMesh, Mathf.RoundToInt(fAmount * (float)meshSimplify.GetMeshSimplifier().GetOriginalMeshUniqueVertexCount()), meshSimplify.name, new Simplifier.ProgressDelegate(this.Progress)));
					while (!meshSimplify.GetMeshSimplifier().CoroutineEnded)
					{
						yield return null;
					}
					if (meshFilter != null)
					{
						meshFilter.mesh = newMesh;
					}
					else if (skin != null)
					{
						skin.sharedMesh = newMesh;
					}
					meshSimplify.m_simplifiedMesh = newMesh;
				}
			}
		}
		this.m_strLastTitle = string.Empty;
		this.m_strLastMessage = string.Empty;
		this.m_nLastProgress = 0;
		yield break;
	}

	public SimplifyMeshPreview.ShowcaseObject[] ShowcaseObjects;

	public Material WireframeMaterial;

	public float MouseSensitvity = 0.3f;

	public float MouseReleaseSpeed = 3f;

	private Dictionary<GameObject, Material[]> m_objectMaterials;

	private MeshSimplify m_selectedMeshSimplify;

	private int m_nSelectedIndex = -1;

	private bool m_bWireframe;

	private float m_fRotationSpeed = 10f;

	private float m_fLastMouseX;

	private Mesh m_newMesh;

	private int m_nLastProgress = -1;

	private string m_strLastTitle = string.Empty;

	private string m_strLastMessage = string.Empty;

	private float m_fVertexAmount = 1f;

	private bool m_bGUIEnabled = true;

	[Serializable]
	public class ShowcaseObject
	{
		public MeshSimplify m_meshSimplify;

		public Vector3 m_position;

		public Vector3 m_angles;

		public Vector3 m_rotationAxis = Vector3.up;

		public string m_description;
	}
}
