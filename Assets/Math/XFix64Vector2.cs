#if true
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using XFixMath.NET;

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

    public XFix64Vector3 normalized
    {
        get
        {
            return Normalize(this);
        }
    }

	public static XFix64Vector3 Normalize(XFix64Vector2 value)
	{
        XFix64Vector3 temp = new XFix64Vector3 (value);
		return temp.normalized;
	}

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
    public static XFix64 Angle(XFix64Vector2 from, XFix64Vector2 to)
    {
        return XFix64.Acos(XFix64.Clamp(XFix64Vector3.Dot(from.normalized, to.normalized), -XFix64.One, XFix64.One));
    }

    public static XFix64Vector2 ClampMagnitude(XFix64Vector2 vector, int maxLength)
    {
        if (vector.sqrMagnitude > maxLength * (XFix64)maxLength)
        {
            return vector.normalized.ToVector2() * maxLength;
        }
        return vector;
    }

    public static XFix64 Distance(XFix64Vector2 a, XFix64Vector2 b)
    {
        return (a - b).magnitude;
    }

    public static XFix64 Dot(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y;
    }

    public static XFix64Vector2 Max(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new XFix64Vector2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
    }

    public static XFix64Vector2 Min(XFix64Vector2 lhs, XFix64Vector2 rhs)
    {
        return new XFix64Vector2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
    }

    public static XFix64Vector2 MoveTowards(XFix64Vector2 current, XFix64Vector2 target, int maxDistanceDelta)
    {
        XFix64Vector2 a = target - current;
        float magnitude = a.magnitude;
        if (magnitude <= maxDistanceDelta || magnitude == 0)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    public static XFix64Vector2 Reflect(XFix64Vector2 inDirection, XFix64Vector2 inNormal)
    {
        return -2 * XFix64Vector2.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    public static XFix64Vector2 Scale(XFix64Vector2 a, XFix64Vector2 b)
    {
        return new XFix64Vector2(a.x * b.x, a.y * b.y);
    }

    public static int SqrMagnitude(XFix64Vector2 a)
    {
        return a.x * a.x + a.y * a.y;
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