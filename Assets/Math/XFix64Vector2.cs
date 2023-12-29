#if true
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using XFixMath.NET;

[BurstCompile]
public struct XFix64Vector2
{
    public XFix64 x;
    public XFix64 y;

    public XFix64Vector2(XFix64 v)
    {
        x = v;
        y = v;
    }

    public XFix64Vector2(XFix64 x, XFix64 y)
    {
        this.x = x;
        this.y = y;
    }

    public XFix64Vector2(int2 v)
    {
        x = v.x;
        y = v.y;
    }

    //public XFix64Vector2(uint2 v)
    //{
    //    x = v.x;
    //    y = v.y;
    //}

    /// <summary>Explicitly converts a single int value to a fp2 vector by converting it to fp and assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator XFix64Vector2(XFix64 v)
    {
        return new XFix64Vector2(v);
    }

    public static implicit operator XFix64Vector2(int2 v)
    {
        return new XFix64Vector2(v);
    }

    public static implicit operator XFix64Vector2(uint2 v)
    {
        return new XFix64Vector2(v.x, v.y);
    }

    //
    // Static Properties
    //
    public static XFix64Vector2 down
    {
        get
        {
            return new XFix64Vector2(XFix64.Zero, -XFix64.One);
        }
    }

    public static XFix64Vector2 left
    {
        get
        {
            return new XFix64Vector2(-XFix64.One, XFix64.Zero);
        }
    }

    public static XFix64Vector2 one
    {
        get
        {
            return new XFix64Vector2(XFix64.One, XFix64.One);
        }
    }

    public static XFix64Vector2 right
    {
        get
        {
            return new XFix64Vector2(XFix64.One, XFix64.Zero);
        }
    }

    public static XFix64Vector2 up
    {
        get
        {
            return new XFix64Vector2(XFix64.Zero, XFix64.One);
        }
    }

    public static XFix64Vector2 zero
    {
        get
        {
            return new XFix64Vector2(XFix64.Zero, XFix64.Zero);
        }
    }

    //
    // Properties
    //
    public XFix64 magnitude
    {
        get
        {
            return XFix64.Sqrt(this.x * this.x + this.y * this.y);
        }
    }

    [BurstCompile]
	public static void Normalize(in XFix64Vector2 value, out XFix64Vector3 result)
	{
        XFix64Vector3 temp = new XFix64Vector3 (value);
		result = temp.normalized;
	}

    public XFix64Vector3 normalized
    {
        get
        {
            Normalize(in this, out var result);
            return result;
        }
    }

    //public static XFix64Vector3 Normalize(XFix64Vector2 value)
    //{
    //    XFix64Vector3 temp = new XFix64Vector3(value);
    //    return temp.normalized;
    //}

