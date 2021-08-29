using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

public class AutomaticLOD : MonoBehaviour
{
	private void Awake()
	{
		if (this.m_originalMesh)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				component.sharedMesh = this.m_originalMesh;
			}
			else
			{
				SkinnedMeshRenderer component2 = base.GetComponent<SkinnedMeshRenderer>();
				if (component2 != null)
				{
					component2.sharedMesh = this.m_originalMesh;
				}
			}
		}
		this.m_localCenter = base.transform.InverseTransformPoint(this.ComputeWorldCenter());
		this.m_cachedFrameLODLevel = new Dictionary<Camera, int>();
		this.b_performCheck = false;
		this.m_frameToCheck = -1;
		if (!this.IsDependent())
		{
			this.m_frameToCheck = AutomaticLOD.s_currentFrameCheckIndex;
			AutomaticLOD.s_currentCheckIndex++;
			if (AutomaticLOD.s_currentCheckIndex >= 4)
			{
				AutomaticLOD.s_currentCheckIndex = 0;
				AutomaticLOD.s_currentFrameCheckIndex++;
				if (AutomaticLOD.s_currentFrameCheckIndex >= 100)
				{
					AutomaticLOD.s_currentFrameCheckIndex = 0;
				}
				AutomaticLOD.s_checkLoopLength = Mathf.Min(AutomaticLOD.s_checkLoopLength + 1, 100);
			}
		}
		AutomaticLOD.s_lastFrameComputedModulus = 0;
		AutomaticLOD.s_currentFrameInLoop = 0;
	}

	private void Update()
	{
		if (Time.frameCount != AutomaticLOD.s_lastFrameComputedModulus && AutomaticLOD.s_checkLoopLength > 0)
		{
			AutomaticLOD.s_currentFrameInLoop = Time.frameCount % AutomaticLOD.s_checkLoopLength;
			AutomaticLOD.s_lastFrameComputedModulus = Time.frameCount;
		}
		if (!this.IsDependent())
		{
			if (AutomaticLOD.s_currentFrameInLoop == this.m_frameToCheck)
			{
				this.b_performCheck = true;
				this.m_cachedFrameLODLevel.Clear();
			}
			else
			{
				this.b_performCheck = false;
			}
		}
	}

	private void OnWillRenderObject()
	{
		if (!this.m_bUseAutomaticCameraLODSwitch)
		{
			return;
		}
		if (this.IsDependent())
		{
			bool flag = (!(this.m_LODObjectRootPersist != null)) ? this.m_LODObjectRoot.b_performCheck : this.m_LODObjectRootPersist.b_performCheck;
			if (flag)
			{
				int num = (!(this.m_LODObjectRootPersist != null)) ? this.m_LODObjectRoot.GetLODLevelUsingCamera(Camera.current) : this.m_LODObjectRootPersist.GetLODLevelUsingCamera(Camera.current);
				if (num != -1)
				{
					this.SwitchToLOD(num, false);
				}
			}
		}
		else if (this.m_originalMesh && this.b_performCheck)
		{
			int lodlevelUsingCamera = this.GetLODLevelUsingCamera(Camera.current);
			if (lodlevelUsingCamera != -1)
			{
				this.SwitchToLOD(lodlevelUsingCamera, false);
			}
		}
	}

	public static bool HasValidMeshData(GameObject go)
	{
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component != null)
		{
			return true;
		}
		SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
		return component2 != null;
	}

	public static bool IsRootOrBelongsToLODTree(AutomaticLOD automaticLOD, AutomaticLOD root)
	{
		return !(automaticLOD == null) && (automaticLOD.m_LODObjectRoot == null || automaticLOD.m_LODObjectRoot == root || automaticLOD == root || automaticLOD.m_LODObjectRoot == root.m_LODObjectRoot);
	}

	public int GetNumberOfLevelsToGenerate()
	{
		return (int)this.m_levelsToGenerate;
	}

	public bool IsGenerateIncludeChildrenActive()
	{
		return this.m_bGenerateIncludeChildren;
	}

	public bool IsRootAutomaticLOD()
	{
		return this.m_LODObjectRoot == null;
	}

	public bool HasDependentChildren()
	{
		return this.m_listDependentChildren != null && this.m_listDependentChildren.Count > 0;
	}

	public bool HasLODDataDirty()
	{
		return this.m_bLODDataDirty;
	}

	public bool SetLODDataDirty(bool bDirty)
	{
		this.m_bLODDataDirty = bDirty;
		return bDirty;
	}

	public int GetLODLevelCount()
	{
		return (this.m_listLODLevels == null) ? 0 : this.m_listLODLevels.Count;
	}

	public float ComputeScreenCoverage(Camera camera)
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		if (this.m_originalMesh)
		{
			if (this._corners == null)
			{
				this._corners = new Vector3[8];
			}
			this.BuildCornerData(ref this._corners, this.m_originalMesh.bounds);
			for (int i = 0; i < this._corners.Length; i++)
			{
				Vector3 vector = camera.WorldToViewportPoint(base.transform.TransformPoint(this._corners[i]));
				if (vector.x < num)
				{
					num = vector.x;
				}
				if (vector.y < num2)
				{
					num2 = vector.y;
				}
				if (vector.x > num3)
				{
					num3 = vector.x;
				}
				if (vector.y > num4)
				{
					num4 = vector.y;
				}
			}
		}
		for (int j = 0; j < this.m_listDependentChildren.Count; j++)
		{
			if (this.m_listDependentChildren[j] != null && this.m_listDependentChildren[j].m_originalMesh)
			{
				if (this.m_listDependentChildren[j]._corners == null)
				{
					this.m_listDependentChildren[j]._corners = new Vector3[8];
				}
				this.BuildCornerData(ref this.m_listDependentChildren[j]._corners, this.m_listDependentChildren[j].m_originalMesh.bounds);
				for (int k = 0; k < this.m_listDependentChildren[j]._corners.Length; k++)
				{
					Vector3 vector2 = camera.WorldToViewportPoint(this.m_listDependentChildren[j].transform.TransformPoint(this.m_listDependentChildren[j]._corners[k]));
					if (vector2.x < num)
					{
						num = vector2.x;
					}
					if (vector2.y < num2)
					{
						num2 = vector2.y;
					}
					if (vector2.x > num3)
					{
						num3 = vector2.x;
					}
					if (vector2.y > num4)
					{
						num4 = vector2.y;
					}
				}
			}
		}
		return (num3 - num) * (num4 - num2);
	}

	public Vector3 ComputeWorldCenter()
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		float num5 = float.MinValue;
		float num6 = float.MinValue;
		if (this.m_originalMesh)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector = (i != 0) ? base.GetComponent<Renderer>().bounds.max : base.GetComponent<Renderer>().bounds.min;
				if (vector.x < num)
				{
					num = vector.x;
				}
				if (vector.y < num2)
				{
					num2 = vector.y;
				}
				if (vector.z < num3)
				{
					num3 = vector.z;
				}
				if (vector.x > num4)
				{
					num4 = vector.x;
				}
				if (vector.y > num5)
				{
					num5 = vector.y;
				}
				if (vector.z > num6)
				{
					num6 = vector.z;
				}
			}
		}
		for (int j = 0; j < this.m_listDependentChildren.Count; j++)
		{
			if (this.m_listDependentChildren[j] != null && this.m_listDependentChildren[j].m_originalMesh)
			{
				for (int k = 0; k < 2; k++)
				{
					Vector3 vector2 = (k != 0) ? this.m_listDependentChildren[j].GetComponent<Renderer>().bounds.max : this.m_listDependentChildren[j].GetComponent<Renderer>().bounds.min;
					if (vector2.x < num)
					{
						num = vector2.x;
					}
					if (vector2.y < num2)
					{
						num2 = vector2.y;
					}
					if (vector2.z < num3)
					{
						num3 = vector2.z;
					}
					if (vector2.x > num4)
					{
						num4 = vector2.x;
					}
					if (vector2.y > num5)
					{
						num5 = vector2.y;
					}
					if (vector2.z > num6)
					{
						num6 = vector2.z;
					}
				}
			}
		}
		Vector3 b = new Vector3(num, num2, num3);
		Vector3 a = new Vector3(num4, num5, num6);
		return (a + b) * 0.5f;
	}

	public float ComputeViewSpaceBounds(Vector3 v3CameraPos, Vector3 v3CameraDir, Vector3 v3CameraUp, out Vector3 v3Min, out Vector3 v3Max, out Vector3 v3Center)
	{
		Matrix4x4 matrix4x = Matrix4x4.TRS(v3CameraPos, Quaternion.LookRotation(v3CameraDir, v3CameraUp), Vector3.one);
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		float num5 = float.MinValue;
		float num6 = float.MinValue;
		v3Center = matrix4x.inverse.MultiplyPoint(base.transform.TransformPoint(Vector3.zero));
		if (this.m_originalMesh)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector3 point = (i != 0) ? base.GetComponent<Renderer>().bounds.max : base.GetComponent<Renderer>().bounds.min;
				Vector3 vector = matrix4x.inverse.MultiplyPoint(point);
				if (vector.x < num)
				{
					num = vector.x;
				}
				if (vector.y < num2)
				{
					num2 = vector.y;
				}
				if (vector.z < num3)
				{
					num3 = vector.z;
				}
				if (vector.x > num4)
				{
					num4 = vector.x;
				}
				if (vector.y > num5)
				{
					num5 = vector.y;
				}
				if (vector.z > num6)
				{
					num6 = vector.z;
				}
			}
		}
		for (int j = 0; j < this.m_listDependentChildren.Count; j++)
		{
			if (this.m_listDependentChildren[j] != null && this.m_listDependentChildren[j].m_originalMesh)
			{
				for (int k = 0; k < 2; k++)
				{
					Vector3 point2 = (k != 0) ? this.m_listDependentChildren[j].GetComponent<Renderer>().bounds.max : this.m_listDependentChildren[j].GetComponent<Renderer>().bounds.min;
					Vector3 vector2 = matrix4x.inverse.MultiplyPoint(point2);
					if (vector2.x < num)
					{
						num = vector2.x;
					}
					if (vector2.y < num2)
					{
						num2 = vector2.y;
					}
					if (vector2.z < num3)
					{
						num3 = vector2.z;
					}
					if (vector2.x > num4)
					{
						num4 = vector2.x;
					}
					if (vector2.y > num5)
					{
						num5 = vector2.y;
					}
					if (vector2.z > num6)
					{
						num6 = vector2.z;
					}
				}
			}
		}
		v3Min = new Vector3(num, num2, num3);
		v3Max = new Vector3(num4, num5, num6);
		return (num4 - num) * (num5 - num2);
	}

	public void SetAutomaticCameraLODSwitch(bool bEnabled)
	{
		AutomaticLOD.SetAutomaticCameraLODSwitchRecursive(this, base.gameObject, bEnabled);
	}

	private static void SetAutomaticCameraLODSwitchRecursive(AutomaticLOD root, GameObject gameObject, bool bEnabled)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			component.m_bUseAutomaticCameraLODSwitch = bEnabled;
		}
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			AutomaticLOD.SetAutomaticCameraLODSwitchRecursive(root, gameObject.transform.GetChild(i).gameObject, bEnabled);
		}
	}

	public void SetLODLevels(List<AutomaticLOD.LODLevelData> listLODLevelData, AutomaticLOD.EvalMode evalMode, float fMaxCameraDistance, bool bRecurseIntoChildren)
	{
		this.m_listLODLevels = listLODLevelData;
		this.m_fMaxCameraDistance = fMaxCameraDistance;
		this.m_nColorEditorBarNewIndex = listLODLevelData.Count;
		this.m_evalMode = evalMode;
		this.m_LODObjectRoot = null;
		this.m_LODObjectRootPersist = null;
		this.m_listDependentChildren = new List<AutomaticLOD>();
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				AutomaticLOD.SetLODLevelsRecursive(this, base.transform.GetChild(i).gameObject);
			}
		}
	}

	private static void SetLODLevelsRecursive(AutomaticLOD root, GameObject gameObject)
	{
		AutomaticLOD automaticLOD = gameObject.GetComponent<AutomaticLOD>();
		bool flag = false;
		if (automaticLOD != null)
		{
			if (AutomaticLOD.IsRootOrBelongsToLODTree(automaticLOD, root))
			{
				flag = true;
			}
		}
		else if (AutomaticLOD.HasValidMeshData(gameObject))
		{
			automaticLOD = gameObject.AddComponent<AutomaticLOD>();
			flag = true;
		}
		if (flag && automaticLOD)
		{
			automaticLOD.m_fMaxCameraDistance = root.m_fMaxCameraDistance;
			automaticLOD.m_nColorEditorBarNewIndex = root.m_nColorEditorBarNewIndex;
			automaticLOD.m_evalMode = root.m_evalMode;
			automaticLOD.m_listLODLevels = new List<AutomaticLOD.LODLevelData>();
			automaticLOD.m_LODObjectRoot = root;
			automaticLOD.m_LODObjectRootPersist = root;
			root.m_listDependentChildren.Add(automaticLOD);
			for (int i = 0; i < root.m_listLODLevels.Count; i++)
			{
				automaticLOD.m_listLODLevels.Add(root.m_listLODLevels[i].GetCopy());
				automaticLOD.m_listLODLevels[i].m_mesh = AutomaticLOD.CreateNewEmptyMesh(automaticLOD);
			}
		}
		for (int j = 0; j < gameObject.transform.childCount; j++)
		{
			AutomaticLOD.SetLODLevelsRecursive(root, gameObject.transform.GetChild(j).gameObject);
		}
		if (flag && automaticLOD)
		{
			for (int k = 0; k < root.m_listLODLevels.Count; k++)
			{
				AutomaticLOD.CheckForLODGameObjectSetup(root, automaticLOD, automaticLOD.m_listLODLevels[k], k);
			}
		}
	}

	public void AddLODLevel(int nLevel, bool bBefore, bool bCreateMesh, bool bRecurseIntoChildren)
	{
		AutomaticLOD.AddLODLevelRecursive(this, base.gameObject, nLevel, bBefore, bCreateMesh, bRecurseIntoChildren);
	}

	public static void AddLODLevelRecursive(AutomaticLOD root, GameObject gameObject, int nLevel, bool bBefore, bool bCreateMesh, bool bRecurseIntoChildren)
	{
		if (Simplifier.Cancelled)
		{
			return;
		}
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			bool flag = true;
			if (component.m_listLODLevels == null)
			{
				flag = false;
			}
			else if (nLevel < 0 || nLevel >= component.m_listLODLevels.Count)
			{
				flag = false;
			}
			if (flag)
			{
				AutomaticLOD.LODLevelData lodlevelData = new AutomaticLOD.LODLevelData();
				lodlevelData.m_bUsesOriginalMesh = false;
				lodlevelData.m_gameObject = null;
				if (bBefore)
				{
					if (nLevel == 0)
					{
						lodlevelData.m_fScreenCoverage = component.m_listLODLevels[0].m_fScreenCoverage;
						lodlevelData.m_fMaxCameraDistance = component.m_listLODLevels[0].m_fMaxCameraDistance;
						lodlevelData.m_fMeshVerticesAmount = 1f;
						lodlevelData.m_nColorEditorBarIndex = component.m_nColorEditorBarNewIndex++;
						if (component.m_listLODLevels.Count > 1)
						{
							component.m_listLODLevels[0].m_fScreenCoverage = (component.m_listLODLevels[0].m_fScreenCoverage + component.m_listLODLevels[1].m_fScreenCoverage) / 2f;
							component.m_listLODLevels[0].m_fMaxCameraDistance = (component.m_listLODLevels[0].m_fMaxCameraDistance + component.m_listLODLevels[1].m_fMaxCameraDistance) / 2f;
						}
						else
						{
							component.m_listLODLevels[0].m_fScreenCoverage *= 0.5f;
							component.m_listLODLevels[0].m_fMaxCameraDistance *= 2f;
							if (Mathf.Approximately(component.m_listLODLevels[0].m_fMaxCameraDistance, 0f))
							{
								component.m_listLODLevels[0].m_fMaxCameraDistance = component.m_fMaxCameraDistance * 0.5f;
							}
						}
					}
					else
					{
						lodlevelData.m_fScreenCoverage = (component.m_listLODLevels[nLevel - 1].m_fScreenCoverage + component.m_listLODLevels[nLevel].m_fScreenCoverage) / 2f;
						lodlevelData.m_fMaxCameraDistance = (component.m_listLODLevels[nLevel - 1].m_fMaxCameraDistance + component.m_listLODLevels[nLevel].m_fMaxCameraDistance) / 2f;
						lodlevelData.m_fMeshVerticesAmount = (component.m_listLODLevels[nLevel - 1].m_fMeshVerticesAmount + component.m_listLODLevels[nLevel].m_fMeshVerticesAmount) / 2f;
						lodlevelData.m_nColorEditorBarIndex = component.m_nColorEditorBarNewIndex++;
					}
					if (bCreateMesh && lodlevelData.m_mesh == null)
					{
						lodlevelData.m_mesh = AutomaticLOD.CreateNewEmptyMesh(component);
					}
					component.m_listLODLevels.Insert(nLevel, lodlevelData);
					if (bCreateMesh)
					{
						AutomaticLOD.CheckForLODGameObjectSetup(root, component, lodlevelData, (nLevel != 0) ? (nLevel - 1) : 0);
					}
				}
				else
				{
					int num = component.m_listLODLevels.Count - 1;
					if (nLevel == num)
					{
						lodlevelData.m_fScreenCoverage = component.m_listLODLevels[num].m_fScreenCoverage * 0.5f;
						lodlevelData.m_fMaxCameraDistance = (component.m_listLODLevels[num].m_fMaxCameraDistance + component.m_fMaxCameraDistance) * 0.5f;
						lodlevelData.m_fMeshVerticesAmount = component.m_listLODLevels[num].m_fMeshVerticesAmount * 0.5f;
						lodlevelData.m_nColorEditorBarIndex = component.m_nColorEditorBarNewIndex++;
					}
					else
					{
						lodlevelData.m_fScreenCoverage = (component.m_listLODLevels[nLevel + 1].m_fScreenCoverage + component.m_listLODLevels[nLevel].m_fScreenCoverage) / 2f;
						lodlevelData.m_fMaxCameraDistance = (component.m_listLODLevels[nLevel + 1].m_fMaxCameraDistance + component.m_listLODLevels[nLevel].m_fMaxCameraDistance) / 2f;
						lodlevelData.m_fMeshVerticesAmount = (component.m_listLODLevels[nLevel + 1].m_fMeshVerticesAmount + component.m_listLODLevels[nLevel].m_fMeshVerticesAmount) / 2f;
						lodlevelData.m_nColorEditorBarIndex = component.m_nColorEditorBarNewIndex++;
					}
					if (bCreateMesh && lodlevelData.m_mesh == null)
					{
						lodlevelData.m_mesh = AutomaticLOD.CreateNewEmptyMesh(component);
					}
					if (nLevel == num)
					{
						component.m_listLODLevels.Add(lodlevelData);
					}
					else
					{
						component.m_listLODLevels.Insert(nLevel + 1, lodlevelData);
					}
					if (bCreateMesh)
					{
						AutomaticLOD.CheckForLODGameObjectSetup(root, component, lodlevelData, (nLevel != num) ? (nLevel + 1) : num);
					}
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				AutomaticLOD.AddLODLevelRecursive(root, gameObject.transform.GetChild(i).gameObject, nLevel, bBefore, bCreateMesh, bRecurseIntoChildren);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	public void RemoveLODLevel(int nLevel, bool bDeleteMesh, bool bRecurseIntoChildren)
	{
		AutomaticLOD.RemoveLODLevelRecursive(this, base.gameObject, nLevel, bDeleteMesh, bRecurseIntoChildren);
	}

	public static void RemoveLODLevelRecursive(AutomaticLOD root, GameObject gameObject, int nLevel, bool bDeleteMesh, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			bool flag = true;
			if (component.m_listLODLevels == null)
			{
				flag = false;
			}
			else if (nLevel < 0 || nLevel >= component.m_listLODLevels.Count || component.m_listLODLevels.Count == 1)
			{
				flag = false;
			}
			if (flag)
			{
				if (bDeleteMesh)
				{
					if (component.m_listLODLevels[nLevel].m_mesh != null)
					{
						component.m_listLODLevels[nLevel].m_mesh.Clear();
					}
					if (component.m_listLODLevels[nLevel].m_gameObject != null)
					{
						if (Application.isEditor && !Application.isPlaying)
						{
							UnityEngine.Object.DestroyImmediate(component.m_listLODLevels[nLevel].m_gameObject);
						}
						else
						{
							UnityEngine.Object.Destroy(component.m_listLODLevels[nLevel].m_gameObject);
						}
					}
				}
				if (nLevel == 0 && component.m_listLODLevels.Count > 1)
				{
					component.m_listLODLevels[1].m_fMaxCameraDistance = 0f;
					component.m_listLODLevels[1].m_fScreenCoverage = 1f;
				}
				component.m_listLODLevels.RemoveAt(nLevel);
			}
			for (int i = 0; i < component.m_listLODLevels.Count; i++)
			{
				if (component.m_listLODLevels[i].m_gameObject != null)
				{
					component.m_listLODLevels[i].m_gameObject.name = "LOD" + i;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				AutomaticLOD.RemoveLODLevelRecursive(root, gameObject.transform.GetChild(j).gameObject, nLevel, bDeleteMesh, bRecurseIntoChildren);
			}
		}
	}

	public Simplifier GetMeshSimplifier()
	{
		return this.m_meshSimplifier;
	}

	public void ComputeLODData(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		this.ComputeLODDataRecursive(this, base.gameObject, bRecurseIntoChildren, progress);
	}

	private void ComputeLODDataRecursive(AutomaticLOD root, GameObject gameObject, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		if (Simplifier.Cancelled)
		{
			return;
		}
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null)
		{
			if (AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
			{
				component.FreeLODData(false);
				MeshFilter component2 = component.GetComponent<MeshFilter>();
				if (component2 != null)
				{
					if (component.m_originalMesh == null)
					{
						component.m_originalMesh = component2.sharedMesh;
					}
					Simplifier[] components = component.GetComponents<Simplifier>();
					for (int i = 0; i < components.Length; i++)
					{
						if (Application.isEditor && !Application.isPlaying)
						{
							UnityEngine.Object.DestroyImmediate(components[i]);
						}
						else
						{
							UnityEngine.Object.Destroy(components[i]);
						}
					}
					component.m_meshSimplifier = component.gameObject.AddComponent<Simplifier>();
					component.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
					IEnumerator enumerator = component.m_meshSimplifier.ProgressiveMesh(gameObject, component.m_originalMesh, root.m_aRelevanceSpheres, component.name, progress);
					while (enumerator.MoveNext())
					{
						if (Simplifier.Cancelled)
						{
							return;
						}
					}
					if (Simplifier.Cancelled)
					{
						return;
					}
				}
				else
				{
					SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
					if (component3 != null)
					{
						if (component.m_originalMesh == null)
						{
							component.m_originalMesh = component3.sharedMesh;
						}
						Simplifier[] components2 = component.GetComponents<Simplifier>();
						for (int j = 0; j < components2.Length; j++)
						{
							if (Application.isEditor && !Application.isPlaying)
							{
								UnityEngine.Object.DestroyImmediate(components2[j]);
							}
							else
							{
								UnityEngine.Object.Destroy(components2[j]);
							}
						}
						component.m_meshSimplifier = component.gameObject.AddComponent<Simplifier>();
						component.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
						IEnumerator enumerator2 = component.m_meshSimplifier.ProgressiveMesh(gameObject, component.m_originalMesh, root.m_aRelevanceSpheres, component.name, progress);
						while (enumerator2.MoveNext())
						{
							if (Simplifier.Cancelled)
							{
								return;
							}
						}
						if (Simplifier.Cancelled)
						{
							return;
						}
					}
				}
			}
			for (int k = 0; k < component.m_listLODLevels.Count; k++)
			{
				component.m_listLODLevels[k].m_mesh = null;
			}
			component.m_bLODDataDirty = false;
		}
		if (bRecurseIntoChildren)
		{
			int num = 0;
			while (num < gameObject.transform.childCount && !Simplifier.Cancelled)
			{
				this.ComputeLODDataRecursive(root, gameObject.transform.GetChild(num).gameObject, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
				num++;
			}
		}
	}

	public bool HasLODData()
	{
		return this.m_meshSimplifier != null && this.m_listLODLevels != null && this.m_listLODLevels.Count > 0;
	}

	public int GetLODLevelUsingCamera(Camera currentCamera)
	{
		if (this.m_cachedFrameLODLevel.ContainsKey(currentCamera))
		{
			return this.m_cachedFrameLODLevel[currentCamera];
		}
		if (this.m_listLODLevels == null || this.m_listLODLevels.Count == 0)
		{
			return -1;
		}
		float num = 0f;
		float num2 = 0f;
		if (this.m_evalMode == AutomaticLOD.EvalMode.CameraDistance)
		{
			Vector3 a = base.transform.TransformPoint(this.m_localCenter.x, this.m_localCenter.y, this.m_localCenter.z);
			num = Vector3.Distance(a, currentCamera.transform.position);
		}
		else if (this.m_evalMode == AutomaticLOD.EvalMode.ScreenCoverage)
		{
			num2 = this.ComputeScreenCoverage(currentCamera);
		}
		int i;
		for (i = 0; i < this.m_listLODLevels.Count; i++)
		{
			if (i == this.m_listLODLevels.Count - 1)
			{
				break;
			}
			if (this.m_evalMode == AutomaticLOD.EvalMode.CameraDistance)
			{
				if (num < this.m_listLODLevels[i + 1].m_fMaxCameraDistance)
				{
					break;
				}
			}
			else if (this.m_evalMode == AutomaticLOD.EvalMode.ScreenCoverage && num2 > this.m_listLODLevels[i + 1].m_fScreenCoverage)
			{
				break;
			}
		}
		this.m_cachedFrameLODLevel.Add(currentCamera, i);
		return i;
	}

	public int GetCurrentLODLevel()
	{
		return this.m_nCurrentLOD;
	}

	public void SwitchToLOD(int nLevel, bool bRecurseIntoChildren)
	{
		AutomaticLOD.SwitchToLODRecursive(this, base.gameObject, nLevel, bRecurseIntoChildren);
	}

	private static void SwitchToLODRecursive(AutomaticLOD root, GameObject gameObject, int nLODLevel, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root) && nLODLevel >= 0 && nLODLevel < component.m_listLODLevels.Count)
		{
			if (root.m_switchMode == AutomaticLOD.SwitchMode.SwitchMesh)
			{
				MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
				if (component2 != null)
				{
					Mesh mesh = (!component.m_listLODLevels[nLODLevel].m_bUsesOriginalMesh) ? component.m_listLODLevels[nLODLevel].m_mesh : component.m_originalMesh;
					if (component2.sharedMesh != mesh)
					{
						component2.sharedMesh = mesh;
					}
				}
				else
				{
					SkinnedMeshRenderer component3 = gameObject.GetComponent<SkinnedMeshRenderer>();
					if (component3 != null)
					{
						Mesh mesh2 = (!component.m_listLODLevels[nLODLevel].m_bUsesOriginalMesh) ? component.m_listLODLevels[nLODLevel].m_mesh : component.m_originalMesh;
						if (component3.sharedMesh != mesh2)
						{
							if (mesh2 != null && mesh2.vertexCount == 0)
							{
								if (component3.sharedMesh != null)
								{
									component3.sharedMesh = null;
								}
							}
							else
							{
								component3.sharedMesh = mesh2;
							}
						}
					}
				}
			}
			else if (root.m_switchMode == AutomaticLOD.SwitchMode.SwitchGameObject)
			{
				gameObject.GetComponent<Renderer>().enabled = component.m_listLODLevels[nLODLevel].m_bUsesOriginalMesh;
				for (int i = 0; i < component.m_listLODLevels.Count; i++)
				{
					if (component.m_listLODLevels[i].m_gameObject != null)
					{
						component.m_listLODLevels[i].m_gameObject.SetActive(!component.m_listLODLevels[nLODLevel].m_bUsesOriginalMesh && i == nLODLevel && component.gameObject.activeSelf);
					}
				}
			}
			component.m_nCurrentLOD = nLODLevel;
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				AutomaticLOD.SwitchToLODRecursive(root, gameObject.transform.GetChild(j).gameObject, nLODLevel, true);
			}
		}
	}

	public void ComputeAllLODMeshes(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		if (this.m_listLODLevels != null)
		{
			for (int i = 0; i < this.m_listLODLevels.Count; i++)
			{
				AutomaticLOD.ComputeLODMeshRecursive(this, base.gameObject, i, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	public void ComputeLODMesh(int nLevel, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		AutomaticLOD.ComputeLODMeshRecursive(this, base.gameObject, nLevel, bRecurseIntoChildren, progress);
	}

	private static void ComputeLODMeshRecursive(AutomaticLOD root, GameObject gameObject, int nLevel, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		if (Simplifier.Cancelled)
		{
			return;
		}
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root) && component.m_meshSimplifier != null)
		{
			if (component.m_listLODLevels[nLevel].m_mesh)
			{
				component.m_listLODLevels[nLevel].m_mesh.Clear();
			}
			float fMeshVerticesAmount = component.m_listLODLevels[nLevel].m_fMeshVerticesAmount;
			if (!component.m_bOverrideRootSettings && component.m_LODObjectRoot != null)
			{
				fMeshVerticesAmount = component.m_LODObjectRoot.m_listLODLevels[nLevel].m_fMeshVerticesAmount;
			}
			if (component.m_listLODLevels[nLevel].m_mesh == null)
			{
				component.m_listLODLevels[nLevel].m_mesh = AutomaticLOD.CreateNewEmptyMesh(component);
			}
			AutomaticLOD.CheckForLODGameObjectSetup(root, component, component.m_listLODLevels[nLevel], nLevel);
			int num = Mathf.RoundToInt(fMeshVerticesAmount * (float)component.m_meshSimplifier.GetOriginalMeshUniqueVertexCount());
			if (num < component.m_meshSimplifier.GetOriginalMeshUniqueVertexCount())
			{
				component.m_listLODLevels[nLevel].m_bUsesOriginalMesh = false;
				IEnumerator enumerator = component.m_meshSimplifier.ComputeMeshWithVertexCount(gameObject, component.m_listLODLevels[nLevel].m_mesh, num, component.name + " LOD " + nLevel, progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						return;
					}
				}
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
			else
			{
				component.m_listLODLevels[nLevel].m_bUsesOriginalMesh = true;
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				AutomaticLOD.ComputeLODMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, nLevel, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	public void RestoreOriginalMesh(bool bDeleteLODData, bool bRecurseIntoChildren)
	{
		AutomaticLOD.RestoreOriginalMeshRecursive(this, base.gameObject, bDeleteLODData, bRecurseIntoChildren);
	}

	private static void RestoreOriginalMeshRecursive(AutomaticLOD root, GameObject gameObject, bool bDeleteLODData, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			if (component.m_originalMesh != null)
			{
				MeshFilter component2 = component.GetComponent<MeshFilter>();
				if (component2 != null)
				{
					component2.sharedMesh = component.m_originalMesh;
				}
				else
				{
					SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
					if (component3 != null)
					{
						component3.sharedMesh = component.m_originalMesh;
					}
				}
			}
			component.m_nCurrentLOD = -1;
			if (root.m_switchMode == AutomaticLOD.SwitchMode.SwitchGameObject)
			{
				gameObject.GetComponent<Renderer>().enabled = true;
				for (int i = 0; i < component.m_listLODLevels.Count; i++)
				{
					if (component.m_listLODLevels[i].m_gameObject != null)
					{
						component.m_listLODLevels[i].m_gameObject.SetActive(false);
					}
				}
			}
			if (bDeleteLODData)
			{
				component.FreeLODData(false);
				component.m_listLODLevels.Clear();
				component.m_listLODLevels = null;
				component.m_listDependentChildren.Clear();
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				AutomaticLOD.RestoreOriginalMeshRecursive(root, gameObject.transform.GetChild(j).gameObject, bDeleteLODData, bRecurseIntoChildren);
			}
		}
	}

	public bool HasOriginalMeshActive(bool bRecurseIntoChildren)
	{
		return AutomaticLOD.HasOriginalMeshActiveRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static bool HasOriginalMeshActiveRecursive(AutomaticLOD root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		bool flag = false;
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root) && component.m_originalMesh != null)
		{
			MeshFilter component2 = component.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				if (component2.sharedMesh == component.m_originalMesh)
				{
					flag = true;
				}
			}
			else
			{
				SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
				if (component3 != null && component3.sharedMesh == component.m_originalMesh)
				{
					flag = true;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				flag = (flag || AutomaticLOD.HasOriginalMeshActiveRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren));
			}
		}
		return flag;
	}

	public bool HasVertexData(int nLevel, bool bRecurseIntoChildren)
	{
		return AutomaticLOD.HasVertexDataRecursive(this, base.gameObject, nLevel, bRecurseIntoChildren);
	}

	private static bool HasVertexDataRecursive(AutomaticLOD root, GameObject gameObject, int nLevel, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			if (component.m_listLODLevels[nLevel].m_bUsesOriginalMesh)
			{
				if (component.m_originalMesh && component.m_originalMesh.vertexCount > 0)
				{
					return true;
				}
			}
			else if (component.m_listLODLevels[nLevel].m_mesh && component.m_listLODLevels[nLevel].m_mesh.vertexCount > 0)
			{
				return true;
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				if (AutomaticLOD.HasVertexDataRecursive(root, gameObject.transform.GetChild(i).gameObject, nLevel, bRecurseIntoChildren))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetOriginalVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		AutomaticLOD.GetOriginalVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	private static void GetOriginalVertexCountRecursive(AutomaticLOD root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root) && component.m_originalMesh != null)
		{
			nVertexCount += component.m_originalMesh.vertexCount;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				AutomaticLOD.GetOriginalVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	public int GetCurrentVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		AutomaticLOD.GetCurrentVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	private static void GetCurrentVertexCountRecursive(AutomaticLOD root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
			if (component2 != null && component2.sharedMesh != null)
			{
				nVertexCount += component2.sharedMesh.vertexCount;
			}
			else
			{
				SkinnedMeshRenderer component3 = gameObject.GetComponent<SkinnedMeshRenderer>();
				if (component3 != null && component3.sharedMesh != null)
				{
					nVertexCount += component3.sharedMesh.vertexCount;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				AutomaticLOD.GetCurrentVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	public int GetLODVertexCount(int nLevel, bool bRecurseIntoChildren)
	{
		int result = 0;
		AutomaticLOD.GetLODVertexCountRecursive(this, base.gameObject, nLevel, ref result, bRecurseIntoChildren);
		return result;
	}

	private static void GetLODVertexCountRecursive(AutomaticLOD root, GameObject gameObject, int nLevel, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			if (component.m_listLODLevels[nLevel].m_bUsesOriginalMesh && component.m_originalMesh != null)
			{
				nVertexCount += component.m_originalMesh.vertexCount;
			}
			else if (component.m_listLODLevels[nLevel].m_mesh != null)
			{
				nVertexCount += component.m_listLODLevels[nLevel].m_mesh.vertexCount;
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				AutomaticLOD.GetLODVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, nLevel, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	public void RemoveFromLODTree()
	{
		if (this.m_LODObjectRoot != null)
		{
			this.m_LODObjectRoot.m_listDependentChildren.Remove(this);
		}
		this.RestoreOriginalMesh(true, false);
	}

	public void FreeLODData(bool bRecurseIntoChildren)
	{
		AutomaticLOD.FreeLODDataRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static void FreeLODDataRecursive(AutomaticLOD root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		AutomaticLOD component = gameObject.GetComponent<AutomaticLOD>();
		if (component != null && AutomaticLOD.IsRootOrBelongsToLODTree(component, root))
		{
			if (component.m_listLODLevels != null)
			{
				foreach (AutomaticLOD.LODLevelData lodlevelData in component.m_listLODLevels)
				{
					if (lodlevelData.m_mesh)
					{
						lodlevelData.m_mesh.Clear();
					}
					if (lodlevelData.m_gameObject != null)
					{
						if (Application.isEditor && !Application.isPlaying)
						{
							UnityEngine.Object.DestroyImmediate(lodlevelData.m_gameObject);
						}
						else
						{
							UnityEngine.Object.Destroy(lodlevelData.m_gameObject);
						}
					}
					lodlevelData.m_bUsesOriginalMesh = false;
				}
			}
			Simplifier[] components = component.GetComponents<Simplifier>();
			for (int i = 0; i < components.Length; i++)
			{
				if (Application.isEditor && !Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(components[i]);
				}
				else
				{
					UnityEngine.Object.Destroy(components[i]);
				}
			}
			if (component.m_meshSimplifier != null)
			{
				component.m_meshSimplifier = null;
			}
			component.m_bLODDataDirty = true;
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				AutomaticLOD.FreeLODDataRecursive(root, gameObject.transform.GetChild(j).gameObject, bRecurseIntoChildren);
			}
		}
	}

	private static Mesh CreateNewEmptyMesh(AutomaticLOD automaticLOD)
	{
		if (automaticLOD.m_originalMesh == null)
		{
			return new Mesh();
		}
		Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(automaticLOD.m_originalMesh);
		mesh.Clear();
		return mesh;
	}

	private static GameObject CreateBasicObjectCopy(GameObject gameObject, Mesh mesh, Transform parent)
	{
		GameObject gameObject2 = new GameObject();
		gameObject2.layer = gameObject.layer;
		gameObject2.isStatic = gameObject.isStatic;
		gameObject2.tag = gameObject.tag;
		gameObject2.transform.parent = parent;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		foreach (Component component in gameObject.GetComponents<Component>())
		{
			if (component.GetType() == typeof(MeshRenderer) || component.GetType() == typeof(MeshFilter) || component.GetType() == typeof(SkinnedMeshRenderer))
			{
				AutomaticLOD.CopyComponent(component, gameObject2);
			}
		}
		MeshFilter component2 = gameObject2.GetComponent<MeshFilter>();
		if (component2 != null)
		{
			component2.sharedMesh = mesh;
		}
		else
		{
			SkinnedMeshRenderer component3 = gameObject2.GetComponent<SkinnedMeshRenderer>();
			if (component3 != null)
			{
				component3.sharedMesh = mesh;
			}
		}
		if (gameObject2.GetComponent<Renderer>() != null)
		{
			gameObject2.GetComponent<Renderer>().enabled = true;
		}
		return gameObject2;
	}

	private static void CheckForLODGameObjectSetup(AutomaticLOD root, AutomaticLOD automaticLOD, AutomaticLOD.LODLevelData levelData, int level)
	{
		if (root.m_switchMode == AutomaticLOD.SwitchMode.SwitchGameObject)
		{
			if (levelData.m_gameObject != null)
			{
				if (Application.isEditor && !Application.isPlaying)
				{
					UnityEngine.Object.DestroyImmediate(levelData.m_gameObject);
				}
				else
				{
					UnityEngine.Object.Destroy(levelData.m_gameObject);
				}
			}
			levelData.m_gameObject = AutomaticLOD.CreateBasicObjectCopy(automaticLOD.gameObject, levelData.m_mesh, automaticLOD.gameObject.transform);
			levelData.m_gameObject.SetActive(false);
			for (int i = 0; i < automaticLOD.m_listLODLevels.Count; i++)
			{
				if (automaticLOD.m_listLODLevels[i].m_gameObject != null)
				{
					automaticLOD.m_listLODLevels[i].m_gameObject.name = "LOD" + i;
					automaticLOD.m_listLODLevels[i].m_gameObject.transform.SetSiblingIndex(i);
				}
			}
		}
		else
		{
			levelData.m_gameObject = null;
		}
	}

	private static Component CopyComponent(Component original, GameObject destination)
	{
		Type type = original.GetType();
		Component component = destination.GetComponent(type);
		if (!component)
		{
			component = destination.AddComponent(type);
		}
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!fieldInfo.IsStatic)
			{
				fieldInfo.SetValue(component, fieldInfo.GetValue(original));
			}
		}
		PropertyInfo[] properties = type.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanWrite && propertyInfo.CanWrite && !(propertyInfo.Name == "mesh") && !(propertyInfo.Name == "sharedMesh") && !(propertyInfo.Name == "material") && !(propertyInfo.Name == "materials"))
			{
				propertyInfo.SetValue(component, propertyInfo.GetValue(original, null), null);
			}
		}
		return component;
	}

	private void BuildCornerData(ref Vector3[] av3Corners, Bounds bounds)
	{
		av3Corners[0].x = bounds.min.x;
		av3Corners[0].y = bounds.min.y;
		av3Corners[0].z = bounds.min.z;
		av3Corners[1].x = bounds.min.x;
		av3Corners[1].y = bounds.min.y;
		av3Corners[1].z = bounds.max.z;
		av3Corners[2].x = bounds.min.x;
		av3Corners[2].y = bounds.max.y;
		av3Corners[2].z = bounds.min.z;
		av3Corners[3].x = bounds.min.x;
		av3Corners[3].y = bounds.max.y;
		av3Corners[3].z = bounds.max.z;
		av3Corners[4].x = bounds.max.x;
		av3Corners[4].y = bounds.min.y;
		av3Corners[4].z = bounds.min.z;
		av3Corners[5].x = bounds.max.x;
		av3Corners[5].y = bounds.min.y;
		av3Corners[5].z = bounds.max.z;
		av3Corners[6].x = bounds.max.x;
		av3Corners[6].y = bounds.max.y;
		av3Corners[6].z = bounds.min.z;
		av3Corners[7].x = bounds.max.x;
		av3Corners[7].y = bounds.max.y;
		av3Corners[7].z = bounds.max.z;
	}

	private bool IsDependent()
	{
		return this.m_LODObjectRoot != null || this.m_LODObjectRootPersist != null;
	}

	[HideInInspector]
	public Mesh m_originalMesh;

	[HideInInspector]
	public AutomaticLOD.EvalMode m_evalMode = AutomaticLOD.EvalMode.ScreenCoverage;

	[HideInInspector]
	public bool m_bEnablePrefabUsage;

	[HideInInspector]
	public string m_strAssetPath;

	[HideInInspector]
	public float m_fMaxCameraDistance = 1000f;

	[HideInInspector]
	public int m_nColorEditorBarNewIndex;

	[HideInInspector]
	public List<AutomaticLOD.LODLevelData> m_listLODLevels;

	[HideInInspector]
	public AutomaticLOD m_LODObjectRoot;

	[HideInInspector]
	public List<AutomaticLOD> m_listDependentChildren = new List<AutomaticLOD>();

	public bool m_bExpandRelevanceSpheres = true;

	public RelevanceSphere[] m_aRelevanceSpheres;

	[SerializeField]
	private Simplifier m_meshSimplifier;

	[SerializeField]
	private bool m_bGenerateIncludeChildren = true;

	[SerializeField]
	private AutomaticLOD.LevelsToGenerate m_levelsToGenerate = AutomaticLOD.LevelsToGenerate._3;

	[SerializeField]
	private AutomaticLOD.SwitchMode m_switchMode;

	[SerializeField]
	private bool m_bOverrideRootSettings;

	[SerializeField]
	[HideInInspector]
	private bool m_bLODDataDirty = true;

	[SerializeField]
	[HideInInspector]
	private AutomaticLOD m_LODObjectRootPersist;

	private bool m_bUseAutomaticCameraLODSwitch = true;

	private int m_nCurrentLOD = -1;

	private Dictionary<Camera, int> m_cachedFrameLODLevel;

	private Vector3 m_localCenter;

	private Vector3[] _corners;

	private const int k_MaxLODChecksPerFrame = 4;

	private const int k_MaxFrameCheckLoop = 100;

	private static int s_currentCheckIndex;

	private static int s_currentFrameCheckIndex;

	private static int s_checkLoopLength;

	private static int s_lastFrameComputedModulus = -1;

	private static int s_currentFrameInLoop = -1;

	private int m_frameToCheck;

	private bool b_performCheck;

	[Serializable]
	public enum EvalMode
	{
		CameraDistance,
		ScreenCoverage
	}

	[Serializable]
	public enum LevelsToGenerate
	{
		_1 = 1,
		_2,
		_3,
		_4,
		_5,
		_6
	}

	[Serializable]
	public enum SwitchMode
	{
		SwitchMesh,
		SwitchGameObject
	}

	[Serializable]
	public class LODLevelData
	{
		public AutomaticLOD.LODLevelData GetCopy()
		{
			return new AutomaticLOD.LODLevelData
			{
				m_fScreenCoverage = this.m_fScreenCoverage,
				m_fMaxCameraDistance = this.m_fMaxCameraDistance,
				m_fMeshVerticesAmount = this.m_fMeshVerticesAmount,
				m_nColorEditorBarIndex = this.m_nColorEditorBarIndex,
				m_mesh = this.m_mesh,
				m_bUsesOriginalMesh = this.m_bUsesOriginalMesh
			};
		}

		public float m_fScreenCoverage;

		public float m_fMaxCameraDistance;

		public float m_fMeshVerticesAmount;

		public int m_nColorEditorBarIndex;

		public Mesh m_mesh;

		public bool m_bUsesOriginalMesh;

		public GameObject m_gameObject;
	}
}
