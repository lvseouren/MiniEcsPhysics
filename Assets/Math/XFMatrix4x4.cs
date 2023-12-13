using UnityEngine;
using XFixMath.NET;

public struct XFMatrix4x4 {
	//
	// Fields
	//
	public XFix64 m00;
	
	public XFix64 m10;
	
	public XFix64 m20;
	
	public XFix64 m30;
	
	public XFix64 m01;
	
	public XFix64 m11;
	
	public XFix64 m21;
	
	public XFix64 m31;
	
	public XFix64 m02;
	
	public XFix64 m12;
	
	public XFix64 m22;
	
	public XFix64 m32;
	
	public XFix64 m03;
	
	public XFix64 m13;
	
	public XFix64 m23;
	
	public XFix64 m33;

	//
	// Static Properties
	//
	public static XFMatrix4x4 identity
	{
		get
		{
			return new XFMatrix4x4
			{
				m00 = 1f,
				m01 = 0f,
				m02 = 0f,
				m03 = 0f,
				m10 = 0f,
				m11 = 1f,
				m12 = 0f,
				m13 = 0f,
				m20 = 0f,
				m21 = 0f,
				m22 = 1f,
				m23 = 0f,
				m30 = 0f,
				m31 = 0f,
				m32 = 0f,
				m33 = 1f
			};
		}
	}

	public static XFMatrix4x4 zero
	{
		get
		{
			return new XFMatrix4x4
			{
				m00 = 0f,
				m01 = 0f,
				m02 = 0f,
				m03 = 0f,
				m10 = 0f,
				m11 = 0f,
				m12 = 0f,
				m13 = 0f,
				m20 = 0f,
				m21 = 0f,
				m22 = 0f,
				m23 = 0f,
				m30 = 0f,
				m31 = 0f,
				m32 = 0f,
				m33 = 0f
			};
		}
	}

	//
	// Properties
	//
	public XFMatrix4x4 inverse
	{
		get
		{
			return XFMatrix4x4.Inverse (this);
		}
	}

	//
	// Static Methods
	//
	public static XFMatrix4x4 Inverse (XFMatrix4x4 m)
	{
		//TODO 未实现
		XFix64 num1 = m.m00;
		XFix64 num2 = m.m01;
		XFix64 num3 = m.m02;
		XFix64 num4 = m.m03;
		XFix64 num5 = m.m10;
		XFix64 num6 = m.m11;
		XFix64 num7 = m.m12;
		XFix64 num8 = m.m13;
		XFix64 num9 = m.m20;
		XFix64 num10 = m.m21;
		XFix64 num11 = m.m22;
		XFix64 num12 = m.m23;
		XFix64 num13 = m.m30;
		XFix64 num14 = m.m31;
		XFix64 num15 = m.m32;
		XFix64 num16 = m.m33;

		XFix64 num17 =  ( num11 *  num16 -  num12 *  num15);
		XFix64 num18 =  ( num10 *  num16 -  num12 *  num14);
		XFix64 num19 =  ( num10 *  num15 -  num11 *  num14);
		XFix64 num20 =  ( num9 *  num16 -  num12 *  num13);
		XFix64 num21 =  ( num9 *  num15 -  num11 *  num13);
		XFix64 num22 =  ( num9 *  num14 -  num10 *  num13);
		XFix64 num23 =  ( num6 *  num17 -  num7 *  num18 +  num8 *  num19);
		XFix64 num24 =  -( num5 *  num17 -  num7 *  num20 +  num8 *  num21);
		XFix64 num25 =  ( num5 *  num18 -  num6 *  num20 +  num8 *  num22);
		XFix64 num26 =  -( num5 *  num19 -  num6 *  num21 +  num7 *  num22);
		XFix64 num27 =  (1.0f / ( num1 *  num23 +  num2 *  num24 +  num3 *  num25 +  num4 *  num26));

		XFMatrix4x4 result = XFMatrix4x4.zero;
		result.m00 = num23 * num27;
		result.m10 = num24 * num27;
		result.m20 = num25 * num27;
		result.m30 = num26 * num27;
		result.m01 =  -( num2 *  num17 -  num3 *  num18 +  num4 *  num19) * num27;
		result.m11 =  ( num1 *  num17 -  num3 *  num20 +  num4 *  num21) * num27;
		result.m21 =  -( num1 *  num18 -  num2 *  num20 +  num4 *  num22) * num27;
		result.m31 =  ( num1 *  num19 -  num2 *  num21 +  num3 *  num22) * num27;
		XFix64 num28 =  ( num7 *  num16 -  num8 *  num15);
		XFix64 num29 =  ( num6 *  num16 -  num8 *  num14);
		XFix64 num30 =  ( num6 *  num15 -  num7 *  num14);
		XFix64 num31 =  ( num5 *  num16 -  num8 *  num13);
		XFix64 num32 =  ( num5 *  num15 -  num7 *  num13);
		XFix64 num33 =  ( num5 *  num14 -  num6 *  num13);
		result.m02 =  ( num2 *  num28 -  num3 *  num29 +  num4 *  num30) * num27;
		result.m12 =  -( num1 *  num28 -  num3 *  num31 +  num4 *  num32) * num27;
		result.m22 =  ( num1 *  num29 -  num2 *  num31 +  num4 *  num33) * num27;
		result.m32 =  -( num1 *  num30 -  num2 *  num32 +  num3 *  num33) * num27;
		XFix64 num34 =  ( num7 *  num12 -  num8 *  num11);
		XFix64 num35 =  ( num6 *  num12 -  num8 *  num10);
		XFix64 num36 =  ( num6 *  num11 -  num7 *  num10);
		XFix64 num37 =  ( num5 *  num12 -  num8 *  num9);
		XFix64 num38 =  ( num5 *  num11 -  num7 *  num9);
		XFix64 num39 =  ( num5 *  num10 -  num6 *  num9);
		result.m03 =  -( num2 *  num34 -  num3 *  num35 +  num4 *  num36) * num27;
		result.m13 =  ( num1 *  num34 -  num3 *  num37 +  num4 *  num38) * num27;
		result.m23 =  -( num1 *  num35 -  num2 *  num37 +  num4 *  num39) * num27;
		result.m33 =  ( num1 *  num36 -  num2 *  num38 +  num3 *  num39) * num27;

		return result;
	}

