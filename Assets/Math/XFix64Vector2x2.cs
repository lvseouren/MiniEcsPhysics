
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using XFixMath.NET;
using static Unity.Mathematics.math;

public struct XFix64Vector2x2
{
    public XFix64Vector2 c0;
    public XFix64Vector2 c1;

    /// <summary>XFix64Vector2x2 identity transform.</summary>
    public static readonly XFix64Vector2x2 identity = new XFix64Vector2x2(1, 0, 0, 1);

    /// <summary>XFix64Vector2x2 zero value.</summary>
    public static readonly XFix64Vector2x2 zero;

    /// <summary>Constructs a XFix64Vector2x2 matrix from two XFix64Vector2 vectors.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public XFix64Vector2x2(XFix64Vector2 c0, XFix64Vector2 c1)
    {
        this.c0 = c0;
        this.c1 = c1;
    }

    /// <summary>Constructs a XFix64Vector2x2 matrix from 4 XFix64 values given in row-major order.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public XFix64Vector2x2(XFix64 m00, XFix64 m01,
                 XFix64 m10, XFix64 m11)
    {
        this.c0 = new XFix64Vector2(m00, m10);
        this.c1 = new XFix64Vector2(m01, m11);
    }

    /// <summary>Constructs a XFix64Vector2x2 matrix from a single XFix64 value by assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public XFix64Vector2x2(XFix64 v)
    {
        this.c0 = v;
        this.c1 = v;
    }

    /// <summary>Constructs a XFix64Vector2x2 matrix from a int2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public XFix64Vector2x2(int2x2 v)
    {
        this.c0 = (XFix64Vector2)v.c0;
        this.c1 = (XFix64Vector2)v.c1;
    }

    ///// <summary>Constructs a XFix64Vector2x2 matrix from a single uint value by converting it to XFix64 and assigning it to every component.</summary>
    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    //public XFix64Vector2x2(uint v)
    //{
    //    this.c0 = (XFix64Vector2)v;
    //    this.c1 = (XFix64Vector2)v;
    //}

    /// <summary>Constructs a XFix64Vector2x2 matrix from a uint2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public XFix64Vector2x2(uint2x2 v)
    {
        this.c0 = (XFix64Vector2)v.c0;
        this.c1 = (XFix64Vector2)v.c1;
    }


    /// <summary>Implicitly converts a single XFix64 value to a XFix64Vector2x2 matrix by assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator XFix64Vector2x2(XFix64 v)
    {
        return new XFix64Vector2x2(v);
    }

    /// <summary>Explicitly converts a single int value to a XFix64Vector2x2 matrix by converting it to XFix64 and assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator XFix64Vector2x2(int v)
    {
        return new XFix64Vector2x2((XFix64)v);
    }

    /// <summary>Explicitly converts a int2x2 matrix to a XFix64Vector2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator XFix64Vector2x2(int2x2 v)
    {
        return new XFix64Vector2x2(v);
    }

    /// <summary>Explicitly converts a single uint value to a XFix64Vector2x2 matrix by converting it to XFix64 and assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator XFix64Vector2x2(uint v)
    {
        return new XFix64Vector2x2((XFix64)v);
    }

    /// <summary>Explicitly converts a uint2x2 matrix to a XFix64Vector2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator XFix64Vector2x2(uint2x2 v)
    {
        return new XFix64Vector2x2(v);
    }


    /// <summary>Returns the result of a componentwise multiplication operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator *(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
    }

    /// <summary>Returns the result of a componentwise multiplication operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator *(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 * rhs, lhs.c1 * rhs);
    }

    /// <summary>Returns the result of a componentwise multiplication operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator *(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs * rhs.c0, lhs * rhs.c1);
    }


    /// <summary>Returns the result of a componentwise addition operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator +(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
    }

    /// <summary>Returns the result of a componentwise addition operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator +(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 + rhs, lhs.c1 + rhs);
    }

    /// <summary>Returns the result of a componentwise addition operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator +(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs + rhs.c0, lhs + rhs.c1);
    }


    /// <summary>Returns the result of a componentwise subtraction operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator -(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
    }

    /// <summary>Returns the result of a componentwise subtraction operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator -(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 - rhs, lhs.c1 - rhs);
    }

    /// <summary>Returns the result of a componentwise subtraction operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator -(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs - rhs.c0, lhs - rhs.c1);
    }


    /// <summary>Returns the result of a componentwise division operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator /(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
    }

    /// <summary>Returns the result of a componentwise division operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator /(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 / rhs, lhs.c1 / rhs);
    }

    /// <summary>Returns the result of a componentwise division operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator /(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs / rhs.c0, lhs / rhs.c1);
    }


    /// <summary>Returns the result of a componentwise modulus operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator %(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
    }

    /// <summary>Returns the result of a componentwise modulus operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator %(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new XFix64Vector2x2(lhs.c0 % rhs, lhs.c1 % rhs);
    }

    /// <summary>Returns the result of a componentwise modulus operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator %(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new XFix64Vector2x2(lhs % rhs.c0, lhs % rhs.c1);
    }


    /// <summary>Returns the result of a componentwise increment operation on a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator ++(XFix64Vector2x2 val)
    {
        return new XFix64Vector2x2(++val.c0, ++val.c1);
    }


    /// <summary>Returns the result of a componentwise decrement operation on a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator --(XFix64Vector2x2 val)
    {
        return new XFix64Vector2x2(--val.c0, --val.c1);
    }


    /// <summary>Returns the result of a componentwise less than operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
    }

    /// <summary>Returns the result of a componentwise less than operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
    }

    /// <summary>Returns the result of a componentwise less than operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
    }


    /// <summary>Returns the result of a componentwise less or equal operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <=(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
    }

    /// <summary>Returns the result of a componentwise less or equal operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <=(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
    }

    /// <summary>Returns the result of a componentwise less or equal operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator <=(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
    }


    /// <summary>Returns the result of a componentwise greater than operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
    }

    /// <summary>Returns the result of a componentwise greater than operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
    }

    /// <summary>Returns the result of a componentwise greater than operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
    }


    /// <summary>Returns the result of a componentwise greater or equal operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >=(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
    }

    /// <summary>Returns the result of a componentwise greater or equal operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >=(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
    }

    /// <summary>Returns the result of a componentwise greater or equal operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator >=(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
    }


    /// <summary>Returns the result of a componentwise unary minus operation on a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator -(XFix64Vector2x2 val)
    {
        return new XFix64Vector2x2(-val.c0, -val.c1);
    }


    /// <summary>Returns the result of a componentwise unary plus operation on a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 operator +(XFix64Vector2x2 val)
    {
        return new XFix64Vector2x2(+val.c0, +val.c1);
    }


    /// <summary>Returns the result of a componentwise equality operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator ==(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
    }

    /// <summary>Returns the result of a componentwise equality operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator ==(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
    }

    /// <summary>Returns the result of a componentwise equality operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator ==(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
    }


    /// <summary>Returns the result of a componentwise not equal operation on two XFix64Vector2x2 matrices.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator !=(XFix64Vector2x2 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
    }

    /// <summary>Returns the result of a componentwise not equal operation on a XFix64Vector2x2 matrix and a XFix64 value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator !=(XFix64Vector2x2 lhs, XFix64 rhs)
    {
        return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
    }

    /// <summary>Returns the result of a componentwise not equal operation on a XFix64 value and a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool2x2 operator !=(XFix64 lhs, XFix64Vector2x2 rhs)
    {
        return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
    }



    /// <summary>Returns the XFix64Vector2 element at a specified index.</summary>
    unsafe public ref XFix64Vector2 this[int index]
    {
        get
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if ((uint)index >= 2)
                throw new System.ArgumentException("index must be between[0...1]");
#endif
            fixed (XFix64Vector2x2* array = &this)
            {
                return ref ((XFix64Vector2*)array)[index];
            }
        }
    }

    /// <summary>Returns true if the XFix64Vector2x2 is equal to a given XFix64Vector2x2, false otherwise.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(XFix64Vector2x2 rhs)
    {
        return c0.Equals(rhs.c0) && c1.Equals(rhs.c1);
    }

    /// <summary>Returns true if the XFix64Vector2x2 is equal to a given XFix64Vector2x2, false otherwise.</summary>
    public override bool Equals(object o)
    {
        return Equals((XFix64Vector2x2)o);
    }


    /// <summary>Returns a hash code for the XFix64Vector2x2.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return (int)fpmath.hash(this);
    }


    /// <summary>Returns a string representation of the XFix64Vector2x2.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return string.Format("XFix64Vector2x2({0}, {1},  {2}, {3})", c0.x, c1.x, c0.y, c1.y);
    }

    /// <summary>Returns a string representation of the XFix64Vector2x2 using a specified format and culture-specific format information.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return string.Format("XFix64Vector2x2({0}, {1},  {2}, {3})", c0.x.ToString(format, formatProvider), c1.x.ToString(format, formatProvider), c0.y.ToString(format, formatProvider), c1.y.ToString(format, formatProvider));
    }

}

