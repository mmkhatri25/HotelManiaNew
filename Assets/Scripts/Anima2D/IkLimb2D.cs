using UnityEngine;

namespace Anima2D
{
	public class IkLimb2D : Ik2D
	{
		public bool flip;

		[SerializeField]
		private IkSolver2DLimb m_Solver = new IkSolver2DLimb();

		protected override IkSolver2D GetSolver()
		{
			return m_Solver;
		}

		protected override void Validate()
		{
			base.numBones = 2;
		}

		protected override int ValidateNumBones(int numBones)
		{
			return 2;
		}

		protected override void OnIkUpdate()
		{
			base.OnIkUpdate();
			m_Solver.flip = flip;
		}

		private void OnValidate()
		{
			base.numBones = 2;
		}
	}
}
