using UnityEngine;
using UnityEngine.Serialization;

namespace Anima2D
{
	public class SpriteMesh : ScriptableObject
	{
		public const int api_version = 4;

		[SerializeField]
		[HideInInspector]
		private int m_ApiVersion;

		[SerializeField]
		[FormerlySerializedAs("sprite")]
		private Sprite m_Sprite;

		[SerializeField]
		private Mesh m_SharedMesh;

		public Sprite sprite => m_Sprite;

		public Mesh sharedMesh => m_SharedMesh;
	}
}
