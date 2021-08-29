using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	public class Simplifier : MonoBehaviour
	{
		public static bool Cancelled { get; set; }

		public static int CoroutineFrameMiliseconds
		{
			get
			{
				return Simplifier.m_nCoroutineFrameMiliseconds;
			}
			set
			{
				Simplifier.m_nCoroutineFrameMiliseconds = value;
			}
		}

		public bool CoroutineEnded { get; set; }

		public bool UseEdgeLength
		{
			get
			{
				return this.m_bUseEdgeLength;
			}
			set
			{
				this.m_bUseEdgeLength = value;
			}
		}

		public bool UseCurvature
		{
			get
			{
				return this.m_bUseCurvature;
			}
			set
			{
				this.m_bUseCurvature = value;
			}
		}

		public bool ProtectTexture
		{
			get
			{
				return this.m_bProtectTexture;
			}
			set
			{
				this.m_bProtectTexture = value;
			}
		}

		public bool LockBorder
		{
			get
			{
				return this.m_bLockBorder;
			}
			set
			{
				this.m_bLockBorder = value;
			}
		}

		public IEnumerator ProgressiveMesh(GameObject gameObject, Mesh sourceMesh, RelevanceSphere[] aRelevanceSpheres, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			this.m_meshOriginal = sourceMesh;
			Vector3[] aVerticesWorld = Simplifier.GetWorldVertices(gameObject);
			if (aVerticesWorld == null)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			this.m_listVertexMap = new List<int>();
			this.m_listVertexPermutationBack = new List<int>();
			this.m_listVertices = new List<Simplifier.Vertex>();
			this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
			if (progress != null)
			{
				progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Building unique vertex data", 1f);
				if (Simplifier.Cancelled)
				{
					this.CoroutineEnded = true;
					yield break;
				}
			}
			this.m_meshUniqueVertices = new MeshUniqueVertices();
			this.m_meshUniqueVertices.BuildData(this.m_meshOriginal, aVerticesWorld);
			this.m_nOriginalMeshVertexCount = this.m_meshUniqueVertices.ListVertices.Count;
			this.m_fOriginalMeshSize = Mathf.Max(new float[]
			{
				this.m_meshOriginal.bounds.size.x,
				this.m_meshOriginal.bounds.size.y,
				this.m_meshOriginal.bounds.size.z
			});
			this.m_listHeap = new List<Simplifier.Vertex>(this.m_meshUniqueVertices.ListVertices.Count);
			for (int i = 0; i < this.m_meshUniqueVertices.ListVertices.Count; i++)
			{
				this.m_listVertexMap.Add(-1);
				this.m_listVertexPermutationBack.Add(-1);
			}
			Vector2[] av2Mapping = this.m_meshOriginal.uv;
			this.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
			for (int j = 0; j < this.m_meshOriginal.subMeshCount; j++)
			{
				int[] triangles = this.m_meshOriginal.GetTriangles(j);
				this.m_aListTriangles[j] = new Simplifier.TriangleList();
				this.AddFaceListSubMesh(j, this.m_meshUniqueVertices.SubmeshesFaceList[j].m_listIndices, triangles, av2Mapping);
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				IEnumerator enumerator = this.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
			}
			else
			{
				yield return base.StartCoroutine(this.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress));
			}
			int nVertices = this.m_listVertices.Count;
			Stopwatch sw = Stopwatch.StartNew();
			while (this.m_listVertices.Count > 0)
			{
				if (progress != null && (this.m_listVertices.Count & 255) == 0)
				{
					progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Collapsing edges", 1f - (float)this.m_listVertices.Count / (float)nVertices);
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
				Simplifier.Vertex mn = this.MinimumCostEdge();
				this.m_listVertexPermutationBack[this.m_listVertices.Count - 1] = mn.m_nID;
				this.m_listVertexMap[mn.m_nID] = ((mn.m_collapse == null) ? -1 : mn.m_collapse.m_nID);
				this.Collapse(mn, mn.m_collapse, true, gameObject.transform, aRelevanceSpheres);
			}
			this.m_listHeap.Clear();
			this.CoroutineEnded = true;
			yield break;
		}

		public IEnumerator ComputeMeshWithVertexCount(GameObject gameObject, Mesh meshOut, int nVertices, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			if (this.GetOriginalMeshUniqueVertexCount() == -1)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			if (nVertices < 3)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			if (nVertices >= this.GetOriginalMeshUniqueVertexCount())
			{
				meshOut.triangles = new int[0];
				meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
				meshOut.vertices = this.m_meshOriginal.vertices;
				meshOut.normals = this.m_meshOriginal.normals;
				meshOut.tangents = this.m_meshOriginal.tangents;
				meshOut.uv = this.m_meshOriginal.uv;
				meshOut.uv2 = this.m_meshOriginal.uv2;
				meshOut.colors32 = this.m_meshOriginal.colors32;
				meshOut.boneWeights = this.m_meshOriginal.boneWeights;
				meshOut.bindposes = this.m_meshOriginal.bindposes;
				meshOut.triangles = this.m_meshOriginal.triangles;
				meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
				for (int i = 0; i < this.m_meshOriginal.subMeshCount; i++)
				{
					meshOut.SetTriangles(this.m_meshOriginal.GetTriangles(i), i);
				}
				meshOut.name = gameObject.name + " simplified mesh";
				this.CoroutineEnded = true;
				yield break;
			}
			this.m_listVertices = new List<Simplifier.Vertex>();
			this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
			List<Simplifier.Vertex> listVertices = new List<Simplifier.Vertex>();
			this.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
			for (int j = 0; j < this.m_listVertices.Count; j++)
			{
				this.m_listVertices[j].m_collapse = ((this.m_listVertexMap[j] != -1) ? this.m_listVertices[this.m_listVertexMap[j]] : null);
				listVertices.Add(this.m_listVertices[this.m_listVertexPermutationBack[j]]);
			}
			Vector2[] av2Mapping = this.m_meshOriginal.uv;
			for (int k = 0; k < this.m_meshOriginal.subMeshCount; k++)
			{
				int[] triangles = this.m_meshOriginal.GetTriangles(k);
				this.m_aListTriangles[k] = new Simplifier.TriangleList();
				this.AddFaceListSubMesh(k, this.m_meshUniqueVertices.SubmeshesFaceList[k].m_listIndices, triangles, av2Mapping);
			}
			int nTotalVertices = listVertices.Count;
			Stopwatch sw = Stopwatch.StartNew();
			while (listVertices.Count > nVertices)
			{
				if (progress != null && nTotalVertices != nVertices && (listVertices.Count & 255) == 0)
				{
					float fT = 1f - (float)(listVertices.Count - nVertices) / (float)(nTotalVertices - nVertices);
					progress("Simplifying mesh: " + strProgressDisplayObjectName, "Collapsing edges", fT);
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
				Simplifier.Vertex mn = listVertices[listVertices.Count - 1];
				listVertices.RemoveAt(listVertices.Count - 1);
				this.Collapse(mn, mn.m_collapse, false, null, null);
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
			}
			Vector3[] av3Vertices = new Vector3[this.m_listVertices.Count];
			for (int l = 0; l < this.m_listVertices.Count; l++)
			{
				this.m_listVertices[l].m_nID = l;
				av3Vertices[l] = this.m_listVertices[l].m_v3Position;
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				IEnumerator enumerator = this.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
			}
			else
			{
				yield return base.StartCoroutine(this.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress));
			}
			this.CoroutineEnded = true;
			yield break;
		}

		public int GetOriginalMeshUniqueVertexCount()
		{
			return this.m_nOriginalMeshVertexCount;
		}

		public int GetOriginalMeshTriangleCount()
		{
			return this.m_meshOriginal.triangles.Length / 3;
		}

		public static Vector3[] GetWorldVertices(GameObject gameObject)
		{
			Vector3[] array = null;
			SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
			MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				if (component.sharedMesh == null)
				{
					return null;
				}
				array = component.sharedMesh.vertices;
				BoneWeight[] boneWeights = component.sharedMesh.boneWeights;
				Matrix4x4[] bindposes = component.sharedMesh.bindposes;
				Transform[] bones = component.bones;
				if (array == null || boneWeights == null || bindposes == null || bones == null)
				{
					return null;
				}
				if (boneWeights.Length == 0 || bindposes.Length == 0 || bones.Length == 0)
				{
					return null;
				}
				for (int i = 0; i < array.Length; i++)
				{
					BoneWeight boneWeight = boneWeights[i];
					Vector3 vector = Vector3.zero;
					if (Math.Abs(boneWeight.weight0) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex0].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex0].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight0;
					}
					if (Math.Abs(boneWeight.weight1) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex1].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex1].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight1;
					}
					if (Math.Abs(boneWeight.weight2) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex2].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex2].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight2;
					}
					if (Math.Abs(boneWeight.weight3) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex3].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex3].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight3;
					}
					array[i] = vector;
				}
			}
			else if (component2 != null)
			{
				if (component2.sharedMesh == null)
				{
					return null;
				}
				array = component2.sharedMesh.vertices;
				if (array == null)
				{
					return null;
				}
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = gameObject.transform.TransformPoint(array[j]);
				}
			}
			return array;
		}

		private IEnumerator ConsolidateMesh(GameObject gameObject, Mesh meshIn, Mesh meshOut, Simplifier.TriangleList[] aListTriangles, Vector3[] av3Vertices, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			Vector3[] av3NormalsIn = meshIn.normals;
			Vector4[] av4TangentsIn = meshIn.tangents;
			Vector2[] av2Mapping1In = meshIn.uv;
			Vector2[] av2Mapping2In = meshIn.uv2;
			Color[] acolColorsIn = meshIn.colors;
			Color32[] aColors32In = meshIn.colors32;
			List<List<int>> listlistIndicesOut = new List<List<int>>();
			List<Vector3> listVerticesOut = new List<Vector3>();
			List<Vector3> listNormalsOut = new List<Vector3>();
			List<Vector4> listTangentsOut = new List<Vector4>();
			List<Vector2> listMapping1Out = new List<Vector2>();
			List<Vector2> listMapping2Out = new List<Vector2>();
			List<Color32> listColors32Out = new List<Color32>();
			List<BoneWeight> listBoneWeightsOut = new List<BoneWeight>();
			Dictionary<Simplifier.VertexDataHash, int> dicVertexDataHash2Index = new Dictionary<Simplifier.VertexDataHash, int>(new Simplifier.VertexDataHashComparer());
			bool bUV = av2Mapping1In != null && av2Mapping1In.Length > 0;
			bool bUV2 = av2Mapping2In != null && av2Mapping2In.Length > 0;
			bool bNormal = av3NormalsIn != null && av3NormalsIn.Length > 0;
			bool bTangent = av4TangentsIn != null && av4TangentsIn.Length > 0;
			Stopwatch sw = Stopwatch.StartNew();
			for (int nSubMesh = 0; nSubMesh < aListTriangles.Length; nSubMesh++)
			{
				List<int> listIndicesOut = new List<int>();
				string strMesh = (aListTriangles.Length <= 1) ? "Consolidating mesh" : ("Consolidating submesh " + (nSubMesh + 1));
				for (int i = 0; i < aListTriangles[nSubMesh].m_listTriangles.Count; i++)
				{
					if (progress != null && (i & 255) == 0)
					{
						float fT = (aListTriangles[nSubMesh].m_listTriangles.Count != 1) ? ((float)i / (float)(aListTriangles[nSubMesh].m_listTriangles.Count - 1)) : 1f;
						progress("Simplifying mesh: " + strProgressDisplayObjectName, strMesh, fT);
						if (Simplifier.Cancelled)
						{
							yield break;
						}
					}
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						yield return null;
						sw = Stopwatch.StartNew();
					}
					for (int j = 0; j < 3; j++)
					{
						int num = aListTriangles[nSubMesh].m_listTriangles[i].IndicesUV[j];
						int num2 = aListTriangles[nSubMesh].m_listTriangles[i].Indices[j];
						bool flag = false;
						Vector3 v3Position = aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_v3Position;
						Vector3 vector = (!bNormal) ? Vector3.zero : av3NormalsIn[num2];
						Vector4 item = (!bTangent) ? Vector4.zero : av4TangentsIn[num2];
						Vector2 vector2 = (!bUV) ? Vector2.zero : av2Mapping1In[num];
						Vector2 vector3 = (!bUV2) ? Vector2.zero : av2Mapping2In[num2];
						Color32 color = new Color32(0, 0, 0, 0);
						if (acolColorsIn != null && acolColorsIn.Length > 0)
						{
							color = acolColorsIn[num2];
							flag = true;
						}
						else if (aColors32In != null && aColors32In.Length > 0)
						{
							color = aColors32In[num2];
							flag = true;
						}
						Simplifier.VertexDataHash vertexDataHash = new Simplifier.VertexDataHash(v3Position, vector, vector2, vector3, color);
						if (dicVertexDataHash2Index.ContainsKey(vertexDataHash))
						{
							listIndicesOut.Add(dicVertexDataHash2Index[vertexDataHash]);
						}
						else
						{
							dicVertexDataHash2Index.Add(vertexDataHash, listVerticesOut.Count);
							listVerticesOut.Add(vertexDataHash.Vertex);
							if (bNormal)
							{
								listNormalsOut.Add(vector);
							}
							if (bUV)
							{
								listMapping1Out.Add(vector2);
							}
							if (bUV2)
							{
								listMapping2Out.Add(vector3);
							}
							if (bTangent)
							{
								listTangentsOut.Add(item);
							}
							if (flag)
							{
								listColors32Out.Add(color);
							}
							if (aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_bHasBoneWeight)
							{
								listBoneWeightsOut.Add(aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_boneWeight);
							}
							listIndicesOut.Add(listVerticesOut.Count - 1);
						}
					}
				}
				listlistIndicesOut.Add(listIndicesOut);
			}
			meshOut.triangles = new int[0];
			meshOut.vertices = listVerticesOut.ToArray();
			meshOut.normals = ((listNormalsOut.Count <= 0) ? null : listNormalsOut.ToArray());
			meshOut.tangents = ((listTangentsOut.Count <= 0) ? null : listTangentsOut.ToArray());
			meshOut.uv = ((listMapping1Out.Count <= 0) ? null : listMapping1Out.ToArray());
			meshOut.uv2 = ((listMapping2Out.Count <= 0) ? null : listMapping2Out.ToArray());
			meshOut.colors32 = ((listColors32Out.Count <= 0) ? null : listColors32Out.ToArray());
			meshOut.boneWeights = ((listBoneWeightsOut.Count <= 0) ? null : listBoneWeightsOut.ToArray());
			meshOut.bindposes = meshIn.bindposes;
			meshOut.subMeshCount = listlistIndicesOut.Count;
			for (int k = 0; k < listlistIndicesOut.Count; k++)
			{
				meshOut.SetTriangles(listlistIndicesOut[k].ToArray(), k);
			}
			meshOut.name = gameObject.name + " simplified mesh";
			progress("Simplifying mesh: " + strProgressDisplayObjectName, "Mesh consolidation done", 1f);
			yield break;
		}

		private int MapVertex(int nVertex, int nMax)
		{
			if (nMax <= 0)
			{
				return 0;
			}
			while (nVertex >= nMax)
			{
				nVertex = this.m_listVertexMap[nVertex];
			}
			return nVertex;
		}

		private float ComputeEdgeCollapseCost(Simplifier.Vertex u, Simplifier.Vertex v, float fRelevanceBias)
		{
			bool bUseEdgeLength = this.m_bUseEdgeLength;
			bool bUseCurvature = this.m_bUseCurvature;
			bool bProtectTexture = this.m_bProtectTexture;
			bool bLockBorder = this.m_bLockBorder;
			float num = (!bUseEdgeLength) ? 1f : (Vector3.Magnitude(v.m_v3Position - u.m_v3Position) / this.m_fOriginalMeshSize);
			float num2 = 0.001f;
			List<Simplifier.Triangle> list = new List<Simplifier.Triangle>();
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (u.m_listFaces[i].HasVertex(v))
				{
					list.Add(u.m_listFaces[i]);
				}
			}
			if (bUseCurvature)
			{
				for (int i = 0; i < u.m_listFaces.Count; i++)
				{
					float num3 = 1f;
					for (int j = 0; j < list.Count; j++)
					{
						float num4 = Vector3.Dot(u.m_listFaces[i].Normal, list[j].Normal);
						num3 = Mathf.Min(num3, (1f - num4) / 2f);
					}
					num2 = Mathf.Max(num2, num3);
				}
			}
			if (u.IsBorder() && list.Count > 1)
			{
				num2 = 1f;
			}
			if (bProtectTexture)
			{
				bool flag = true;
				for (int i = 0; i < u.m_listFaces.Count; i++)
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (!u.m_listFaces[i].HasUVData)
						{
							flag = false;
							break;
						}
						if (u.m_listFaces[i].TexAt(u) == list[k].TexAt(u))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					num2 = 1f;
				}
			}
			if (bLockBorder && u.IsBorder())
			{
				num2 = 1E+07f;
			}
			num2 += fRelevanceBias;
			return num * num2;
		}

		private void ComputeEdgeCostAtVertex(Simplifier.Vertex v, Transform transform, RelevanceSphere[] aRelevanceSpheres)
		{
			if (v.m_listNeighbors.Count == 0)
			{
				v.m_collapse = null;
				v.m_fObjDist = -0.01f;
				return;
			}
			v.m_fObjDist = 1E+07f;
			v.m_collapse = null;
			float fRelevanceBias = 0f;
			if (aRelevanceSpheres != null)
			{
				for (int i = 0; i < aRelevanceSpheres.Length; i++)
				{
					Matrix4x4 matrix4x = Matrix4x4.TRS(aRelevanceSpheres[i].m_v3Position, Quaternion.Euler(aRelevanceSpheres[i].m_v3Rotation), aRelevanceSpheres[i].m_v3Scale);
					Vector3 v3PositionWorld = v.m_v3PositionWorld;
					if (matrix4x.inverse.MultiplyPoint(v3PositionWorld).magnitude <= 0.5f)
					{
						fRelevanceBias = aRelevanceSpheres[i].m_fRelevance;
					}
				}
			}
			for (int j = 0; j < v.m_listNeighbors.Count; j++)
			{
				float num = this.ComputeEdgeCollapseCost(v, v.m_listNeighbors[j], fRelevanceBias);
				if (v.m_collapse == null || num < v.m_fObjDist)
				{
					v.m_collapse = v.m_listNeighbors[j];
					v.m_fObjDist = num;
				}
			}
		}

		private IEnumerator ComputeAllEdgeCollapseCosts(string strProgressDisplayObjectName, Transform transform, RelevanceSphere[] aRelevanceSpheres, Simplifier.ProgressDelegate progress = null)
		{
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < this.m_listVertices.Count; i++)
			{
				if (progress != null && (i & 255) == 0)
				{
					progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Computing edge collapse cost", (this.m_listVertices.Count != 1) ? ((float)i / ((float)this.m_listVertices.Count - 1f)) : 1f);
					if (Simplifier.Cancelled)
					{
						yield break;
					}
				}
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
				this.ComputeEdgeCostAtVertex(this.m_listVertices[i], transform, aRelevanceSpheres);
				this.HeapAdd(this.m_listVertices[i]);
			}
			yield break;
		}

		private void Collapse(Simplifier.Vertex u, Simplifier.Vertex v, bool bRecompute, Transform transform, RelevanceSphere[] aRelevanceSpheres)
		{
			if (v == null)
			{
				u.Destructor(this);
				return;
			}
			List<Simplifier.Vertex> list = new List<Simplifier.Vertex>();
			for (int i = 0; i < u.m_listNeighbors.Count; i++)
			{
				list.Add(u.m_listNeighbors[i]);
			}
			List<Simplifier.Triangle> list2 = new List<Simplifier.Triangle>();
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (u.m_listFaces[i].HasVertex(v))
				{
					list2.Add(u.m_listFaces[i]);
				}
			}
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (!u.m_listFaces[i].HasVertex(v))
				{
					if (u.m_listFaces[i].HasUVData)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (u.m_listFaces[i].TexAt(u) == list2[j].TexAt(u))
							{
								u.m_listFaces[i].SetTexAt(u, list2[j].TexAt(v));
								break;
							}
						}
					}
				}
			}
			for (int i = u.m_listFaces.Count - 1; i >= 0; i--)
			{
				if (i < u.m_listFaces.Count && i >= 0 && u.m_listFaces[i].HasVertex(v))
				{
					u.m_listFaces[i].Destructor(this);
				}
			}
			for (int i = u.m_listFaces.Count - 1; i >= 0; i--)
			{
				u.m_listFaces[i].ReplaceVertex(u, v);
			}
			u.Destructor(this);
			if (bRecompute)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.ComputeEdgeCostAtVertex(list[i], transform, aRelevanceSpheres);
					this.HeapSortUp(list[i].m_nHeapSpot);
					this.HeapSortDown(list[i].m_nHeapSpot);
				}
			}
		}

		private void AddVertices(List<Vector3> listVertices, List<Vector3> listVerticesWorld, List<MeshUniqueVertices.SerializableBoneWeight> listBoneWeights)
		{
			bool flag = listBoneWeights != null && listBoneWeights.Count > 0;
			for (int i = 0; i < listVertices.Count; i++)
			{
				new Simplifier.Vertex(this, listVertices[i], listVerticesWorld[i], flag, (!flag) ? default(BoneWeight) : listBoneWeights[i].ToBoneWeight(), i);
			}
		}

		private void AddFaceListSubMesh(int nSubMesh, List<int> listTriangles, int[] anIndices, Vector2[] v2Mapping)
		{
			bool bUVData = false;
			if (v2Mapping != null && v2Mapping.Length > 0)
			{
				bUVData = true;
			}
			for (int i = 0; i < listTriangles.Count / 3; i++)
			{
				Simplifier.Triangle t = new Simplifier.Triangle(this, nSubMesh, this.m_listVertices[listTriangles[i * 3]], this.m_listVertices[listTriangles[i * 3 + 1]], this.m_listVertices[listTriangles[i * 3 + 2]], bUVData, anIndices[i * 3], anIndices[i * 3 + 1], anIndices[i * 3 + 2]);
				this.ShareUV(v2Mapping, t);
			}
		}

		private void ShareUV(Vector2[] aMapping, Simplifier.Triangle t)
		{
			if (!t.HasUVData)
			{
				return;
			}
			if (aMapping == null || aMapping.Length == 0)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				int num = i;
				for (int j = 0; j < t.Vertices[num].m_listFaces.Count; j++)
				{
					Simplifier.Triangle triangle = t.Vertices[num].m_listFaces[j];
					if (t != triangle)
					{
						int num2 = t.TexAt(t.Vertices[num]);
						int num3 = triangle.TexAt(t.Vertices[num]);
						if (num2 != num3)
						{
							Vector2 lhs = aMapping[num2];
							Vector2 rhs = aMapping[num3];
							if (lhs == rhs)
							{
								t.SetTexAt(t.Vertices[num], num3);
							}
						}
					}
				}
			}
		}

		private Simplifier.Vertex MinimumCostEdge()
		{
			return this.HeapPop();
		}

		private float HeapValue(int i)
		{
			if (i < 0 || i >= this.m_listHeap.Count)
			{
				return 1E+13f;
			}
			if (this.m_listHeap[i] == null)
			{
				return 1E+13f;
			}
			return this.m_listHeap[i].m_fObjDist;
		}

		private void HeapSortUp(int k)
		{
			int num;
			while (this.HeapValue(k) < this.HeapValue(num = (k - 1) / 2))
			{
				Simplifier.Vertex value = this.m_listHeap[k];
				this.m_listHeap[k] = this.m_listHeap[num];
				this.m_listHeap[k].m_nHeapSpot = k;
				this.m_listHeap[num] = value;
				this.m_listHeap[num].m_nHeapSpot = num;
				k = num;
			}
		}

		private void HeapSortDown(int k)
		{
			if (k == -1)
			{
				return;
			}
			int num;
			while (this.HeapValue(k) > this.HeapValue(num = (k + 1) * 2) || this.HeapValue(k) > this.HeapValue(num - 1))
			{
				num = ((this.HeapValue(num) >= this.HeapValue(num - 1)) ? (num - 1) : num);
				Simplifier.Vertex vertex = this.m_listHeap[k];
				this.m_listHeap[k] = this.m_listHeap[num];
				this.m_listHeap[k].m_nHeapSpot = k;
				this.m_listHeap[num] = vertex;
				if (vertex != null)
				{
					this.m_listHeap[num].m_nHeapSpot = num;
				}
				k = num;
			}
		}

		private void HeapAdd(Simplifier.Vertex v)
		{
			int count = this.m_listHeap.Count;
			this.m_listHeap.Add(v);
			v.m_nHeapSpot = count;
			this.HeapSortUp(count);
		}

		private Simplifier.Vertex HeapPop()
		{
			Simplifier.Vertex vertex = this.m_listHeap[0];
			vertex.m_nHeapSpot = -1;
			this.m_listHeap[0] = null;
			this.HeapSortDown(0);
			return vertex;
		}

		private static int m_nCoroutineFrameMiliseconds;

		private const float MAX_VERTEX_COLLAPSE_COST = 1E+07f;

		private List<Simplifier.Vertex> m_listVertices;

		private List<Simplifier.Vertex> m_listHeap;

		private Simplifier.TriangleList[] m_aListTriangles;

		[SerializeField]
		[HideInInspector]
		private int m_nOriginalMeshVertexCount = -1;

		[SerializeField]
		[HideInInspector]
		private float m_fOriginalMeshSize = 1f;

		[SerializeField]
		[HideInInspector]
		private List<int> m_listVertexMap;

		[SerializeField]
		[HideInInspector]
		private List<int> m_listVertexPermutationBack;

		[SerializeField]
		[HideInInspector]
		private MeshUniqueVertices m_meshUniqueVertices;

		[SerializeField]
		[HideInInspector]
		private Mesh m_meshOriginal;

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

		public delegate void ProgressDelegate(string strTitle, string strProgressMessage, float fT);

		private class Triangle
		{
			public Triangle(Simplifier simplifier, int nSubMesh, Simplifier.Vertex v0, Simplifier.Vertex v1, Simplifier.Vertex v2, bool bUVData, int nIndex1, int nIndex2, int nIndex3)
			{
				this.m_aVertices = new Simplifier.Vertex[3];
				this.m_aUV = new int[3];
				this.m_aIndices = new int[3];
				this.m_aVertices[0] = v0;
				this.m_aVertices[1] = v1;
				this.m_aVertices[2] = v2;
				this.m_nSubMesh = nSubMesh;
				this.m_bUVData = bUVData;
				if (this.m_bUVData)
				{
					this.m_aUV[0] = nIndex1;
					this.m_aUV[1] = nIndex2;
					this.m_aUV[2] = nIndex3;
				}
				this.m_aIndices[0] = nIndex1;
				this.m_aIndices[1] = nIndex2;
				this.m_aIndices[2] = nIndex3;
				this.ComputeNormal();
				simplifier.m_aListTriangles[nSubMesh].m_listTriangles.Add(this);
				for (int i = 0; i < 3; i++)
				{
					this.m_aVertices[i].m_listFaces.Add(this);
					for (int j = 0; j < 3; j++)
					{
						if (i != j && !this.m_aVertices[i].m_listNeighbors.Contains(this.m_aVertices[j]))
						{
							this.m_aVertices[i].m_listNeighbors.Add(this.m_aVertices[j]);
						}
					}
				}
			}

			public Simplifier.Vertex[] Vertices
			{
				get
				{
					return this.m_aVertices;
				}
			}

			public bool HasUVData
			{
				get
				{
					return this.m_bUVData;
				}
			}

			public int[] IndicesUV
			{
				get
				{
					return this.m_aUV;
				}
			}

			public Vector3 Normal
			{
				get
				{
					return this.m_v3Normal;
				}
			}

			public int[] Indices
			{
				get
				{
					return this.m_aIndices;
				}
			}

			public void Destructor(Simplifier simplifier)
			{
				simplifier.m_aListTriangles[this.m_nSubMesh].m_listTriangles.Remove(this);
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] != null)
					{
						this.m_aVertices[i].m_listFaces.Remove(this);
					}
				}
				for (int i = 0; i < 3; i++)
				{
					int num = (i + 1) % 3;
					if (this.m_aVertices[i] != null && this.m_aVertices[num] != null)
					{
						this.m_aVertices[i].RemoveIfNonNeighbor(this.m_aVertices[num]);
						this.m_aVertices[num].RemoveIfNonNeighbor(this.m_aVertices[i]);
					}
				}
			}

			public bool HasVertex(Simplifier.Vertex v)
			{
				return v == this.m_aVertices[0] || v == this.m_aVertices[1] || v == this.m_aVertices[2];
			}

			public void ComputeNormal()
			{
				Vector3 v3Position = this.m_aVertices[0].m_v3Position;
				Vector3 v3Position2 = this.m_aVertices[1].m_v3Position;
				Vector3 v3Position3 = this.m_aVertices[2].m_v3Position;
				this.m_v3Normal = Vector3.Cross(v3Position2 - v3Position, v3Position3 - v3Position2);
				if (this.m_v3Normal.magnitude == 0f)
				{
					return;
				}
				this.m_v3Normal = this.m_v3Normal.normalized;
			}

			public int TexAt(Simplifier.Vertex vertex)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] == vertex)
					{
						return this.m_aUV[i];
					}
				}
				UnityEngine.Debug.LogError("TexAt(): Vertex not found");
				return 0;
			}

			public int TexAt(int i)
			{
				return this.m_aUV[i];
			}

			public void SetTexAt(Simplifier.Vertex vertex, int uv)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] == vertex)
					{
						this.m_aUV[i] = uv;
						return;
					}
				}
				UnityEngine.Debug.LogError("SetTexAt(): Vertex not found");
			}

			public void SetTexAt(int i, int uv)
			{
				this.m_aUV[i] = uv;
			}

			public void ReplaceVertex(Simplifier.Vertex vold, Simplifier.Vertex vnew)
			{
				if (vold == this.m_aVertices[0])
				{
					this.m_aVertices[0] = vnew;
				}
				else if (vold == this.m_aVertices[1])
				{
					this.m_aVertices[1] = vnew;
				}
				else
				{
					this.m_aVertices[2] = vnew;
				}
				vold.m_listFaces.Remove(this);
				vnew.m_listFaces.Add(this);
				for (int i = 0; i < 3; i++)
				{
					vold.RemoveIfNonNeighbor(this.m_aVertices[i]);
					this.m_aVertices[i].RemoveIfNonNeighbor(vold);
				}
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (i != j && !this.m_aVertices[i].m_listNeighbors.Contains(this.m_aVertices[j]))
						{
							this.m_aVertices[i].m_listNeighbors.Add(this.m_aVertices[j]);
						}
					}
				}
				this.ComputeNormal();
			}

			private Simplifier.Vertex[] m_aVertices;

			private bool m_bUVData;

			private int[] m_aUV;

			private int[] m_aIndices;

			private Vector3 m_v3Normal;

			private int m_nSubMesh;
		}

		private class TriangleList
		{
			public TriangleList()
			{
				this.m_listTriangles = new List<Simplifier.Triangle>();
			}

			public List<Simplifier.Triangle> m_listTriangles;
		}

		private class Vertex
		{
			public Vertex(Simplifier simplifier, Vector3 v, Vector3 v3World, bool bHasBoneWeight, BoneWeight boneWeight, int nID)
			{
				this.m_v3Position = v;
				this.m_v3PositionWorld = v3World;
				this.m_bHasBoneWeight = bHasBoneWeight;
				this.m_boneWeight = boneWeight;
				this.m_nID = nID;
				this.m_listNeighbors = new List<Simplifier.Vertex>();
				this.m_listFaces = new List<Simplifier.Triangle>();
				simplifier.m_listVertices.Add(this);
			}

			public void Destructor(Simplifier simplifier)
			{
				while (this.m_listNeighbors.Count > 0)
				{
					this.m_listNeighbors[0].m_listNeighbors.Remove(this);
					if (this.m_listNeighbors.Count > 0)
					{
						this.m_listNeighbors.RemoveAt(0);
					}
				}
				simplifier.m_listVertices.Remove(this);
			}

			public void RemoveIfNonNeighbor(Simplifier.Vertex n)
			{
				if (!this.m_listNeighbors.Contains(n))
				{
					return;
				}
				for (int i = 0; i < this.m_listFaces.Count; i++)
				{
					if (this.m_listFaces[i].HasVertex(n))
					{
						return;
					}
				}
				this.m_listNeighbors.Remove(n);
			}

			public bool IsBorder()
			{
				for (int i = 0; i < this.m_listNeighbors.Count; i++)
				{
					int num = 0;
					for (int j = 0; j < this.m_listFaces.Count; j++)
					{
						if (this.m_listFaces[j].HasVertex(this.m_listNeighbors[i]))
						{
							num++;
						}
					}
					if (num == 1)
					{
						return true;
					}
				}
				return false;
			}

			public Vector3 m_v3Position;

			public Vector3 m_v3PositionWorld;

			public bool m_bHasBoneWeight;

			public BoneWeight m_boneWeight;

			public int m_nID;

			public List<Simplifier.Vertex> m_listNeighbors;

			public List<Simplifier.Triangle> m_listFaces;

			public float m_fObjDist;

			public Simplifier.Vertex m_collapse;

			public int m_nHeapSpot;
		}

		private class VertexDataHashComparer : IEqualityComparer<Simplifier.VertexDataHash>
		{
			public bool Equals(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return a.UV1 == b.UV1 && a.UV2 == b.UV2 && a.Vertex == b.Vertex && a.Color.r == b.Color.r && a.Color.g == b.Color.g && a.Color.b == b.Color.b && a.Color.a == b.Color.a;
			}

			public int GetHashCode(Simplifier.VertexDataHash vdata)
			{
				return vdata.GetHashCode();
			}
		}

		private class VertexDataHash
		{
			public VertexDataHash(Vector3 v3Vertex, Vector3 v3Normal, Vector2 v2Mapping1, Vector2 v2Mapping2, Color32 color)
			{
				this._v3Vertex = v3Vertex;
				this._v3Normal = v3Normal;
				this._v2Mapping1 = v2Mapping1;
				this._v2Mapping2 = v2Mapping2;
				this._color = color;
				this._uniqueVertex = new MeshUniqueVertices.UniqueVertex(v3Vertex);
			}

			public Vector3 Vertex
			{
				get
				{
					return this._v3Vertex;
				}
			}

			public Vector3 Normal
			{
				get
				{
					return this._v3Normal;
				}
			}

			public Vector2 UV1
			{
				get
				{
					return this._v2Mapping1;
				}
			}

			public Vector2 UV2
			{
				get
				{
					return this._v2Mapping2;
				}
			}

			public Color32 Color
			{
				get
				{
					return this._color;
				}
			}

			public override bool Equals(object obj)
			{
				Simplifier.VertexDataHash vertexDataHash = obj as Simplifier.VertexDataHash;
				return vertexDataHash._v2Mapping1 == this._v2Mapping1 && vertexDataHash._v2Mapping2 == this._v2Mapping2 && vertexDataHash._v3Vertex == this._v3Vertex && vertexDataHash._color.r == this._color.r && vertexDataHash._color.g == this._color.g && vertexDataHash._color.b == this._color.b && vertexDataHash._color.a == this._color.a;
			}

			public override int GetHashCode()
			{
				return this._uniqueVertex.GetHashCode();
			}

			public static bool operator ==(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return a.Equals(b);
			}

			public static bool operator !=(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return !a.Equals(b);
			}

			private Vector3 _v3Vertex;

			private Vector3 _v3Normal;

			private Vector2 _v2Mapping1;

			private Vector2 _v2Mapping2;

			private Color32 _color;

			private MeshUniqueVertices.UniqueVertex _uniqueVertex;
		}
	}
}
