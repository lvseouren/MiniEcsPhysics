using System;
using UnityEngine;
using XFixMath.NET;

public struct XVector2
{
    public XFix64 x;
    public XFix64 y;

    //public XVector2(float x, float y)
    //{
    //    this.x = MathX.FixToInt(x * XDefine.UNIT_CM_PER_METRE);
    //    this.y = MathX.FixToInt(y * XDefine.UNIT_CM_PER_METRE);
    //}
    public XVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    //TODO 移至LBattleUtils
    public Vector2 ToUnityVector2()
    {
        return new Vector2(MathX.ConvertToUnityValueFromCM(x), MathX.ConvertToUnityValueFromCM(y));
    }
    //
    // Static Properties
    //
    public static XVector2 down
    {
        get
        {
            return new XVector2(0, -1);
        }
    }

    public static XVector2 left
    {
        get
        {
            return new XVector2(-1, 0);
        }
    }

    public static XVector2 one
    {
        get
        {
            return new XVector2(1, 1);
        }
    }

    public static XVector2 right
    {
        get
        {
            return new XVector2(1, 0);
        }
    }

    public static XVector2 up
    {
        get
        {
            return new XVector2(0, 1);
        }
    }

    public static XVector2 zero
    {
        get
        {
            return new XVector2(0, 0);
        }
    }

    //
    // Properties
    //
    public float magnitude
    {
        get
        {
            return (float)Math.Sqrt(this.x * this.x + this.y * this.y);
        }
    }

    public XFVector3 normalized
    {
        get
        {
			return XVector2.Normalize(this);
        }
    }

	public static XFVector3 Normalize(XVector2 value)
	{
		XFVector3 temp = new XFVector3 (value);
		return temp.normalized;
	}

    public int sqrMagnitude
    {
        get
        {
            return this.x * this.x + this.y * this.y;
        }
    }

