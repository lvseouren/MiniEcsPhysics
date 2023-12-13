using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 内部统一描述角度、弧度
/// </summary>
public struct XAngle
{
    //游戏内弧度，unity弧度的100倍
    private int m_radian;

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
    public int degree
    {
        //get { return MathX.FixToInt(Math.Round((double)(MathX.Rad2Deg * m_radian))); }
        //get { return MathX.FixToInt(MathX.Rad2Deg * m_radian); }
        get { return MathX.FixToInt(MathX.Rad2Deg * m_radian / XDefine.UNIT_CR_PER_RADIAN);}
    }
    public float unityRadian
    {
        get { return ToUnityRadian(); }
    }
    public float unityDegree
    {
        get { return ToUnityDegree(); }
    }

    public static XAngle CreateXAngleFromUnityRadian(float unityRadian)
    {
        int radian = MathX.FixToInt(Math.Round(unityRadian * XDefine.UNIT_CR_PER_RADIAN));
        return new XAngle(radian);
    }
    public static XAngle CreateXAngleFromUnityDegree(float unityDegree)
    {
        int radian = MathX.FixToInt(Math.Round(unityDegree * MathX.Deg2Rad * XDefine.UNIT_CR_PER_RADIAN));
        return new XAngle(radian);
    }
    public static XAngle CreateXAngleFromXRadian(int radian)
    {
        return new XAngle(radian);
    }
    public static XAngle CreateXAngleFromXDegree(int degree)
    {
        int radian = MathX.FixToInt(Math.Round(degree * MathX.Deg2Rad));
        return new XAngle(radian);
    }

    public static XAngle zero
    {
        get { return new XAngle(0); }
    }

    /// <summary>
    /// 转成unity的标准角度
    /// </summary>
    /// <returns></returns>
    public float ToUnityDegree()
    {
		return MathX.Rad2Deg * m_radian / XDefine.UNIT_CR_PER_RADIAN;
    }
    /// <summary>
    /// 转成unity的标准角度，取整数
    /// </summary>
    /// <returns></returns>
    public int ToUnityDegreeInt()
    {
        return MathX.FixToInt(MathX.Rad2Deg * m_radian / XDefine.UNIT_CR_PER_RADIAN);
    }
    /// <summary>
    /// 转成unity的弧度
    /// </summary>
    /// <returns></returns>
    public float ToUnityRadian()
    {
        return (float)(m_radian / (float)XDefine.UNIT_CR_PER_RADIAN);
    }

    /// <summary>
    /// 转成游戏内自定的弧度，相当于unity标准弧度数值百分一
    /// </summary>
    /// <returns></returns>
    public int ToXRadian()
    {
        return m_radian;
    }
    /// <summary>
    /// 转成游戏内自定的角度，相当于unity标准角数百分一
    /// </summary>
    /// <returns></returns>
    public int ToXDegree()
    {
        return MathX.FixToInt(Math.Round((double)(MathX.Rad2Deg * m_radian)));
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
        return string.Format("XAngle({0})", ToUnityDegree());
    }

    public string ToString(string format)
    {
        return string.Format("XAngle({0},{1})", new object[]
            {
                this.m_radian,
                this.m_bSmooth
            });
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
        int radian = MathX.FixToInt(Math.Round((double)(a.m_radian / b.m_radian)));
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

    public static XAngle operator *(XAngle a, float d)
    {
        int radian = MathX.FixToInt(Math.Round((double)(a.m_radian * d)));
        return new XAngle(radian);
    }
    public static XAngle operator *(float d, XAngle a)
    {
        int radian = MathX.FixToInt(Math.Round((double)(a.m_radian * d)));
        return new XAngle(radian);
    }


}