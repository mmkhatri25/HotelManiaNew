using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anima2D
{
	[ExecuteInEditMode]
	public class SpriteMeshInstance : MonoBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("spriteMesh")]
		private SpriteMesh m_SpriteMesh;

		[SerializeField]
		private Color m_Color = Color.white;

		[SerializeField]
		private Material[] m_Materials;

		[SerializeField]
		private int m_SortingLayerID;

		[SerializeField]
		[FormerlySerializedAs("orderInLayer")]
		private int m_SortingOrder;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("bones")]
		private Bone2D[] m_Bones;

		[SerializeField]
		[HideInInspector]
		private Transform[] m_BoneTransforms;

		private List<Bone2D> m_CachedBones = new List<Bone2D>();

		private MaterialPropertyBlock m_MaterialPropertyBlock;

		private Renderer mCachedRenderer;

		private MeshFilter mCachedMeshFilter;

		private SkinnedMeshRenderer mCachedSkinnedRenderer;

		private Mesh m_InitialMesh;

		private Mesh m_CurrentMesh;

		public SpriteMesh spriteMesh
		{
			get
			{
				return m_SpriteMesh;
			}
			set
			{
				m_SpriteMesh = value;
			}
		}

		public Material sharedMaterial
		{
			get
			{
				if (m_Materials.Length != 0)
				{
					return m_Materials[0];
				}
				return null;
			}
			set
			{
				m_Materials = new Material[1]
				{
					value
				};
			}
		}

		public Material[] sharedMaterials
		{
			get
			{
				return m_Materials;
			}
			set
			{
				m_Materials = value;
			}
		}

		public Color color
		{
			get
			{
				return m_Color;
			}
			set
			{
				m_Color = value;
			}
		}

		public int sortingLayerID
		{
			get
			{
				return m_SortingLayerID;
			}
			set
			{
				m_SortingLayerID = value;
			}
		}

		public string sortingLayerName
		{
			get
			{
				if ((bool)cachedRenderer)
				{
					return cachedRenderer.sortingLayerName;
				}
				return "Default";
			}
			set
			{
				if ((bool)cachedRenderer)
				{
					cachedRenderer.sortingLayerName = value;
					sortingLayerID = cachedRenderer.sortingLayerID;
				}
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_SortingOrder;
			}
			set
			{
				m_SortingOrder = value;
			}
		}

		public List<Bone2D> bones
		{
			get
			{
				if (m_Bones != null && m_Bones.Length != 0 && m_CachedBones.Count == 0)
				{
					bones = new List<Bone2D>(m_Bones);
				}
				if (m_BoneTransforms != null && m_CachedBones.Count != m_BoneTransforms.Length)
				{
					m_CachedBones = new List<Bone2D>(m_BoneTransforms.Length);
					for (int i = 0; i < m_BoneTransforms.Length; i++)
					{
						Bone2D item = null;
						if ((bool)m_BoneTransforms[i])
						{
							item = m_BoneTransforms[i].GetComponent<Bone2D>();
						}
						m_CachedBones.Add(item);
					}
				}
				for (int j = 0; j < m_CachedBones.Count; j++)
				{
					if ((bool)m_CachedBones[j] && m_BoneTransforms[j] != m_CachedBones[j].transform)
					{
						m_CachedBones[j] = null;
					}
					if (!m_CachedBones[j] && (bool)m_BoneTransforms[j])
					{
						m_CachedBones[j] = m_BoneTransforms[j].GetComponent<Bone2D>();
					}
				}
				return m_CachedBones;
			}
			set
			{
				m_Bones = null;
				m_CachedBones = new List<Bone2D>(value);
				m_BoneTransforms = new Transform[m_CachedBones.Count];
				for (int i = 0; i < m_CachedBones.Count; i++)
				{
					Bone2D bone2D = m_CachedBones[i];
					if ((bool)bone2D)
					{
						m_BoneTransforms[i] = bone2D.transform;
					}
				}
				if ((bool)cachedSkinnedRenderer)
				{
					cachedSkinnedRenderer.bones = m_BoneTransforms;
				}
			}
		}

		private MaterialPropertyBlock materialPropertyBlock
		{
			get
			{
				if (m_MaterialPropertyBlock == null)
				{
					m_MaterialPropertyBlock = new MaterialPropertyBlock();
				}
				return m_MaterialPropertyBlock;
			}
		}

		public Renderer cachedRenderer
		{
			get
			{
				if (!mCachedRenderer)
				{
					mCachedRenderer = GetComponent<Renderer>();
				}
				return mCachedRenderer;
			}
		}

		public MeshFilter cachedMeshFilter
		{
			get
			{
				if (!mCachedMeshFilter)
				{
					mCachedMeshFilter = GetComponent<MeshFilter>();
				}
				return mCachedMeshFilter;
			}
		}

		public SkinnedMeshRenderer cachedSkinnedRenderer
		{
			get
			{
				if (!mCachedSkinnedRenderer)
				{
					mCachedSkinnedRenderer = GetComponent<SkinnedMeshRenderer>();
				}
				return mCachedSkinnedRenderer;
			}
		}

		private Texture2D spriteTexture
		{
			get
			{
				if ((bool)spriteMesh && (bool)spriteMesh.sprite)
				{
					return spriteMesh.sprite.texture;
				}
				return null;
			}
		}

		public Mesh sharedMesh
		{
			get
			{
				if ((bool)m_InitialMesh)
				{
					return m_InitialMesh;
				}
				return null;
			}
		}

		public Mesh mesh
		{
			get
			{
				if ((bool)m_CurrentMesh)
				{
					return UnityEngine.Object.Instantiate(m_CurrentMesh);
				}
				return null;
			}
		}

		private void OnDestroy()
		{
			if ((bool)m_CurrentMesh)
			{
				UnityEngine.Object.DestroyImmediate(m_CurrentMesh);
			}
		}

		private void Awake()
		{
			UpdateCurrentMesh();
		}

		private void UpdateInitialMesh()
		{
			m_InitialMesh = null;
			if ((bool)spriteMesh && (bool)spriteMesh.sharedMesh)
			{
				m_InitialMesh = spriteMesh.sharedMesh;
			}
		}

		private void UpdateCurrentMesh()
		{
			UpdateInitialMesh();
			if ((bool)m_InitialMesh)
			{
				if (!m_CurrentMesh)
				{
					m_CurrentMesh = new Mesh();
					m_CurrentMesh.hideFlags = HideFlags.DontSave;
					m_CurrentMesh.MarkDynamic();
				}
				m_CurrentMesh.Clear();
				m_CurrentMesh.name = m_InitialMesh.name;
				m_CurrentMesh.vertices = m_InitialMesh.vertices;
				m_CurrentMesh.uv = m_InitialMesh.uv;
				m_CurrentMesh.normals = m_InitialMesh.normals;
				m_CurrentMesh.tangents = m_InitialMesh.tangents;
				m_CurrentMesh.boneWeights = m_InitialMesh.boneWeights;
				m_CurrentMesh.bindposes = m_InitialMesh.bindposes;
				m_CurrentMesh.bounds = m_InitialMesh.bounds;
				m_CurrentMesh.colors = m_InitialMesh.colors;
				for (int i = 0; i < m_InitialMesh.subMeshCount; i++)
				{
					m_CurrentMesh.SetTriangles(m_InitialMesh.GetTriangles(i), i);
				}
				m_CurrentMesh.ClearBlendShapes();
				for (int j = 0; j < m_InitialMesh.blendShapeCount; j++)
				{
					string blendShapeName = m_InitialMesh.GetBlendShapeName(j);
					for (int k = 0; k < m_InitialMesh.GetBlendShapeFrameCount(j); k++)
					{
						float blendShapeFrameWeight = m_InitialMesh.GetBlendShapeFrameWeight(j, k);
						Vector3[] deltaVertices = new Vector3[m_InitialMesh.vertexCount];
						m_InitialMesh.GetBlendShapeFrameVertices(j, k, deltaVertices, null, null);
						m_CurrentMesh.AddBlendShapeFrame(blendShapeName, blendShapeFrameWeight, deltaVertices, null, null);
					}
				}
				m_CurrentMesh.hideFlags = HideFlags.DontSave;
			}
			else
			{
				m_InitialMesh = null;
				if ((bool)m_CurrentMesh)
				{
					m_CurrentMesh.Clear();
				}
			}
			if ((bool)m_CurrentMesh)
			{
				if ((bool)spriteMesh && (bool)spriteMesh.sprite && spriteMesh.sprite.packed)
				{
					SetSpriteUVs(m_CurrentMesh, spriteMesh.sprite);
				}
				m_CurrentMesh.UploadMeshData(markNoLongerReadable: false);
			}
			UpdateRenderers();
		}

		private void SetSpriteUVs(Mesh mesh, Sprite sprite)
		{
			Vector2[] uv = sprite.uv;
			if (mesh.vertexCount == uv.Length)
			{
				mesh.uv = sprite.uv;
			}
		}

		private void UpdateRenderers()
		{
			Mesh sharedMesh = null;
			if ((bool)m_InitialMesh)
			{
				sharedMesh = m_CurrentMesh;
			}
			if ((bool)cachedSkinnedRenderer)
			{
				cachedSkinnedRenderer.sharedMesh = sharedMesh;
			}
			else if ((bool)cachedMeshFilter)
			{
				cachedMeshFilter.sharedMesh = sharedMesh;
			}
		}

		private void LateUpdate()
		{
			if (!spriteMesh || ((bool)spriteMesh && spriteMesh.sharedMesh != m_InitialMesh))
			{
				UpdateCurrentMesh();
			}
		}

		private void OnWillRenderObject()
		{
			UpdateRenderers();
			if ((bool)cachedRenderer)
			{
				cachedRenderer.sortingLayerID = sortingLayerID;
				cachedRenderer.sortingOrder = sortingOrder;
				cachedRenderer.sharedMaterials = m_Materials;
				cachedRenderer.GetPropertyBlock(materialPropertyBlock);
				if ((bool)spriteTexture)
				{
					materialPropertyBlock.SetTexture("_MainTex", spriteTexture);
				}
				materialPropertyBlock.SetColor("_Color", color);
				cachedRenderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}
}
