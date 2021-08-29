using System;
using System.Collections;
using System.Collections.Generic;
using UltimateGameTools.MeshSimplifier;
using UnityEngine;

public class MeshSimplify : MonoBehaviour
{
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

	public static bool IsRootOrBelongsToTree(MeshSimplify meshSimplify, MeshSimplify root)
	{
		return !(meshSimplify == null) && !meshSimplify.m_bExcludedFromTree && (meshSimplify.m_meshSimplifyRoot == null || meshSimplify.m_meshSimplifyRoot == root || meshSimplify == root || meshSimplify.m_meshSimplifyRoot == root.m_meshSimplifyRoot);
	}

	public bool IsGenerateIncludeChildrenActive()
	{
		return this.m_bGenerateIncludeChildren;
	}

	public bool HasDependentChildren()
	{
		return this.m_listDependentChildren != null && this.m_listDependentChildren.Count > 0;
	}

	public bool HasDataDirty()
	{
		return this.m_bDataDirty;
	}

	public bool SetDataDirty(bool bDirty)
	{
		this.m_bDataDirty = bDirty;
		return bDirty;
	}

	public bool HasNonMeshSimplifyGameObjectsInTree()
	{
		return this.HasNonMeshSimplifyGameObjectsInTreeRecursive(this, base.gameObject);
	}

