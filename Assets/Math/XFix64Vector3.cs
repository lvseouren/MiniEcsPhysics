#if true
using System;
using Unity.Burst;
using UnityEngine;
using XFixMath.NET;


[Serializable]
[BurstCompile]
public struct XFix64Vector3
{

    public XFix64 x;
    public XFix64 y;
    public XFix64 z;

    /// <summary>
    /// 输入单位为厘米
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public XFix64Vector3(XFix64 x, XFix64 y, XFix64 z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public XFix64Vector3(Vector3 vect)
    {
        this.x = (XFix64)vect.x;
        this.y = (XFix64)vect.y;
        this.z = (XFix64)vect.z;
    }

    public XFix64Vector3(XFix64Vector2 v2)
    {
        this.x = v2.x;
        this.y = 0;
        this.z = v2.y;
    }
    public XFix64Vector3 Clone()
    {
        XFix64Vector3 xvec = new XFix64Vector3();
        xvec.x = x;
        xvec.y = y;
        xvec.z = z;
        return xvec;
    }

    //TODO 移至LBattleUtils
    public Vector3 ToUnityVector3()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }

    public static readonly XFix64Vector3 undefined = new XFix64Vector3(XFix64.Zero, (XFix64)(-999999), XFix64.Zero);
    //
    // Properties
    //
    public static XFix64Vector3 back
    {
        get
        {
            return new XFix64Vector3(XFix64.Zero, XFix64.Zero, -XFix64.One);
        }
    }

    public static XFix64Vector3 down
    {
        get
        {
            return new XFix64Vector3(XFix64.Zero, -XFix64.One, XFix64.Zero);
        }
    }

    public static XFix64Vector3 forward
    {
        get
        {
            return new XFix64Vector3(XFix64.Zero, XFix64.Zero, XFix64.One);
        }
    }

    public static XFix64Vector3 left
    {
        get
        {
            return new XFix64Vector3(-XFix64.One, XFix64.Zero, XFix64.Zero);
        }
    }

    public static XFix64Vector3 one
    {
        get
        {
            return new XFix64Vector3(XFix64.One, XFix64.One, XFix64.One);
        }
    }

    public static XFix64Vector3 right
    {
        get
        {
            return new XFix64Vector3(XFix64.One, XFix64.Zero, XFix64.Zero);
        }
    }

    public static XFix64Vector3 up
    {
        get
        {
            return new XFix64Vector3(XFix64.Zero, XFix64.One, XFix64.Zero);
        }
    }

    public static XFix64Vector3 zero
    {
        get
        {
            return new XFix64Vector3(XFix64.Zero, XFix64.Zero, XFix64.Zero);
        }
    }
    public XFix64 magnitude
    {
        get
        {
            SqrMagnitude(in this, out var result);
            var result2 = XFix64.Sqrt(result);
            return result2;
        }
    }

	public XFix64Vector3 normalized
    {
        get
        {
            Normalize(in this, out var result);
            return result;
        }
    }

    public XFix64 sqrMagnitude
    {
        get
        {
            SqrMagnitude(in this, out var result);
            return result;            
        }
    }

    [BurstCompile]
    public static void CreateXVector3(in Vector3 vec3, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(vec3);
    }
    [BurstCompile]
    public static void CreateXVector3(int x, int y, int z, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(x, y, z);
    }

