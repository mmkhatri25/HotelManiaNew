using System.Collections.Generic;
using UnityEngine;

namespace Anima2D
{
	public class IkGroup : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private List<Ik2D> m_IkComponents = new List<Ik2D>();

		public void UpdateGroup()
		{
			for (int i = 0; i < m_IkComponents.Count; i++)
			{
				Ik2D ik2D = m_IkComponents[i];
				if ((bool)ik2D)
				{
					ik2D.enabled = false;
					ik2D.UpdateIK();
				}
			}
		}

		private void LateUpdate()
		{
			UpdateGroup();
		}
	}
}
