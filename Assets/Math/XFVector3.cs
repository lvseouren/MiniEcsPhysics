using System;
using UnityEngine;

/// <summary>
/// 精度为0.01的浮点vector3
/// 这个结构与XVector3 的数值比例为 100 ： 1
/// </summary>
public struct XFVector3
{
    public float x;
    public float y;
    public float z;


	//转换为2位精度
	public static float FloatTransform(float t){
        return ((float)(Math.Round(t * XDefine.UNIT_CM_PER_METRE + 0.0001f))) / XDefine.UNIT_CM_PER_METRE;
	}

	/// <summary>
	/// 普通浮点数转换为两位精度浮点
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	public XFVector3(float x,float y,float z){
		this.x = FloatTransform(x);
		this.y = FloatTransform(y);
		this.z = FloatTransform(z);
	}

    /// <summary>
    /// XVector3转换为XFVector3
    /// </summary>
	public XFVector3(XVector3 rhsV)
    {
		this.x = (float)rhsV.x/XDefine.UNIT_CM_PER_METRE;
        this.y = (float)rhsV.y / XDefine.UNIT_CM_PER_METRE;
        this.z = (float)rhsV.z / XDefine.UNIT_CM_PER_METRE;
    }

	public XFVector3(XVector2 rhsV){
        this.x = (float)rhsV.x / XDefine.UNIT_CM_PER_METRE;
		this.y = 0;
        this.z = (float)rhsV.y / XDefine.UNIT_CM_PER_METRE;
	}

    /// <summary>
    /// unity3d vector3好像已经处理过精度了
    /// </summary>
    /// <param name="vect"></param>
	public XFVector3(Vector3 vect)
    {
		this.x = vect.x;
		this.y = vect.y;
		this.z = vect.z;
    }

    public Vector3 ToUnityVector3()
    {
        return new Vector3(x,y,z);
    }

	public XVector3 ToXVector3(){
		return new XVector3 (ToUnityVector3());
	}

	public XVector2 ToXVector2(){
		return new XVector2 (MathX.FixToInt(x*XDefine.UNIT_CM_PER_METRE),MathX.FixToInt(z*XDefine.UNIT_CM_PER_METRE));
	}

    //
    // Properties
    //
	public static XFVector3 back
    {
        get
        {
			return new XFVector3(0, 0, -1);
        }
    }

	public static XFVector3 down
    {
        get
        {
			return new XFVector3(0, -1, 0);
        }
    }

    //public static XDVector3 forward
    //{
    //    get
    //    {
    //        return new XDVector3(0, 0, 1);
    //    }
    //}

	public static XFVector3 CreateXFVector3(Vector3 vec3)
    {
		return new XFVector3(vec3);
    }
	public static XFVector3 CreateXFVector3(int x, int y, int z)
    {
		return new XFVector3(x, y, z);
    }
	public static XFVector3 CreateXFVector3(float x,float y,float z)
    {
		return new XFVector3(x,y,z);
    }

	public static XFVector3 forward
    {
        get
        {
			return new XFVector3(0, 0, 1);
        }
    }

	public static XFVector3 left
    {
        get
        {
			return new XFVector3(-1, 0, 0);
        }
    }

	public static XFVector3 one
    {
        get
        {
			return new XFVector3(1, 1, 1);
        }
    }

	public static XFVector3 right
    {
        get
        {
			return new XFVector3(1, 0, 0);
        }
    }

	public static XFVector3 up
    {
        get
        {
			return new XFVector3(0, 1, 0);
        }
    }