    [BurstCompile]
    public static void CreateXVector3(in XFix64 x, in XFix64 y, in XFix64 z, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(x, y, z);
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
    [BurstCompile]
    public static void Angle(in XFix64Vector3 from, in XFix64Vector3 to, out XFix64 result)
    {
        Dot(from.normalized, to.normalized, out var dotResult);
        result = XFix64.Acos(XFix64.Clamp(dotResult, -XFix64.One, XFix64.One));
    }

    [BurstCompile]
    public static void ClampMagnitude(in XFix64Vector3 vector, int maxLength, out XFix64Vector3 result)
    {
        if (vector.sqrMagnitude > maxLength * maxLength)
        {
            vector.Normalize();
            result = vector * maxLength;
            return;
        }
        result = vector;
    }

    [BurstCompile]
    public static void Cross(in XFix64Vector3 lhs, in XFix64Vector3 rhs, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
    }

    public XFix64Vector2 ToVector2()
    {
        return new XFix64Vector2(x, y);
    }

    [BurstCompile]
    public static void Distance(in XFix64Vector3 a, in XFix64Vector3 b, out XFix64 result)
    {
        XFix64 x = (a.x - b.x);
        XFix64 y = (a.y - b.y);
        XFix64 z = (a.z - b.z);

        result = XFix64.Sqrt(x * x + y * y + z * z);  
    }

    [BurstCompile]
    public static void Dot(in XFix64Vector3 lhs, in XFix64Vector3 rhs, out XFix64 result)
    {
        result = lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    [BurstCompile]
    public static void Lerp(in XFix64Vector3 from, in XFix64Vector3 to, in XFix64 tt, out XFix64Vector3 result)
    {
        var t = XFix64.Clamp01(tt);
		result = new XFix64Vector3((from.x + (to.x - from.x) * t), (from.y + (to.y - from.y) * t), (from.z + (to.z - from.z) * t));
    }

    [BurstCompile]
    public static void Magnitude(in XFix64Vector3 a, out XFix64 result)
    {
        SqrMagnitude(a, out var temp);
        result = XFix64.Sqrt(temp);
    }

    [BurstCompile]
    public static void Max(in XFix64Vector3 lhs, in XFix64Vector3 rhs, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(XFix64.Max(lhs.x, rhs.x), XFix64.Max(lhs.y, rhs.y), XFix64.Max(lhs.z, rhs.z));
    }

    [BurstCompile]
    public static void Min(in XFix64Vector3 lhs, in XFix64Vector3 rhs, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(XFix64.Min(lhs.x, rhs.x), XFix64.Min(lhs.y, rhs.y), XFix64.Min(lhs.z, rhs.z));
    }

    [BurstCompile]
    public static void MoveTowards(in XFix64Vector3 current, in XFix64Vector3 target, in XFix64 maxDistanceDelta, out XFix64Vector3 result)
    {
        XFix64Vector3 a = target - current;
        XFix64 magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude.Equals(XFix64.Zero))
        {
            result = target;
            return;
        }
        result = current + (a / magnitude * maxDistanceDelta);
        return;
    }

    [BurstCompile]
    public static void Normalize(in XFix64Vector3 value, out XFix64Vector3 result)
    {
        var magnitude = value.magnitude;
        if (magnitude == XFix64.Zero)
        {
            result = value;
            return;
        }
        XFix64Vector3 v = value / magnitude;
        result = v;
    }

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent, ref XDVector3 binormal)
    //{
    //    XDVector3.Internal_OrthoNormalize3(ref normal, ref tangent, ref binormal);
    //}

    //public static void OrthoNormalize(ref XDVector3 normal, ref XDVector3 tangent)
    //{
    //    XDVector3.Internal_OrthoNormalize2(ref normal, ref tangent);
    //}

    [BurstCompile]
    public static void Project(in XFix64Vector3 vector, in XFix64Vector3 onNormal, out XFix64Vector3 result)
    {
        Dot(onNormal, onNormal, out var num);
        if (num == XFix64.Zero)
        {
            result = zero;
            return;
        }
        Dot(vector, onNormal, out var dotResult);
        result = onNormal * dotResult / num;
    }

    [BurstCompile]
    public static void ProjectOnPlane(in XFix64Vector3 vector, in XFix64Vector3 planeNormal, out XFix64Vector3 result)
    {
        Project(vector, planeNormal, out var temp);
        result = vector - temp;
    }

    [BurstCompile]
    public static void Reflect(in XFix64Vector3 inDirection, in XFix64Vector3 inNormal, out XFix64Vector3 result)
    {
        Dot(inNormal, inDirection, out var dotResult);
        result = -2 * dotResult * inNormal + inDirection;
    }

