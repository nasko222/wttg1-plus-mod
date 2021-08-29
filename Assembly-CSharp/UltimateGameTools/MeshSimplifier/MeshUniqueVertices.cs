using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	[Serializable]
	public class MeshUniqueVertices
	{
		public MeshUniqueVertices.ListIndices[] SubmeshesFaceList
		{
			get
			{
				return this.m_aFaceList;
			}
		}

		public List<Vector3> ListVertices
		{
			get
			{
				return this.m_listVertices;
			}
		}

		public List<Vector3> ListVerticesWorld
		{
			get
			{
				return this.m_listVerticesWorld;
			}
		}

		public List<MeshUniqueVertices.SerializableBoneWeight> ListBoneWeights
		{
			get
			{
				return this.m_listBoneWeights;
			}
		}

		public void BuildData(Mesh sourceMesh, Vector3[] av3VerticesWorld)
		{
			Vector3[] vertices = sourceMesh.vertices;
			BoneWeight[] boneWeights = sourceMesh.boneWeights;
			Dictionary<MeshUniqueVertices.UniqueVertex, MeshUniqueVertices.RepeatedVertexList> dictionary = new Dictionary<MeshUniqueVertices.UniqueVertex, MeshUniqueVertices.RepeatedVertexList>();
			this.m_listVertices = new List<Vector3>();
			this.m_listVerticesWorld = new List<Vector3>();
			this.m_listBoneWeights = new List<MeshUniqueVertices.SerializableBoneWeight>();
			this.m_aFaceList = new MeshUniqueVertices.ListIndices[sourceMesh.subMeshCount];
			for (int i = 0; i < sourceMesh.subMeshCount; i++)
			{
				this.m_aFaceList[i] = new MeshUniqueVertices.ListIndices();
				int[] triangles = sourceMesh.GetTriangles(i);
				for (int j = 0; j < triangles.Length; j++)
				{
					MeshUniqueVertices.UniqueVertex key = new MeshUniqueVertices.UniqueVertex(vertices[triangles[j]]);
					if (dictionary.ContainsKey(key))
					{
						dictionary[key].Add(new MeshUniqueVertices.RepeatedVertex(j / 3, triangles[j]));
						this.m_aFaceList[i].m_listIndices.Add(dictionary[key].UniqueIndex);
					}
					else
					{
						int count = this.m_listVertices.Count;
						dictionary.Add(key, new MeshUniqueVertices.RepeatedVertexList(count, new MeshUniqueVertices.RepeatedVertex(j / 3, triangles[j])));
						this.m_listVertices.Add(vertices[triangles[j]]);
						this.m_listVerticesWorld.Add(av3VerticesWorld[triangles[j]]);
						this.m_aFaceList[i].m_listIndices.Add(count);
						if (boneWeights != null && boneWeights.Length > 0)
						{
							this.m_listBoneWeights.Add(new MeshUniqueVertices.SerializableBoneWeight(boneWeights[triangles[j]]));
						}
					}
				}
			}
		}

		[SerializeField]
		private List<Vector3> m_listVertices;

		[SerializeField]
		private List<Vector3> m_listVerticesWorld;

		[SerializeField]
		private List<MeshUniqueVertices.SerializableBoneWeight> m_listBoneWeights;

		[SerializeField]
		private MeshUniqueVertices.ListIndices[] m_aFaceList;

		[Serializable]
		public class ListIndices
		{
			public ListIndices()
			{
				this.m_listIndices = new List<int>();
			}

			public List<int> m_listIndices;
		}

		[Serializable]
		public class SerializableBoneWeight
		{
			public SerializableBoneWeight(BoneWeight boneWeight)
			{
				this._boneIndex0 = boneWeight.boneIndex0;
				this._boneIndex1 = boneWeight.boneIndex1;
				this._boneIndex2 = boneWeight.boneIndex2;
				this._boneIndex3 = boneWeight.boneIndex3;
				this._boneWeight0 = boneWeight.weight0;
				this._boneWeight1 = boneWeight.weight1;
				this._boneWeight2 = boneWeight.weight2;
				this._boneWeight3 = boneWeight.weight3;
			}

			public BoneWeight ToBoneWeight()
			{
				BoneWeight result = default(BoneWeight);
				result.boneIndex0 = this._boneIndex0;
				result.boneIndex1 = this._boneIndex1;
				result.boneIndex2 = this._boneIndex2;
				result.boneIndex3 = this._boneIndex3;
				result.weight0 = this._boneWeight0;
				result.weight1 = this._boneWeight1;
				result.weight2 = this._boneWeight2;
				result.weight3 = this._boneWeight3;
				return result;
			}

			public int _boneIndex0;

			public int _boneIndex1;

			public int _boneIndex2;

			public int _boneIndex3;

			public float _boneWeight0;

			public float _boneWeight1;

			public float _boneWeight2;

			public float _boneWeight3;
		}

		public class UniqueVertex
		{
			public UniqueVertex(Vector3 v3Vertex)
			{
				this.FromVertex(v3Vertex);
			}

			public override bool Equals(object obj)
			{
				MeshUniqueVertices.UniqueVertex uniqueVertex = obj as MeshUniqueVertices.UniqueVertex;
				return uniqueVertex.m_nFixedX == this.m_nFixedX && uniqueVertex.m_nFixedY == this.m_nFixedY && uniqueVertex.m_nFixedZ == this.m_nFixedZ;
			}

			public override int GetHashCode()
			{
				return this.m_nFixedX + (this.m_nFixedY << 2) + (this.m_nFixedZ << 4);
			}

			public Vector3 ToVertex()
			{
				return new Vector3(this.FixedToCoord(this.m_nFixedX), this.FixedToCoord(this.m_nFixedY), this.FixedToCoord(this.m_nFixedZ));
			}

			public static bool operator ==(MeshUniqueVertices.UniqueVertex a, MeshUniqueVertices.UniqueVertex b)
			{
				return a.Equals(b);
			}

			public static bool operator !=(MeshUniqueVertices.UniqueVertex a, MeshUniqueVertices.UniqueVertex b)
			{
				return !a.Equals(b);
			}

			private void FromVertex(Vector3 vertex)
			{
				this.m_nFixedX = this.CoordToFixed(vertex.x);
				this.m_nFixedY = this.CoordToFixed(vertex.y);
				this.m_nFixedZ = this.CoordToFixed(vertex.z);
			}

			private int CoordToFixed(float fCoord)
			{
				int num = Mathf.FloorToInt(fCoord);
				int num2 = Mathf.FloorToInt((fCoord - (float)num) * 100000f);
				return num << 16 | num2;
			}

			private float FixedToCoord(int nFixed)
			{
				float num = (float)(nFixed & 65535) / 100000f;
				float num2 = (float)(nFixed >> 16);
				return num2 + num;
			}

			private int m_nFixedX;

			private int m_nFixedY;

			private int m_nFixedZ;

			private const float fDecimalMultiplier = 100000f;
		}

		private class RepeatedVertex
		{
			public RepeatedVertex(int nFaceIndex, int nOriginalVertexIndex)
			{
				this._nFaceIndex = nFaceIndex;
				this._nOriginalVertexIndex = nOriginalVertexIndex;
			}

			public int FaceIndex
			{
				get
				{
					return this._nFaceIndex;
				}
			}

			public int OriginalVertexIndex
			{
				get
				{
					return this._nOriginalVertexIndex;
				}
			}

			private int _nFaceIndex;

			private int _nOriginalVertexIndex;
		}

		private class RepeatedVertexList
		{
			public RepeatedVertexList(int nUniqueIndex, MeshUniqueVertices.RepeatedVertex repeatedVertex)
			{
				this.m_nUniqueIndex = nUniqueIndex;
				this.m_listRepeatedVertices = new List<MeshUniqueVertices.RepeatedVertex>();
				this.m_listRepeatedVertices.Add(repeatedVertex);
			}

			public int UniqueIndex
			{
				get
				{
					return this.m_nUniqueIndex;
				}
			}

			public void Add(MeshUniqueVertices.RepeatedVertex repeatedVertex)
			{
				this.m_listRepeatedVertices.Add(repeatedVertex);
			}

			private int m_nUniqueIndex;

			private List<MeshUniqueVertices.RepeatedVertex> m_listRepeatedVertices;
		}
	}
}