public static partial class fpmath
{
    /// <summary>Returns a XFix64Vector2x2 matrix constructed from two XFix64Vector2 vectors.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(XFix64Vector2 c0, XFix64Vector2 c1)
    {
        return new XFix64Vector2x2(c0, c1);
    }

    /// <summary>Returns a XFix64Vector2x2 matrix constructed from from 4 XFix64 values given in row-major order.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(XFix64 m00, XFix64 m01,
                              XFix64 m10, XFix64 m11)
    {
        return new XFix64Vector2x2(m00, m01,
                         m10, m11);
    }

    /// <summary>Returns a XFix64Vector2x2 matrix constructed from a single XFix64 value by assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(XFix64 v)
    {
        return new XFix64Vector2x2(v);
    }

    /// <summary>Returns a XFix64Vector2x2 matrix constructed from a single int value by converting it to XFix64 and assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(int v)
    {
        return new XFix64Vector2x2((XFix64)v);
    }

    /// <summary>Return a XFix64Vector2x2 matrix constructed from a int2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(int2x2 v)
    {
        return new XFix64Vector2x2(v);
    }

    /// <summary>Returns a XFix64Vector2x2 matrix constructed from a single uint value by converting it to XFix64 and assigning it to every component.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(uint v)
    {
        return new XFix64Vector2x2((XFix64)v);
    }

    /// <summary>Return a XFix64Vector2x2 matrix constructed from a uint2x2 matrix by componentwise conversion.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 XFix64Vector2x2(uint2x2 v)
    {
        return new XFix64Vector2x2(v);
    }

    /// <summary>Return the XFix64Vector2x2 transpose of a XFix64Vector2x2 matrix.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static XFix64Vector2x2 transpose(XFix64Vector2x2 v)
    {
        return XFix64Vector2x2(
            v.c0.x, v.c0.y,
            v.c1.x, v.c1.y);
    }

    /// <summary>Returns a uint hash code of a XFix64Vector2x2 vector.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint hash(XFix64Vector2x2 v)
    {
        return math.csum(MathXFix64.asuint(v.c0) * uint2(0x90A285BBu, 0x5D19E1D5u) +
                    MathXFix64.asuint(v.c1) * uint2(0xFAAF07DDu, 0x625C45BDu)) + 0xC9F27FCBu;
    }

    /// <summary>
    /// Returns a uint2 vector hash code of a XFix64Vector2x2 vector.
    /// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
    /// that are only reduced to a narrow uint hash at the very end instead of at every step.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint2 hashwide(XFix64Vector2x2 v)
    {
        return (MathXFix64.asuint(v.c0) * uint2(0x6D2523B1u, 0x6E2BF6A9u) +
                MathXFix64.asuint(v.c1) * uint2(0xCC74B3B7u, 0x83B58237u)) + 0x833E3E29u;
    }

}