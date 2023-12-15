using System;
using UnityEngine;

namespace Anima2D
{
	public static class MathUtils
	{
		public static float SignedAngle(Vector3 a, Vector3 b, Vector3 forward)
		{
			float num = Vector3.Angle(a, b);
			float num2 = Mathf.Sign(Vector3.Dot(forward, Vector3.Cross(a, b)));
			return num * num2;
		}

		public static float Fmod(float value, float mod)
		{
			return Mathf.Abs(value % mod + mod) % mod;
		}

		public static float SegmentDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			Vector2 lhs = b - a;
			Vector2 rhs = point - a;
			float num = Vector2.Dot(lhs, rhs);
			if (num <= 0f)
			{
				return rhs.magnitude;
			}
			if (num >= lhs.sqrMagnitude)
			{
				return (point - b).magnitude;
			}
			return LineDistance(point, a, b);
		}

		public static float SegmentSqrtDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			Vector2 lhs = b - a;
			Vector2 rhs = point - a;
			float num = Vector2.Dot(lhs, rhs);
			if (num <= 0f)
			{
				return rhs.sqrMagnitude;
			}
			if (num >= lhs.sqrMagnitude)
			{
				return (point - b).sqrMagnitude;
			}
			return SqrtLineDistance(point, a, b);
		}

		public static float LineDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			return Mathf.Sqrt(SqrtLineDistance(point, a, b));
		}

		public static float SqrtLineDistance(Vector3 point, Vector3 a, Vector3 b)
		{
			float num = Mathf.Abs((b.y - a.y) * point.x - (b.x - a.x) * point.y + b.x * a.y - b.y * a.x);
			return num * num / ((b.y - a.y) * (b.y - a.y) + (b.x - a.x) * (b.x - a.x));
		}

		public static void WorldFromMatrix4x4(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.rotation = matrix.GetRotation();
			transform.position = matrix.GetPosition();
		}

		public static void LocalFromMatrix4x4(this Transform transform, Matrix4x4 matrix)
		{
			transform.localScale = matrix.GetScale();
			transform.localRotation = matrix.GetRotation();
			transform.localPosition = matrix.GetPosition();
		}

		public static Quaternion GetRotation(this Matrix4x4 matrix)
		{
			float num = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2f;
			float num2 = 4f * num;
			float x = (matrix.m21 - matrix.m12) / num2;
			float y = (matrix.m02 - matrix.m20) / num2;
			float z = (matrix.m10 - matrix.m01) / num2;
			return new Quaternion(x, y, z, num);
		}

		public static Vector3 GetPosition(this Matrix4x4 matrix)
		{
			float m = matrix.m03;
			float m2 = matrix.m13;
			float m3 = matrix.m23;
			return new Vector3(m, m2, m3);
		}

		public static Vector3 GetScale(this Matrix4x4 m)
		{
			float x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
			float y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
			float z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);
			return new Vector3(x, y, z);
		}

		public static Rect ClampRect(Rect rect, Rect frame)
		{
			Rect result = default(Rect);
			result.xMin = Mathf.Clamp(rect.xMin, 0f, frame.width - 1f);
			result.yMin = Mathf.Clamp(rect.yMin, 0f, frame.height - 1f);
			result.xMax = Mathf.Clamp(rect.xMax, 1f, frame.width);
			result.yMax = Mathf.Clamp(rect.yMax, 1f, frame.height);
			return result;
		}

		public static Vector2 ClampPositionInRect(Vector2 position, Rect frame)
		{
			return new Vector2(Mathf.Clamp(position.x, frame.xMin, frame.xMax), Mathf.Clamp(position.y, frame.yMin, frame.yMax));
		}

		public static Rect OrderMinMax(Rect rect)
		{
			if (rect.xMin > rect.xMax)
			{
				float xMin = rect.xMin;
				rect.xMin = rect.xMax;
				rect.xMax = xMin;
			}
			if (rect.yMin > rect.yMax)
			{
				float yMin = rect.yMin;
				rect.yMin = rect.yMax;
				rect.yMax = yMin;
			}
			return rect;
		}

		public static float RoundToMultipleOf(float value, float roundingValue)
		{
			if (roundingValue == 0f)
			{
				return value;
			}
			return Mathf.Round(value / roundingValue) * roundingValue;
		}

		public static float GetClosestPowerOfTen(float positiveNumber)
		{
			if (positiveNumber <= 0f)
			{
				return 1f;
			}
			return Mathf.Pow(10f, Mathf.RoundToInt(Mathf.Log10(positiveNumber)));
		}

		public static float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
		{
			if (minDifference == 0f)
			{
				return DiscardLeastSignificantDecimal(valueToRound);
			}
			return (float)Math.Round(valueToRound, GetNumberOfDecimalsForMinimumDifference(minDifference), MidpointRounding.AwayFromZero);
		}

		public static float DiscardLeastSignificantDecimal(float v)
		{
			int digits = Mathf.Clamp((int)(5f - Mathf.Log10(Mathf.Abs(v))), 0, 15);
			return (float)Math.Round(v, digits, MidpointRounding.AwayFromZero);
		}

		public static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
		{
			return Mathf.Clamp(-Mathf.FloorToInt(Mathf.Log10(minDifference)), 0, 15);
		}

		public static Vector3 Unskin(Vector3 skinedPosition, Matrix4x4 localToWorld, BoneWeight boneWeight, Matrix4x4[] bindposes, Transform[] bones)
		{
			Matrix4x4 matrix4x = bones[boneWeight.boneIndex0].localToWorldMatrix * bindposes[boneWeight.boneIndex0];
			Matrix4x4 matrix4x2 = bones[boneWeight.boneIndex1].localToWorldMatrix * bindposes[boneWeight.boneIndex1];
			Matrix4x4 matrix4x3 = bones[boneWeight.boneIndex2].localToWorldMatrix * bindposes[boneWeight.boneIndex2];
			Matrix4x4 matrix4x4 = bones[boneWeight.boneIndex3].localToWorldMatrix * bindposes[boneWeight.boneIndex3];
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				int index = i;
				matrix4x[index] *= boneWeight.weight0;
				index = i;
				matrix4x2[index] *= boneWeight.weight1;
				index = i;
				matrix4x3[index] *= boneWeight.weight2;
				index = i;
				matrix4x4[index] *= boneWeight.weight3;
				identity[i] = matrix4x[i] + matrix4x2[i] + matrix4x3[i] + matrix4x4[i];
			}
			return localToWorld.MultiplyPoint3x4(identity.inverse.MultiplyPoint3x4(skinedPosition));
		}
	}
}
