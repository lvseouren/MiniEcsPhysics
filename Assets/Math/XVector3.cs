using System;
using UnityEngine;
/// <summary>
/// 单位是厘米
/// </summary>
[Serializable]
public struct XVector3
{

    public int x;
    public int y;
    public int z;

    /// <summary>
    /// 输入单位为厘米
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public XVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }


    /// <summary>
    /// 注意：此处输入单位为米！
    /// </summary>
    /// <param name="vect"></param>
    public XVector3(Vector3 vect)
    {
		this.x = MathX.FixToInt(vect.x * XDefine.UNIT_CM_PER_METRE);
		this.y = MathX.FixToInt(vect.y * XDefine.UNIT_CM_PER_METRE);
		this.z = MathX.FixToInt(vect.z * XDefine.UNIT_CM_PER_METRE);
    }
    public void FromUnityVector3(Vector3 vect)
    {
        this.x = MathX.FixToInt(vect.x * XDefine.UNIT_CM_PER_METRE);
        this.y = MathX.FixToInt(vect.y * XDefine.UNIT_CM_PER_METRE);
        this.z = MathX.FixToInt(vect.z * XDefine.UNIT_CM_PER_METRE);
    }
    public XVector3(XVector2 v2)
    {
        this.x = v2.x;
        this.y = 0;
        this.z = v2.y;
    }
    public XVector3 Clone()
    {
        XVector3 xvec = new XVector3();
        xvec.x = x;
        xvec.y = y;
        xvec.z = z;
        return xvec;
    }

    //TODO 移至LBattleUtils
    public Vector3 ToUnityVector3()
    {
        return new Vector3(MathX.ConvertToUnityValueFromCM(x), MathX.ConvertToUnityValueFromCM(y), MathX.ConvertToUnityValueFromCM(z));
    }

    //public PTVector3 ToPTVector3()
    //{
    //    PTVector3 v3 = new PTVector3();
    //    v3.X = x;
    //    v3.Y = y;
    //    v3.Z = z;
    //    return v3;
    //}


    public static readonly XVector3 undefined = new XVector3(0, -999999, 0);
    //
    // Properties
    //
    public static XVector3 back
    {
        get
        {
            return new XVector3(0, 0, -1);
        }
    }

    public static XVector3 down
    {
        get
        {
            return new XVector3(0, -1, 0);
        }
    }

    //public static XDVector3 forward
    //{
    //    get
    //    {
    //        return new XDVector3(0, 0, 1);
    //    }
    //}

    public static XVector3 CreateXVector3(Vector3 vec3)
    {
        return new XVector3(vec3);
    }
    public static XVector3 CreateXVector3(int x, int y, int z)
    {
        return new XVector3(x, y, z);
    }
    public static XVector3 CreateXVector3(float x,float y,float z)
    {
		return new XVector3(MathX.FixToInt(x),MathX.FixToInt(y),MathX.FixToInt(z));
    }

    public static XVector3 forward
    {
        get
        {
            return new XVector3(0, 0, 1);
        }
    }

    public static XVector3 left
    {
        get
        {
            return new XVector3(-1, 0, 0);
        }
    }

    public static XVector3 one
    {
        get
        {
            return new XVector3(1, 1, 1);
        }
    }

    public static XVector3 right
    {
        get
        {
            return new XVector3(1, 0, 0);
        }
    }

    public static XVector3 up
    {
        get
        {
            return new XVector3(0, 1, 0);
        }
    }

    public static XVector3 zero
    {
        get
        {
            return new XVector3(0, 0, 0);
        }
    }
    public float magnitude
    {
        get
        {
            return (float)Math.Sqrt(SqrMagnitude(this));
        }
    }

	public XFVector3 normalized
    {
        get
        {
            return XVector3.Normalize(this);
        }
    }

    public long sqrMagnitude
    {
        get
        {
          
            return SqrMagnitude(this);
        }
    }

    //
    // Indexer
    //
    public int this[int index]
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
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                case 2:
                    this.z = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid XDVector3 index!");
            }
        }
    }

    //
    // Static Methods
    //
    public static int Angle(XVector3 from, XVector3 to)
    {
		return MathX.FixToInt((float)Math.Acos(MathX.Clamp(XFVector3.Dot(from.normalized, to.normalized), -1, 1)) * 57.29578f);
    }

    //[Obsolete("Use XDVector3.Angle instead. AngleBetween uses radians instead of degrees and was deprecated for this reason")]
    //public static int AngleBetween(XDVector3 from, XDVector3 to)
    //{
    //    return MathX.Acos(MathX.Clamp(XDVector3.Dot(from.normalized, to.normalized), -1f, 1f));
    //}

    public static XVector3 ClampMagnitude(XVector3 vector, int maxLength)
    {
        if (vector.sqrMagnitude > maxLength * maxLength)
        {
            vector.Normalize();
            return vector * maxLength;
        }
        return vector;
    }

    public static XVector3 Cross(XVector3 lhs, XVector3 rhs)
    {
        return new XVector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
    }

    public static int Distance(XVector3 a, XVector3 b)
    {
        long x = (a.x - b.x);
        long y = (a.y - b.y);
        long z = (a.z - b.z);

        return MathX.FixToInt((float)Math.Sqrt(x * x + y * y + z * z));

  //      XVector3 vector = new XVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		//return MathX.FixToInt((float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + (long)vector.z * (long)vector.z));
    }

    public static float Dot(XVector3 lhs, XVector3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + (long)lhs.z * (long)rhs.z;
    }

    //[Obsolete("Use XDVector3.ProjectOnPlane instead.")]
    //public static XDVector3 Exclude(XDVector3 excludeThis, XDVector3 fromThat)
    //{
    //    return fromThat - XDVector3.Project(fromThat, excludeThis);
    //}

    public static XVector3 Lerp(XVector3 from, XVector3 to, float t)
    {
        t = MathX.Clamp01(t);
		return new XVector3(MathX.FixToInt(from.x + (to.x - from.x) * t), MathX.FixToInt(from.y + (to.y - from.y) * t), MathX.FixToInt(from.z + (to.z - from.z) * t));
    }

    public static float Magnitude(XVector3 a)
    {
        return (float)Math.Sqrt(SqrMagnitude(a));
    }

    public static XVector3 Max(XVector3 lhs, XVector3 rhs)
    {
        return new XVector3(MathX.Max(lhs.x, rhs.x), MathX.Max(lhs.y, rhs.y), MathX.Max(lhs.z, rhs.z));
    }

    public static XVector3 Min(XVector3 lhs, XVector3 rhs)
    {
        return new XVector3(MathX.Min(lhs.x, rhs.x), MathX.Min(lhs.y, rhs.y), MathX.Min(lhs.z, rhs.z));
    }

    public static XVector3 MoveTowards(XVector3 current, XVector3 target, int maxDistanceDelta)
    {
        XVector3 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    public static XFVector3 Normalize(XVector3 value)
    {
		XFVector3 v = new XFVector3 (value);
		return v.normalized;
    }

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent, ref XDVector3 binormal)
    //{
    //    XDVector3.Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
    //}

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent)
    //{
    //    XDVector3.Internal_OrthoNormalize2(ref normal, ref tangent);
    //}

    public static XVector3 Project(XVector3 vector, XVector3 onNormal)
    {
        float num = XVector3.Dot(onNormal, onNormal);
        if (num < MathX.Epsilon)
        {
            return XVector3.zero;
        }
        return onNormal * XVector3.Dot(vector, onNormal) / num;
    }

    public static XVector3 ProjectOnPlane(XVector3 vector, XVector3 planeNormal)
    {
        return vector - XVector3.Project(vector, planeNormal);
    }

    public static XVector3 Reflect(XVector3 inDirection, XVector3 inNormal)
    {
        return -2 * XVector3.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    //public static XDVector3 RotateTowards(XDVector3 current, XDVector3 target, int maxRadiansDelta, int maxMagnitudeDelta)
    //{
    //    return XDVector3.INTERNAL_CALL_RotateTowards(ref current, ref target, maxRadiansDelta, maxMagnitudeDelta);
    //}

    public static XVector3 Scale(XVector3 a, XVector3 b)
    {
        return new XVector3(a.x * b.x, a.y * b.y, a.z * b.z);
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

    public static long SqrMagnitude(XVector3 a)
    {
        long x1 = a.x;
        long y1 = a.y;
        long z1 = a.z;
        return x1 * x1 + y1 * y1 + z1 * z1;
    }

    //
    // Methods
    //
    public override bool Equals(object other)
    {
        if (!(other is XVector3))
        {
            return false;
        }
        XVector3 vector = (XVector3)other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y) && this.z.Equals(vector.z);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
    }

    public void Normalize()
    {
        float num = MathX.FixMilli(XVector3.Magnitude(this) / 100f);
        if (num > 1E-05f)
        {
            this /= num;
        }
        else
        {
            this = XVector3.zero;
        }
    }
    //TODO
    //get direction of vector
    public void GetDirection()
    {
        this.Normalize();
    }
    //TODO
    //set the magnitude of the vector (without modifying direction,
    //apportioned across the existing vector's magnitudes
    public void SetMagnitudeOfVector(float velocity)
    {
        //XFVector3 v3 = XDVector3.Normalize(this);
        //this.x = (int)(v3.x * velocity);
        //this.y = (int)(v3.y * velocity);
        //this.z = (int)(v3.z * velocity);

        this.Normalize();
		this.x = MathX.FixToInt(x * velocity);
		this.y = MathX.FixToInt(y * velocity);
		this.z = MathX.FixToInt(z * velocity);

    }

    public void Scale(XVector3 scale)
    {
        this.x *= scale.x;
        this.y *= scale.y;
        this.z *= scale.z;
    }

    public void Set(int new_x, int new_y, int new_z)
    {
        this.x = new_x;
        this.y = new_y;
        this.z = new_z;
    }

    public override string ToString()
    {
        //int dx = x/XDefine.UNIT_CM_PER_METRE;
        //int dy = y/XDefine.UNIT_CM_PER_METRE;
        //int dz = z/XDefine.UNIT_CM_PER_METRE;
        ////return string.Format("XVector3({0}, {1}, {2}) ({3},{4},{5})", new object[]
        ////    {
        ////       (( dx==0 && x<0)?"-":"")+ dx +"."+ Math.Abs(x%XDefine.UNIT_CM_PER_METRE),
        ////       (( dy==0 && y<0)?"-":"")+ dy +"."+ Math.Abs(y%XDefine.UNIT_CM_PER_METRE),
        ////       (( dz==0 && z<0)?"-":"")+ dz +"."+ Math.Abs(z%XDefine.UNIT_CM_PER_METRE),
        ////    x,y,z,
        ////    });
        //return string.Format("XVector3({0}, {1}, {2}) ", new object[]
        //    {
        //       (( dx==0 && x<0)?"-":"")+ dx +"."+ Math.Abs(x%XDefine.UNIT_CM_PER_METRE),
        //       (( dy==0 && y<0)?"-":"")+ dy +"."+ Math.Abs(y%XDefine.UNIT_CM_PER_METRE),
        //       (( dz==0 && z<0)?"-":"")+ dz +"."+ Math.Abs(z%XDefine.UNIT_CM_PER_METRE)
        //    });

        return string.Format("XVector3({0}, {1}, {2}) ", new object[]
            {
               MathX.FillZero(x, XDefine.UNIT_CM_PER_METRE),
               MathX.FillZero(y, XDefine.UNIT_CM_PER_METRE),
               MathX.FillZero(z, XDefine.UNIT_CM_PER_METRE)
            });
    }

    //public string ToString(string format)
    //{
    //    return string.Format("XVector3({0}, {1}, {2})", new object[]
    //        {
    //            this.x.ToString (format),
    //            this.y.ToString (format),
    //            this.z.ToString (format)
    //        });
    //}

    //
    // Operators
    //
    public static XVector3 operator +(XVector3 a, XVector3 b)
    {
        return new XVector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

	public static XVector3 operator +(XVector3 a,XFVector3 b){
		return new XVector3 (a.x + MathX.FixToInt(b.x * XDefine.UNIT_CM_PER_METRE ), a.y + MathX.FixToInt(b.y * XDefine.UNIT_CM_PER_METRE), a.z + MathX.FixToInt(b.z * XDefine.UNIT_CM_PER_METRE));
	}

    public static XVector3 operator /(XVector3 a, int d)
    {
        return new XVector3(a.x / d, a.y / d, a.z / d);
    }
    public static XVector3 operator /(XVector3 a, float d)
    {
		return new XVector3(MathX.FixToInt(a.x / d), MathX.FixToInt(a.y / d), MathX.FixToInt(a.z / d));
    }

    public static bool operator ==(XVector3 lhs, XVector3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(XVector3 lhs, XVector3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

    public static XVector3 operator *(XVector3 a, int d)
    {
        return new XVector3(a.x * d, a.y * d, a.z * d);
    }

    public static XVector3 operator *(XVector3 a, float d)
    {
        return CreateXVector3(a.x * d, a.y * d, a.z * d);
    }
    public static XVector3 operator *(float d, XVector3 a)
    {
        return CreateXVector3(a.x * d, a.y * d, a.z * d);
    }

    public static XVector3 operator *(int d, XVector3 a)
    {
        return new XVector3(a.x * d, a.y * d, a.z * d);
    }

    public static XVector3 operator %(XVector3 a, XVector3 b)
    {
        int nx = a.y * b.z - a.z * b.y;
        int ny = a.z * b.x - a.x * b.z;
        int nz = a.x * b.y - a.y * b.x;
        return new XVector3(nx,ny,nz);
    }
    public static XVector3 operator -(XVector3 a, XVector3 b)
    {
        return new XVector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static XVector3 operator -(XVector3 a)
    {
        return new XVector3(-a.x, -a.y, -a.z);
    }
}