	//
	// Methods
	//
	//public void SetTRS (Vector3 pos, Quaternion q, Vector3 s)
	//{
	//	//TODO 未实现
	//	Vector3 eulerAngles = q.eulerAngles;
	//	XFix64 sinX = Mathf.Sin (eulerAngles.x);
	//	XFix64 cosX = Mathf.Cos (eulerAngles.x);
	//	XFix64 sinY = Mathf.Sin (eulerAngles.y);
	//	XFix64 cosY = Mathf.Cos (eulerAngles.y);
	//	XFix64 sinZ = Mathf.Sin (eulerAngles.z);
	//	XFix64 cosZ = Mathf.Cos (eulerAngles.z);

	//	XFMatrix4x4 transPos = XFMatrix4x4.identity;
	//	transPos.m03 = pos.x;
	//	transPos.m13 = pos.y;
	//	transPos.m23 = pos.z;

	//	XFMatrix4x4 xrote = XFMatrix4x4.identity;
	//	xrote.m11 = cosX;
	//	xrote.m12 = -sinX;
	//	xrote.m21 = sinX;
	//	xrote.m22 = cosX;

	//	XFMatrix4x4 yrote = XFMatrix4x4.identity;
	//	yrote.m00 = cosY;
	//	yrote.m02 = sinY;
	//	yrote.m20 = -sinY;
	//	yrote.m22 = cosY;

	//	XFMatrix4x4 zrote = XFMatrix4x4.identity;
	//	zrote.m00 = cosZ;
	//	zrote.m01 = -sinZ;
	//	zrote.m10 = sinZ;
	//	zrote.m11 = cosZ;

	//	XFMatrix4x4 scale = XFMatrix4x4.identity;
	//	scale.m00 = s.x;
	//	scale.m11 = s.y;
	//	scale.m22 = s.z;

	//	this = xrote * yrote * zrote * scale * transPos;
	//}

	public Vector3 MultiplyPoint3x4 (Vector3 v)
	{
		Vector3 result;
		result.x = this.m00 * v.x + this.m01 * v.y + this.m02 * v.z + this.m03;
		result.y = this.m10 * v.x + this.m11 * v.y + this.m12 * v.z + this.m13;
		result.z = this.m20 * v.x + this.m21 * v.y + this.m22 * v.z + this.m23;
		return result;
	}

	//
	// Operators
	//
	public static XFMatrix4x4 operator * (XFMatrix4x4 lhs, XFMatrix4x4 rhs)
	{
		return new XFMatrix4x4
		{
			m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30,
			m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31,
			m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32,
			m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33,
			m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30,
			m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31,
			m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32,
			m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33,
			m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30,
			m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31,
			m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32,
			m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33,
			m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30,
			m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31,
			m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32,
			m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33
		};
	}

	public static Vector4 operator * (XFMatrix4x4 lhs, Vector4 v)
	{
		Vector4 result;
		result.x = lhs.m00 * v.x + lhs.m01 * v.y + lhs.m02 * v.z + lhs.m03 * v.w;
		result.y = lhs.m10 * v.x + lhs.m11 * v.y + lhs.m12 * v.z + lhs.m13 * v.w;
		result.z = lhs.m20 * v.x + lhs.m21 * v.y + lhs.m22 * v.z + lhs.m23 * v.w;
		result.w = lhs.m30 * v.x + lhs.m31 * v.y + lhs.m32 * v.z + lhs.m33 * v.w;
		return result;
	}

	public override string ToString ()
	{
		return string.Format ("XFMatrix4x4\n {0:F5}\t{1:F5}\t{2:F5}\t{3:F5}\n{4:F5}\t{5:F5}\t{6:F5}\t{7:F5}\n{8:F5}\t{9:F5}\t{10:F5}\t{11:F5}\n{12:F5}\t{13:F5}\t{14:F5}\t{15:F5}\n", new object[]
		                           {
			this.m00,
			this.m01,
			this.m02,
			this.m03,
			this.m10,
			this.m11,
			this.m12,
			this.m13,
			this.m20,
			this.m21,
			this.m22,
			this.m23,
			this.m30,
			this.m31,
			this.m32,
			this.m33
		});
	}

}
