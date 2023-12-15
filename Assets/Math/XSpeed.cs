using System;
using System.Collections.Generic;
using System.Text;
using XFixMath.NET;

/// <summary>
/// 通用速度结构
/// </summary>
public struct XSpeed
{
    public enum XSpeedType
    {
        CM_PER_SECOND,      //距离，厘米/秒
        HP_REC_PER_SECOND,  //回血，点/秒
        FREQUENCY_PER_SECOND,  //次数，次/秒
        ANGLE_PER_SECOND,      //角度, 角/秒
    }
    private XSpeedType m_speedType;
    private XFix64 m_value;    //分子
    //private int m_denominator;  //分母

    public XSpeed(XSpeedType speedType, XFix64 value )
    {
        m_speedType = speedType;
        m_value = value;
    }
    public XSpeedType speedType
    {
        get { return m_speedType; }
    }
    public int value
    {
        get { return m_value; }
    }

    public float ToUnitySpeed()
    {
        return (float)m_value / XDefine.UNIT_MS_PER_SECOND;
    }

    public static XSpeed operator +(XSpeed a, XSpeed b)
    {
        if (a.m_speedType != b.speedType)
        {
            throw new Exception("XSpeed operator + error,speedType is different");
        }
        return new XSpeed(a.speedType, a.m_value + b.m_value);
    }
    public static XSpeed operator -(XSpeed a, XSpeed b)
    {
        if (a.m_speedType != b.speedType)
        {
            throw new Exception("XSpeed operator + error,speedType is different");
        }
        return new XSpeed(a.speedType, a.m_value - b.m_value);
    }

    public static XSpeed operator -(XSpeed a)
    {
        return new XSpeed(a.speedType,-a.m_value);
    }

    public static XSpeed operator /(XSpeed a, int d)
    {
        return new XSpeed(a.speedType, a.m_value / d);
    }
    //TODO 速度相除==？
    //public static XSpeed operator /(XSpeed a, XSpeed b)
    //{
    //    if (a.m_speedType != b.speedType)
    //    {
    //        throw new Exception("XSpeed operator + error,speedType is different");
    //    }
    //    return new XSpeed(a.speedType, a.m_numerator / b.m_numerator);
    //}

    public static bool operator ==(XSpeed lhs, XSpeed rhs)
    {
        return lhs.speedType == rhs.speedType && lhs.m_value==rhs.m_value;
    }

    public static bool operator !=(XSpeed lhs, XSpeed rhs)
    {
        return lhs.speedType != rhs.speedType || lhs.m_value != rhs.m_value;
    }

    public static XSpeed operator *(XSpeed a, int d)
    {
        return new XSpeed(a.speedType, a.m_value * d);
    }
    public static XSpeed operator *(int d, XSpeed a)
    {
        return new XSpeed(a.speedType, a.m_value * d);
    }

    public static XSpeed operator *(XSpeed a, XFix64 d)
    {
        return new XSpeed(a.speedType, a.m_value * d);
    }
    public static XSpeed operator *(XFix64 d, XSpeed a)
    {
        return new XSpeed(a.speedType, a.m_value * d);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is XSpeed))
        {
            return false;
        }
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}