	private bool HasNonMeshSimplifyGameObjectsInTreeRecursive(MeshSimplify root, GameObject gameObject)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component == null && MeshSimplify.HasValidMeshData(gameObject))
		{
			return true;
		}
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			if (this.HasNonMeshSimplifyGameObjectsInTreeRecursive(root, gameObject.transform.GetChild(i).gameObject))
			{
				return true;
			}
		}
		return false;
	}

	private void ConfigureSimplifier()
	{
		this.m_meshSimplifier.UseEdgeLength = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bUseEdgeLength : this.m_meshSimplifyRoot.m_bUseEdgeLength);
		this.m_meshSimplifier.UseCurvature = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bUseCurvature : this.m_meshSimplifyRoot.m_bUseCurvature);
		this.m_meshSimplifier.ProtectTexture = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bProtectTexture : this.m_meshSimplifyRoot.m_bProtectTexture);
		this.m_meshSimplifier.LockBorder = ((!(this.m_meshSimplifyRoot != null) || this.m_bOverrideRootSettings) ? this.m_bLockBorder : this.m_meshSimplifyRoot.m_bLockBorder);
	}

	public Simplifier GetMeshSimplifier()
	{
		return this.m_meshSimplifier;
	}

	public void ComputeData(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify.ComputeDataRecursive(this, base.gameObject, bRecurseIntoChildren, progress);
	}

	private static void ComputeDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify meshSimplify = gameObject.GetComponent<MeshSimplify>();
		if (meshSimplify == null && root.m_bGenerateIncludeChildren && MeshSimplify.HasValidMeshData(gameObject))
		{
			meshSimplify = gameObject.AddComponent<MeshSimplify>();
			meshSimplify.m_meshSimplifyRoot = root;
			root.m_listDependentChildren.Add(meshSimplify);
		}
		if (meshSimplify != null && MeshSimplify.IsRootOrBelongsToTree(meshSimplify, root))
		{
			meshSimplify.FreeData(false);
			MeshFilter component = meshSimplify.GetComponent<MeshFilter>();
			if (component != null && component.sharedMesh != null)
			{
				if (component.sharedMesh.vertexCount > 0)
				{
					if (meshSimplify.m_originalMesh == null)
					{
						meshSimplify.m_originalMesh = component.sharedMesh;
					}
					Simplifier[] components = meshSimplify.GetComponents<Simplifier>();
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
					meshSimplify.m_meshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
					meshSimplify.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
					meshSimplify.ConfigureSimplifier();
					IEnumerator enumerator = meshSimplify.m_meshSimplifier.ProgressiveMesh(gameObject, meshSimplify.m_originalMesh, root.m_aRelevanceSpheres, meshSimplify.name, progress);
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
			}
			else
			{
				SkinnedMeshRenderer component2 = meshSimplify.GetComponent<SkinnedMeshRenderer>();
				if (component2 != null && component2.sharedMesh.vertexCount > 0)
				{
					if (meshSimplify.m_originalMesh == null)
					{
						meshSimplify.m_originalMesh = component2.sharedMesh;
					}
					Simplifier[] components2 = meshSimplify.GetComponents<Simplifier>();
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
					meshSimplify.m_meshSimplifier = meshSimplify.gameObject.AddComponent<Simplifier>();
					meshSimplify.m_meshSimplifier.hideFlags = HideFlags.HideInInspector;
					meshSimplify.ConfigureSimplifier();
					IEnumerator enumerator2 = meshSimplify.m_meshSimplifier.ProgressiveMesh(gameObject, meshSimplify.m_originalMesh, root.m_aRelevanceSpheres, meshSimplify.name, progress);
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
			meshSimplify.m_bDataDirty = false;
		}
		if (bRecurseIntoChildren)
		{
			for (int k = 0; k < gameObject.transform.childCount; k++)
			{
				MeshSimplify.ComputeDataRecursive(root, gameObject.transform.GetChild(k).gameObject, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	public bool HasData()
	{
		return (this.m_meshSimplifier != null && this.m_simplifiedMesh != null) || (this.m_listDependentChildren != null && this.m_listDependentChildren.Count != 0);
	}

	public bool HasSimplifiedMesh()
	{
		return this.m_simplifiedMesh != null && this.m_simplifiedMesh.vertexCount > 0;
	}

	public void ComputeMesh(bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify.ComputeMeshRecursive(this, base.gameObject, bRecurseIntoChildren, progress);
	}

	private static void ComputeMeshRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren, Simplifier.ProgressDelegate progress = null)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_meshSimplifier != null)
		{
			if (component.m_simplifiedMesh)
			{
				component.m_simplifiedMesh.Clear();
			}
			float fVertexAmount = component.m_fVertexAmount;
			if (!component.m_bOverrideRootSettings && component.m_meshSimplifyRoot != null)
			{
				fVertexAmount = component.m_meshSimplifyRoot.m_fVertexAmount;
			}
			if (component.m_simplifiedMesh == null)
			{
				component.m_simplifiedMesh = MeshSimplify.CreateNewEmptyMesh(component);
			}
			component.ConfigureSimplifier();
			IEnumerator enumerator = component.m_meshSimplifier.ComputeMeshWithVertexCount(gameObject, component.m_simplifiedMesh, Mathf.RoundToInt(fVertexAmount * (float)component.m_meshSimplifier.GetOriginalMeshUniqueVertexCount()), component.name + " Simplified", progress);
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
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.ComputeMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren, progress);
				if (Simplifier.Cancelled)
				{
					return;
				}
			}
		}
	}

	public void AssignSimplifiedMesh(bool bRecurseIntoChildren)
	{
		MeshSimplify.AssignSimplifiedMeshRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static void AssignSimplifiedMeshRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh != null)
		{
			MeshFilter component2 = component.GetComponent<MeshFilter>();
			if (component2 != null)
			{
				component2.sharedMesh = component.m_simplifiedMesh;
			}
			else
			{
				SkinnedMeshRenderer component3 = component.GetComponent<SkinnedMeshRenderer>();
				if (component3 != null)
				{
					component3.sharedMesh = component.m_simplifiedMesh;
				}
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.AssignSimplifiedMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren);
			}
		}
	}

	public void RestoreOriginalMesh(bool bDeleteData, bool bRecurseIntoChildren)
	{
		MeshSimplify.RestoreOriginalMeshRecursive(this, base.gameObject, bDeleteData, bRecurseIntoChildren);
	}

	private static void RestoreOriginalMeshRecursive(MeshSimplify root, GameObject gameObject, bool bDeleteData, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
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
			if (bDeleteData)
			{
				component.FreeData(false);
				component.m_listDependentChildren.Clear();
			}
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.RestoreOriginalMeshRecursive(root, gameObject.transform.GetChild(i).gameObject, bDeleteData, bRecurseIntoChildren);
			}
		}
	}

	public bool HasOriginalMeshActive(bool bRecurseIntoChildren)
	{
		return MeshSimplify.HasOriginalMeshActiveRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static bool HasOriginalMeshActiveRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		bool flag = false;
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_originalMesh != null)
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
				flag = (flag || MeshSimplify.HasOriginalMeshActiveRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren));
			}
		}
		return flag;
	}

	public bool HasVertexData(bool bRecurseIntoChildren)
	{
		return MeshSimplify.HasVertexDataRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static bool HasVertexDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh && component.m_simplifiedMesh.vertexCount > 0)
		{
			return true;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				if (MeshSimplify.HasVertexDataRecursive(root, gameObject.transform.GetChild(i).gameObject, bRecurseIntoChildren))
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
		MeshSimplify.GetOriginalVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	private static void GetOriginalVertexCountRecursive(MeshSimplify root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_originalMesh != null)
		{
			nVertexCount += component.m_originalMesh.vertexCount;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetOriginalVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	public int GetSimplifiedVertexCount(bool bRecurseIntoChildren)
	{
		int result = 0;
		MeshSimplify.GetSimplifiedVertexCountRecursive(this, base.gameObject, ref result, bRecurseIntoChildren);
		return result;
	}

	private static void GetSimplifiedVertexCountRecursive(MeshSimplify root, GameObject gameObject, ref int nVertexCount, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root) && component.m_simplifiedMesh != null)
		{
			nVertexCount += component.m_simplifiedMesh.vertexCount;
		}
		if (bRecurseIntoChildren)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				MeshSimplify.GetSimplifiedVertexCountRecursive(root, gameObject.transform.GetChild(i).gameObject, ref nVertexCount, bRecurseIntoChildren);
			}
		}
	}

	public void RemoveFromTree()
	{
		if (this.m_meshSimplifyRoot != null)
		{
			this.m_meshSimplifyRoot.m_listDependentChildren.Remove(this);
		}
		this.RestoreOriginalMesh(true, false);
		this.m_bExcludedFromTree = true;
	}

	public void FreeData(bool bRecurseIntoChildren)
	{
		MeshSimplify.FreeDataRecursive(this, base.gameObject, bRecurseIntoChildren);
	}

	private static void FreeDataRecursive(MeshSimplify root, GameObject gameObject, bool bRecurseIntoChildren)
	{
		MeshSimplify component = gameObject.GetComponent<MeshSimplify>();
		if (component != null && MeshSimplify.IsRootOrBelongsToTree(component, root))
		{
			if (component.m_simplifiedMesh)
			{
				component.m_simplifiedMesh.Clear();
			}
			Simplifier[] components = gameObject.GetComponents<Simplifier>();
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
			component.m_bDataDirty = true;
		}
		if (bRecurseIntoChildren)
		{
			for (int j = 0; j < gameObject.transform.childCount; j++)
			{
				MeshSimplify.FreeDataRecursive(root, gameObject.transform.GetChild(j).gameObject, bRecurseIntoChildren);
			}
		}
	}

	private static Mesh CreateNewEmptyMesh(MeshSimplify meshSimplify)
	{
		if (meshSimplify.m_originalMesh == null)
		{
			return new Mesh();
		}
		Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(meshSimplify.m_originalMesh);
		mesh.Clear();
		return mesh;
	}

	[HideInInspector]
	public Mesh m_originalMesh;

	[HideInInspector]
	public Mesh m_simplifiedMesh;

	[HideInInspector]
	public bool m_bEnablePrefabUsage;

	[HideInInspector]
	public float m_fVertexAmount = 1f;

	[HideInInspector]
	public string m_strAssetPath;

	[HideInInspector]
	public MeshSimplify m_meshSimplifyRoot;

	[HideInInspector]
	public List<MeshSimplify> m_listDependentChildren = new List<MeshSimplify>();

	[HideInInspector]
	public bool m_bExpandRelevanceSpheres = true;

	public RelevanceSphere[] m_aRelevanceSpheres;

	[SerializeField]
	[HideInInspector]
	private Simplifier m_meshSimplifier;

	[SerializeField]
	[HideInInspector]
	private bool m_bGenerateIncludeChildren = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bOverrideRootSettings;

	[SerializeField]
	[HideInInspector]
	private bool m_bUseEdgeLength = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bUseCurvature = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bProtectTexture = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bLockBorder = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bDataDirty = true;

	[SerializeField]
	[HideInInspector]
	private bool m_bExcludedFromTree;
}