    [BurstCompile]
    public static void Scale(in XFix64Vector3 a, in XFix64Vector3 b, out XFix64Vector3 result)
    {
        result = new XFix64Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    //public static XFix64 SqrMagnitude(XFix64Vector3 a)
    //{
    //    XFix64 x1 = a.x;
    //    XFix64 y1 = a.y;
    //    XFix64 z1 = a.z;
    //    return x1 * x1 + y1 * y1 + z1 * z1;
    //}

    [BurstCompile]
    public static void SqrMagnitude(in XFix64Vector3 a, out XFix64 result)
    {
        XFix64 x1 = a.x;
        XFix64 y1 = a.y;
        XFix64 z1 = a.z;
        result = x1 * x1 + y1 * y1 + z1 * z1;
    }

    //
    // Methods
    //
    public override bool Equals(object other)
    {
        if (!(other is XFix64Vector3))
        {
            return false;
        }
        XFix64Vector3 vector = (XFix64Vector3)other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y) && this.z.Equals(vector.z);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
    }

    public void Normalize()
    {
        this = normalized;
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
    public void SetMagnitudeOfVector(XFix64 velocity)
    {
        //XFix64Vector3 v3 = XDVector3.Normalize(this);
        //this.x = (int)(v3.x * velocity);
        //this.y = (int)(v3.y * velocity);
        //this.z = (int)(v3.z * velocity);

        this.Normalize();
        this.x = x * velocity;
		this.y = y * velocity;
		this.z = z * velocity;

    }

    public void Scale(XFix64Vector3 scale)
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

        return string.Format("XFix64Vector3({0}, {1}, {2}) ", x, y, z);
    }

    //
    // Operators
    //
    public static XFix64Vector3 operator +(XFix64Vector3 a, XFix64Vector3 b)
    {
        return new XFix64Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static XFix64Vector3 operator /(XFix64Vector3 a, int d)
    {
        return new XFix64Vector3(a.x / d, a.y / d, a.z / d);
    }
    public static XFix64Vector3 operator /(XFix64Vector3 a, XFix64 d)
    {
		return new XFix64Vector3(a.x / d, a.y / d, a.z / d);
    }

    public static bool operator ==(XFix64Vector3 lhs, XFix64Vector3 rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
    }

    public static bool operator !=(XFix64Vector3 lhs, XFix64Vector3 rhs)
    {
        return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
    }

    public static XFix64Vector3 operator *(XFix64Vector3 a, int d)
    {
        return new XFix64Vector3(a.x * d, a.y * d, a.z * d);
    }

    public static XFix64Vector3 operator *(XFix64Vector3 a, XFix64 d)
    {
        var x = a.x * d;
        var y = a.y * d;
        var z = a.z * d;
        CreateXVector3(in x, in y, in z, out var result);
        return result;
    }
    public static XFix64Vector3 operator *(XFix64 d, XFix64Vector3 a)
    {
        var x = a.x * d;
        var y = a.y * d;
        var z = a.z * d;
        CreateXVector3(in x, in y, in z, out var result);
        return result;      
    }

    public static XFix64Vector3 operator *(int d, XFix64Vector3 a)
    {
        var x = a.x * d;
        var y = a.y * d;
        var z = a.z * d;
        CreateXVector3(in x, in y, in z, out var result);
        return result;        
    }

    public static XFix64Vector3 operator %(XFix64Vector3 a, XFix64Vector3 b)
    {
        XFix64 nx = a.y * b.z - a.z * b.y;
        XFix64 ny = a.z * b.x - a.x * b.z;
        XFix64 nz = a.x * b.y - a.y * b.x;
        return new XFix64Vector3(nx,ny,nz);
    }
    public static XFix64Vector3 operator -(XFix64Vector3 a, XFix64Vector3 b)
    {
        return new XFix64Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static XFix64Vector3 operator -(XFix64Vector3 a)
    {
        return new XFix64Vector3(-a.x, -a.y, -a.z);
    }
}
#endif