    //
    // Indexer
    //
    public int this[int index]
    {
        get
        {
            if (index == 0)
            {
                return this.x;
            }
            if (index != 1)
            {
                throw new IndexOutOfRangeException("Invalid XVector2 index!");
            }
            return this.y;
        }
        set
        {
            if (index != 0)
            {
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("Invalid XVector2 index!");
                }
                this.y = value;
            }
            else
            {
                this.x = value;
            }
        }
    }


    //
    // Static Methods
    //
    //TODO 转整型？
    public static float Angle(XVector2 from, XVector2 to)
    {
        return (float)Math.Acos(MathX.Clamp(XFVector3.Dot(from.normalized, to.normalized), -1, 1)) * 57.29578f;
    }

    public static XVector2 ClampMagnitude(XVector2 vector, int maxLength)
    {
        if (vector.sqrMagnitude > maxLength * maxLength)
        {
            return vector.normalized.ToXVector2() * maxLength;
        }
        return vector;
    }

    public static int Distance(XVector2 a, XVector2 b)
    {
		return MathX.FixToInt((a - b).magnitude);
    }

    public static int Dot(XVector2 lhs, XVector2 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }

    //public static XVector2 Lerp(XVector2 from, XVector2 to, float t)
    //{
    //    t = Mathf.Clamp01(t);
    //    return new XVector2(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
    //}

    public static XVector2 Max(XVector2 lhs, XVector2 rhs)
    {
        return new XVector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    public static XVector2 Min(XVector2 lhs, XVector2 rhs)
    {
        return new XVector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    public static XVector2 MoveTowards(XVector2 current, XVector2 target, int maxDistanceDelta)
    {
        XVector2 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    public static XVector2 Reflect(XVector2 inDirection, XVector2 inNormal)
    {
        return -2 * XVector2.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    public static XVector2 Scale(XVector2 a, XVector2 b)
    {
        return new XVector2(a.x * b.x, a.y * b.y);
    }

    //[ExcludeFromDocs]
    //public static XVector2 SmoothDamp(XVector2 current, XVector2 target, ref XVector2 currentVelocity, int smoothTime)
    //{
    //    int deltaTime = Time.deltaTime;
    //    int maxSpeed = int.PositiveInfinity;
    //    return XVector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    //}

    //[ExcludeFromDocs]
    //public static XVector2 SmoothDamp(XVector2 current, XVector2 target, ref XVector2 currentVelocity, int smoothTime, int maxSpeed)
    //{
    //    int deltaTime = Time.deltaTime;
    //    return XVector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    //}

    //public static XVector2 SmoothDamp(XVector2 current, XVector2 target, ref XVector2 currentVelocity, int smoothTime, [DefaultValue("Mathf.Infinity")] int maxSpeed, [DefaultValue("Time.deltaTime")] int deltaTime)
    //{
    //    smoothTime = Mathf.Max(0.0001f, smoothTime);
    //    int num = 2f / smoothTime;
    //    int num2 = num * deltaTime;
    //    int d = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
    //    XVector2 vector = current - target;
    //    XVector2 vector2 = target;
    //    int maxLength = maxSpeed * smoothTime;
    //    vector = XVector2.ClampMagnitude(vector, maxLength);
    //    target = current - vector;
    //    XVector2 vector3 = (currentVelocity + num * vector) * deltaTime;
    //    currentVelocity = (currentVelocity - num * vector3) * d;
    //    XVector2 vector4 = target + (vector + vector3) * d;
    //    if (XVector2.Dot(vector2 - current, vector4 - vector2) > 0f)
    //    {
    //        vector4 = vector2;
    //        currentVelocity = (vector4 - vector2) / deltaTime;
    //    }
    //    return vector4;
    //}

    public static int SqrMagnitude(XVector2 a)
    {
        return a.x * a.x + a.y * a.y;
    }

    //
    // Methods
    //
    public override bool Equals(object other)
    {
        if (!(other is XVector2))
        {
            return false;
        }
        XVector2 vector = (XVector2)other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public void Normalize()
    {
        float magnitude = this.magnitude;
        if (magnitude > 1)
        {
			x = MathX.FixToInt(x / magnitude);
			y = MathX.FixToInt(y / magnitude);
        }
        else
        {
            x = 0;
            y = 0;
        }
    }

    public void Scale(XVector2 scale)
    {
        this.x *= scale.x;
        this.y *= scale.y;
    }

    public void Set(int new_x, int new_y)
    {
        this.x = new_x;
        this.y = new_y;
    }

    public int SqrMagnitude()
    {
        return this.x * this.x + this.y * this.y;
    }

    //public string ToString(string format)
    //{
    //    return string.Format("({0}, {1})", new object[]
    //        {
    //            this.x.ToString (format),
    //            this.y.ToString (format)
    //        });
    //}

    public override string ToString()
    {
        int dx = x / XDefine.UNIT_CM_PER_METRE;
        int dy = y / XDefine.UNIT_CM_PER_METRE;
		return string.Format("XVector2({0}, {1})", new object[]
            {
               (( dx==0 && x<0)?"-":"")+ dx +"."+ Math.Abs(x%XDefine.UNIT_CM_PER_METRE),
               (( dy==0 && y<0)?"-":"")+ dy +"."+ Math.Abs(y%XDefine.UNIT_CM_PER_METRE)
            });
    }
    
    public string ToOriString()
    {
        
        return $"XVector2({x}, {y})";
    }

    //
    // Operators
    //
    public static XVector2 operator +(XVector2 a, XVector2 b)
    {
        return new XVector2(a.x + b.x, a.y + b.y);
    }

	public static XVector2 operator +(XVector2 a, XFVector3 b)
	{
		return new XVector2(a.x + MathX.FixToInt(b.x*XDefine.UNIT_CM_PER_METRE), a.y + MathX.FixToInt(b.z*XDefine.UNIT_CM_PER_METRE));
	}
    public static XVector2 operator -(XVector2 a, XFVector3 b)
    {
		return new XVector2(a.x - MathX.FixToInt(b.x * XDefine.UNIT_CM_PER_METRE), a.y - MathX.FixToInt(b.z * XDefine.UNIT_CM_PER_METRE));
    }

    public static XVector2 operator /(XVector2 a, int d)
    {
        return new XVector2(a.x / d, a.y / d);
    }
    public static XVector2 operator /(XVector2 a, float d)
    {
		return new XVector2(MathX.FixToInt(a.x / d ), MathX.FixToInt(a.y / d) );
    }
    public static bool operator ==(XVector2 lhs, XVector2 rhs)
    {
        return XVector2.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
    } 

    public static implicit operator XVector2(XVector3 v)
    {
        return new XVector2(v.x, v.y);
    }

    //public static implicit operator XDVector3(XVector2 v)
    //{
    //    return new XDVector3(v.x, v.y, 0);
    //}

    public static bool operator !=(XVector2 lhs, XVector2 rhs)
    {
        return XVector2.SqrMagnitude(lhs - rhs) >= 9.99999944E-11f;
    }

    public static XVector2 operator *(XVector2 a, int d)
    {
        return new XVector2(a.x * d, a.y * d);
    }

    public static XVector2 operator *(int d, XVector2 a)
    {
        return new XVector2(a.x * d, a.y * d);
    }

    public static XVector2 operator -(XVector2 a, XVector2 b)
    {
        return new XVector2(a.x - b.x, a.y - b.y);
    }

    public static XVector2 operator -(XVector2 a)
    {
        return new XVector2(-a.x, -a.y);
    }
}