    public XFix64 sqrMagnitude
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
                throw new IndexOutOfRangeException("Invalid XFix64Vector2 index!");
            }
            return this.y;
        }
        set
        {
            if (index != 0)
            {
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("Invalid XFix64Vector2 index!");
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
    [BurstCompile]
    public static void Angle(in XFix64Vector2 from, in XFix64Vector2 to, out XFix64 result)
    {
        XFix64Vector3.Dot(from.normalized, to.normalized, out var dotResult);
        result = XFix64.Acos(XFix64.Clamp(dotResult, -XFix64.One, XFix64.One));        
    }

    [BurstCompile]
    public static void ClampMagnitude(in XFix64Vector2 vector, int maxLength, out XFix64Vector2 result)
    {
        if (vector.sqrMagnitude > maxLength * (XFix64)maxLength)
        {
            result = vector.normalized.ToVector2() * maxLength;
            return;
        }
        result = vector;
    }

    [BurstCompile]
    public static void Distance(in XFix64Vector2 a, in XFix64Vector2 b, out XFix64 result)
    {
        result = (a - b).magnitude;
    }

    [BurstCompile]
    public static void Dot(in XFix64Vector2 lhs, in XFix64Vector2 rhs, out XFix64 result)
    {
        result = lhs.x * rhs.x + lhs.y * rhs.y;
    }

    [BurstCompile]
    public static void Max(in XFix64Vector2 lhs, in XFix64Vector2 rhs, out XFix64Vector2 result)
    {
        result = new XFix64Vector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    [BurstCompile]
    public static void Min(in XFix64Vector2 lhs, in XFix64Vector2 rhs, out XFix64Vector2 result)
    {
        result = new XFix64Vector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    [BurstCompile]
    public static void MoveTowards(in XFix64Vector2 current, in XFix64Vector2 target, int maxDistanceDelta, out XFix64Vector2 result)
    {
        XFix64Vector2 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0)
        {
            result = target;
            return;
        }
        result = current + a / magnitude * maxDistanceDelta;
    }

    [BurstCompile]
    public static void Reflect(in XFix64Vector2 inDirection, in XFix64Vector2 inNormal, out XFix64Vector2 result)
    {
        XFix64Vector2.Dot(inNormal, inDirection, out var dotResult);
        result = -2 * dotResult * inNormal + inDirection;
    }

    [BurstCompile]
    public static void Scale(in XFix64Vector2 a, in XFix64Vector2 b, out XFix64Vector2 result)
    {
        result = new XFix64Vector2(a.x * b.x, a.y * b.y);
    }

    [BurstCompile]
    public static void SqrMagnitude(in XFix64Vector2 a, out XFix64 result)
    {
        result = a.x * a.x + a.y * a.y;
    }

    //
    // Methods
    //
    public override bool Equals(object other)
    {
        if (!(other is XFix64Vector2))
        {
            return false;
        }
        XFix64Vector2 vector = (XFix64Vector2)other;
        return this.x.Equals(vector.x) && this.y.Equals(vector.y);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public void Normalize()
    {
        var mag = magnitude;
        if (mag != XFix64.Zero)
        {        
            x = x / mag;
            y = y / mag;
        }
    }

    public void Scale(XFix64Vector2 scale)
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

    public override string ToString()
    {
		return string.Format("XFix64Vector2({0}, {1})", x, y);
    }
    
    public string ToOriString()
    {
        
        return $"XFix64Vector2({x}, {y})";
    }

    //
    // Operators
    //
    public static XFix64Vector2 operator +(XFix64Vector2 a, XFix64Vector2 b)
    {
        return new XFix64Vector2(a.x + b.x, a.y + b.y);
    }

	public static XFix64Vector2 operator +(XFix64Vector2 a, XFix64Vector3 b)
	{
		return new XFix64Vector2(a.x + b.x, a.y + b.y);
	}
    public static XFix64Vector2 operator -(XFix64Vector2 a, XFix64Vector3 b)
    {
		return new XFix64Vector2(a.x - b.x, a.y - b.y);
    }

    public static XFix64Vector2 operator /(XFix64Vector2 a, int d)
    {
        return new XFix64Vector2(a.x / d, a.y / d);
    }

    public static XFix64Vector2 operator /(XFix64Vector2 a, XFix64 d)
    {
        return new XFix64Vector2(a.x / d, a.y / d);
    }
    public static bool operator ==(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return lhs.x.Equals(rhs.x) && lhs.y.Equals(rhs.y);
    } 

    public static implicit operator XFix64Vector2(XFix64Vector3 v)
    {
        return new XFix64Vector2(v.x, v.y);
    }

    //public static implicit operator XDVector3(XFix64Vector2 v)
    //{
    //    return new XDVector3(v.x, v.y, 0);
    //}

    public static bool operator !=(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return !(lhs == rhs);
    }

    public static XFix64Vector2 operator *(XFix64Vector2 a, XFix64 d)
    {
        return new XFix64Vector2(a.x * d, a.y * d);
    }

    public static XFix64Vector2 operator *(XFix64 d, XFix64Vector2 a)
    {
        return new XFix64Vector2(a.x * d, a.y * d);
    }

    public static XFix64Vector2 operator -(XFix64Vector2 a, XFix64Vector2 b)
    {
        return new XFix64Vector2(a.x - b.x, a.y - b.y);
    }

    public static XFix64Vector2 operator -(XFix64Vector2 a)
    {
        return new XFix64Vector2(-a.x, -a.y);
    }

    public static XFix64Vector2 operator -(XFix64Vector2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2(lhs.x - rhs, lhs.y - rhs);
    }

    public static XFix64Vector2 operator +(XFix64Vector2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2(lhs.x + rhs, lhs.y + rhs);
    }

    public static XFix64Vector2 operator *(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new XFix64Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
    }

    public static XFix64Vector2 operator /(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new XFix64Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
    }

    public static XFix64Vector2 operator %(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new XFix64Vector2(lhs.x % rhs.x, lhs.y % rhs.y);
    }

    public static bool2 operator <(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
    }
    public static bool2 operator >(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
    }
    public static bool2 operator <=(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
    }
    public static bool2 operator >=(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
    }

    public static XFix64Vector2 operator ++(XFix64Vector2 val)
    {
        return new XFix64Vector2(++val.x, ++val.y);
    }

    public static XFix64Vector2 operator --(XFix64Vector2 val)
    {
        return new XFix64Vector2(--val.x, --val.y);
    }

    public static XFix64Vector2 operator +(XFix64Vector2 val)
    {
        return +val;
    }
}
#endif