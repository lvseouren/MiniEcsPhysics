using System;
using System.Collections.Generic;
using System.Text;
using XFixMath.NET;

/// <summary>
/// 内部统一描述角度、弧度
/// </summary>
public struct XAngle
{
    //游戏内弧度，unity弧度的100倍
    private XFix64 m_radian;

    public bool m_bSmooth;//是否要平滑表现
        
	//注意，目前这是unity弧度的100倍，不是unity角度。初始值是角度的可使用CreateXAngleFromUnityDegree
	//TODO 考虑到代码里面使用角度比弧度的频率要高，后面再修改成使用角度
    public XAngle(int radian)
    {
        m_radian = radian;
        m_bSmooth = true;
    }
    //转成游戏内自定的弧度，相当于unity标准弧度数值千分一
    public int radian
    {
        get { return m_radian; }
    }
    public XFix64 degree
    {
        get { return MathXFix64.Rad2Deg * m_radian;}
    }

    public static XAngle CreateXAngleFromXRadian(XFix64 radian)
    {
        return new XAngle(radian);
    }
    public static XAngle CreateXAngleFromXDegree(XFix64 degree)
    {
        int radian = degree * MathXFix64.Deg2Rad;
        return new XAngle(radian);
    }

    public static XAngle zero
    {
        get { return new XAngle(0); }
    }

    //
    // Methods
    //
    public override int GetHashCode()
    {
        return this.m_radian.GetHashCode();
    }
    public bool Equals(XAngle other)
    {
        return this.m_radian == other.m_radian;
    }
    public override bool Equals(object other)
    {
        if (!(other is XAngle))
        {
            return false;
        }
        return this.m_radian == ((XAngle)other).m_radian;
    }
    public override string ToString()
    {
        return string.Format("XAngle({0})", m_radian);
    }

    public string ToString(string format)
    {
        return string.Format("XAngle({0},{1})", m_radian, m_bSmooth);
    }

    //
    // Operators
    //
    public static XAngle operator +(XAngle a, XAngle b)
    {
        return new XAngle(a.m_radian + b.m_radian);
    }
    public static XAngle operator -(XAngle a, XAngle b)
    {
        return new XAngle(a.m_radian - b.m_radian);
    }

    public static XAngle operator -(XAngle a)
    {
        return new XAngle(-a.m_radian);
    }

    public static XAngle operator /(XAngle a, int d)
    {
        return new XAngle(a.m_radian * d);
    }
    public static XAngle operator /(XAngle a, XAngle b)
    {
        int radian = a.m_radian / b.m_radian;
        return new XAngle(radian);
    }

    public static bool operator ==(XAngle lhs, XAngle rhs)
    {
        return lhs.m_radian== rhs.m_radian;
    }

    public static bool operator !=(XAngle lhs, XAngle rhs)
    {
        return lhs.m_radian != rhs.m_radian;
    }

    public static XAngle operator *(XAngle a, int d)
    {
        return new XAngle(a.m_radian * d );
    }
    public static XAngle operator *(int d, XAngle a)
    {
        return new XAngle(a.m_radian * d);
    }

    public static XAngle operator *(XAngle a, XFix64 d)
    {
        return new XAngle(a.m_radian * d);
    }
    public static XAngle operator *(XFix64 d, XAngle a)
    {
        return new XAngle(a.m_radian * d);
    }
}