	public static XFVector3 zero
    {
        get
        {
			return new XFVector3(0, 0, 0);
        }
    }
    public float magnitude
    {
        get
        {
			float value = (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            return FloatTransform(value);
        }
    }

	public XFVector3 normalized
    {
        get
        {
			return XFVector3.Normalize(this);
        }
    }

    public float sqrMagnitude
    {
        get
        {
			float value = this.x * this.x + this.y * this.y + this.z * this.z;
            return FloatTransform(value);
        }
    }

    //
    // Indexer
    //
    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                case 2:
                    return this.z;
                default:
                    throw new IndexOutOfRangeException("Invalid XDVector3 index!");
            }
        }
        set
        {
            switch (index)
            {
                case 0:
				this.x = FloatTransform(value);
                    break;
                case 1:
				this.y = FloatTransform(value);
                    break;
                case 2:
				this.z = FloatTransform(value);
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid XDVector3 index!");
            }
        }
    }

    //
    // Static Methods
    //
	public static float Angle(XFVector3 from, XFVector3 to)
    {
		float value = (float)(Math.Acos(MathX.Clamp(XFVector3.Dot(from.normalized, to.normalized), -1, 1)) * 57.29578f);
		return FloatTransform(value);
    }

    //[Obsolete("Use XDVector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
    //public static int AngleBetween(XDVector3 from, XDVector3 to)
    //{
    //    return MathX.Acos(MathX.Clamp(XDVector3.Dot(from.normalized, to.normalized), -1f, 1f));
    //}

	public static XFVector3 ClampMagnitude(XFVector3 vector, float maxLength)
    {
		maxLength = FloatTransform(maxLength);
        if (vector.sqrMagnitude > maxLength * maxLength)
        {
            return vector.normalized * maxLength;
        }
        return vector;
    }

	public static XFVector3 Cross(XFVector3 lhs, XFVector3 rhs)
    {
		return new XFVector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
    }

	public static float Distance(XFVector3 a, XFVector3 b)
    {
		XFVector3 vector = new XFVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		float value = (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
		return FloatTransform(value);
    }

	public static float Dot(XFVector3 lhs, XFVector3 rhs)
    {
		float value = lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		return FloatTransform(value);
    }

    //[Obsolete("Use XDVector3.ProjectOnPlane instead.")]
    //public static XDVector3 Exclude(XDVector3 excludeThis, XDVector3 fromThat)
    //{
    //    return fromThat - XDVector3.Project(fromThat, excludeThis);
    //}

	public static XFVector3 Lerp(XFVector3 from, XFVector3 to, float t)
    {
        t = MathX.Clamp01(t);
		return new XFVector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
    }

	public static float Magnitude(XFVector3 a)
    {
		float value = (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
		return FloatTransform(value);
    }

	public static XFVector3 Max(XFVector3 lhs, XFVector3 rhs)
    {
		return new XFVector3(MathX.Max(lhs.x, rhs.x), MathX.Max(lhs.y, rhs.y), MathX.Max(lhs.z, rhs.z));
    }

	public static XFVector3 Min(XFVector3 lhs, XFVector3 rhs)
    {
		return new XFVector3(MathX.Min(lhs.x, rhs.x), MathX.Min(lhs.y, rhs.y), MathX.Min(lhs.z, rhs.z));
    }

	public static XFVector3 MoveTowards(XFVector3 current, XFVector3 target, float maxDistanceDelta)
    {
		maxDistanceDelta = FloatTransform (maxDistanceDelta);
		XFVector3 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

	public static XFVector3 Normalize(XFVector3 value)
    {
		float num = XFVector3.Magnitude(value);
        if (num > 1E-05f)
        {

			return (value / num);
        }
		return XFVector3.zero;
    }

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent, ref XDVector3 binormal)
    //{
    //    XDVector3.Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
    //}

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent)
    //{
    //    XDVector3.Internal_OrthoNormalize2(ref normal, ref tangent);
    //}

	public static XFVector3 Project(XFVector3 vector, XFVector3 onNormal)
    {
		float num = XFVector3.Dot(onNormal, vector);
        if (num < MathX.Epsilon)
        {
			return XFVector3.zero;
        }
		return onNormal * XFVector3.Dot(vector, onNormal) / num;
    }

	public static XFVector3 ProjectOnPlane(XFVector3 vector, XFVector3 planeNormal)
    {
		return vector - XFVector3.Project(vector, planeNormal);
    }

	public static XFVector3 Reflect(XFVector3 inDirection, XFVector3 inNormal)
    {
		return -2 * XFVector3.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    //public static XDVector3 RotateTowards(XDVector3 current, XDVector3 target, int maxRadiansDelta, int maxMagnitudeDelta)
    //{
    //    return XDVector3.INTERNAL_CALL_RotateTowards(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta);
    //}

	public static XFVector3 Scale(XFVector3 a, XFVector3 b)
    {
		return new XFVector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    //public static XDVector3 Slerp(XDVector3 from, XDVector3 to, int t)
    //{
    //    return XDVector3.INTERNAL_CALL_Slerp(ref from, ref to, t);
    //}

    //[ExcludeFromDocs]
    //public static XDVector3 SmoothDamp(XDVector3 current, XDVector3 target, ref XDVector3 currentVelocity, int smoothTime)
    //{
    //    int deltaTime = Time.deltaTime;
    //    int maxSpeed = int.PositiveInfinity;
    //    return XDVector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    //}

    //public static XDVector3 SmoothDamp(XDVector3 current, XDVector3 target, ref XDVector3 currentVelocity, int smoothTime, [DefaultValue("Mathf.Infinity")] int maxSpeed, [DefaultValue("Time.deltaTime")] int deltaTime)
    //{
    //    smoothTime = MathX.Max(0.0001f, smoothTime);
    //    int num = 2f / smoothTime;
    //    int num2 = num * deltaTime;
    //    int d = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
    //    XDVector3 vector = current - target;
    //    XDVector3 vector2 = target;
    //    int maxLength = maxSpeed * smoothTime;
    //    vector = XDVector3.ClampMagnitude(vector, maxLength);
    //    target = current - vector;
    //    XDVector3 vector3 = (currentVelocity + num * vector) * deltaTime;
    //    currentVelocity = (currentVelocity - num * vector3) * d;
    //    XDVector3 vector4 = target + (vector + vector3) * d;
    //    if (XDVector3.Dot(vector2 - current, vector4 - vector2) > 0f)
    //    {
    //        vector4 = vector2;
    //        currentVelocity = (vector4 - vector2) / deltaTime;
    //    }
    //    return vector4;
    //}

    //[ExcludeFromDocs]
    //public static XDVector3 SmoothDamp(XDVector3 current, XDVector3 target, ref XDVector3 currentVelocity, int smoothTime, int maxSpeed)
    //{
    //    int deltaTime = Time.deltaTime;
    //    return XDVector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    //}

	public static float SqrMagnitude(XFVector3 a)
    {
		float value = a.x * a.x + a.y * a.y + a.z * a.z;
		return FloatTransform(value);
    }

    //
    // Methods
    //
    public override bool Equals(object other)
    {
		if (!(other is XFVector3))
        {
            return false;
        }
		XFVector3 vector = (XFVector3)other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y) && this.z.Equals(vector.z);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
    }

    public void Normalize()
    {
		float num = XFVector3.Magnitude(this);
        if (num > 1E-05f)
        {
            this /= num;
        }
        else
        {
			this = XFVector3.zero;
        }
    }

	public void Scale(XFVector3 scale)
    {
		this.x = FloatTransform (this.x * scale.x);
		this.y = FloatTransform (this.y * scale.y);
		this.z = FloatTransform (this.z * scale.z);
    }

	public void Set(float new_x, float new_y, float new_z)
    {
		this.x = FloatTransform(new_x);
		this.y = FloatTransform(new_y);
        this.z = FloatTransform(new_z);
    }

    public override string ToString()
    {
		return string.Format ("XFVector3({0}, {1}, {2})", x, y, z);
    }

    //
    // Operators
    //
	public static XFVector3 operator +(XFVector3 a, XFVector3 b)
    {
		return new XFVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

	public static XFVector3 operator /(XFVector3 a, int d)
    {
		return new XFVector3(a.x / d, a.y / d, a.z / d);
    }
	public static XFVector3 operator /(XFVector3 a, float d)
    {
		return new XFVector3((a.x / d), (a.y / d), (a.z / d));
    }

	public static bool operator ==(XFVector3 lhs, XFVector3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

	public static bool operator !=(XFVector3 lhs, XFVector3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

	public static XFVector3 operator *(XFVector3 a, int d)
    {
		return new XFVector3(a.x * d, a.y * d, a.z * d);
    }

	public static XFVector3 operator *(XFVector3 a, float d)
    {
		return new XFVector3(a.x * d, a.y * d, a.z * d);
    }
	public static XFVector3 operator *(float d, XFVector3 a)
    {
        return CreateXFVector3(a.x * d, a.y * d, a.z * d);
    }

	public static XFVector3 operator *(int d, XFVector3 a)
    {
		return CreateXFVector3(a.x * d, a.y * d, a.z * d);
    }

	public static XFVector3 operator -(XFVector3 a, XFVector3 b)
    {
		return new XFVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

	public static XFVector3 operator -(XFVector3 a)
    {
		return new XFVector3(-a.x, -a.y, -a.z);
    }
}