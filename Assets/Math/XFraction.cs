using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// 分数
/// </summary>
public class XFraction//: IComparable
{
    #region 变量
    private long m_numerator;//分子
    private int m_denominator;//分母
    #endregion

    #region 属性
    /// <summary>
    /// 分子
    /// </summary>
    public long numerator
    {
        get {
            return m_numerator;
        }
        //set
        //{
        //    numerator = value;
        //}
    }
    /// <summary>
    /// 分母
    /// </summary>
    public int denominator
    {
        get {
            return m_denominator;
        }
        //set
        //{
        //    denominator = value;
        //}
    }
    /// <summary>
    /// 浮点值
    /// </summary>
    public float valueFloat
    {
        get {
            float result = (float)m_numerator / (float)m_denominator;
            return result;
        }
    }

    public int valueInt
    {
        get {
            return (int)(m_numerator / m_denominator);
        }
    }

    #endregion

    #region 构造函数

    public XFraction()
    {
        this.m_numerator = 0;
        this.m_denominator = 1;
        this.CreateIrreducibleFration();
        this.minus();
    }

    public XFraction(int numerator, int denominator)
    {
        if (denominator == 0) {
            //XLog.LogError("Denominator can't be zero!!!" + "numerator:" + numerator + "  denominator:" + denominator);
            denominator = 1;
        }
        this.m_numerator = numerator;
        this.m_denominator = denominator;
        this.CreateIrreducibleFration();
        this.minus();
    }

    public XFraction(float x)
    {
        //string strTemp = x.ToString();
        //strTemp = strTemp.Substring(strTemp.IndexOf(".") + 1);
        //int number = strTemp.Length;
        ////
        //this.m_denominator = (int)Math.Pow(10, number);
        //this.m_numerator = (int)(x * Math.Pow(10, number));

        m_denominator = 10000;
        m_numerator = MathX.FloorToInt(x * m_denominator);
        this.CreateIrreducibleFration();
        this.minus();
    }

    public XFraction(int x)
    {
        this.m_numerator = x;
        this.m_denominator = 1;
        this.CreateIrreducibleFration();
        this.minus();
    }
    #endregion

    #region 


    public override string ToString()
    {
        return string.Format("{0}/{1}", m_numerator, m_denominator);
        //return base.ToString();
    }

    /// <summary>
    /// 固定不可约分数的方法
    /// </summary>
    private void CreateIrreducibleFration()
    {
        //int d = GCD(m_numerator, m_denominator);
        //m_numerator = m_numerator / d;
        //m_denominator = m_denominator / d;
    }

    /// <summary>
    /// 改变负号所在的位置  即如果是负数 负号一定在分子上
    /// </summary>
    private void minus()
    {
        if ((m_denominator < 0 && m_numerator < 0) || (m_denominator < 0)) {
            m_numerator *= -1;
            m_denominator *= -1;
        }
    }
    #endregion

    //#region 静的メソッド（演算子関係）
    ///// <summary>
    ///// 获取分数的绝对值
    ///// </summary>
    ///// <param name="x"></param>
    ///// <returns></returns>
    //public static XFix64 ABS(XFix64 x)
    //{
    //    return new XFix64(Math.Abs(x.numerator), Math.Abs(x.denominator));
    //}
    //#region 四个基本算术运算
    ///// <summary>
    ///// 加算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator +(XFix64 x, XFix64 y)
    //{
    //    //両分母の最小公倍数をとる
    //    int lc = LCM(x.m_denominator, y.m_denominator);
    //    //分母が異なるなら最小公倍数/分母を分子にかける
    //    if (x.m_denominator != y.m_denominator) {
    //        x.m_numerator *= (lc / x.m_denominator);
    //        y.m_numerator *= (lc / y.m_denominator);
    //    }
    //    XFix64 result = new XFix64(x.m_numerator + y.m_numerator, lc);
    //    return result;
    //}
    ///// <summary>
    ///// 加算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator +(XFix64 x, int y)
    //{
    //    return x + new XFix64(y);
    //}
    ///// <summary>
    ///// 減算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator -(XFix64 x, XFix64 y)
    //{
    //    //両分母の最小公倍数をとる
    //    int lc = LCM(x.m_denominator, y.m_denominator);
    //    //分母が異なるなら最小公倍数/分母を分子にかける
    //    if (x.m_denominator != y.m_denominator) {
    //        x.m_numerator *= (lc / x.m_denominator);
    //        y.m_numerator *= (lc / y.m_denominator);
    //    }
    //    XFix64 result = new XFix64(x.m_numerator - y.m_numerator, lc);
    //    return result;
    //}
    ///// <summary>
    ///// 減算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator -(XFix64 x, int y)
    //{
    //    return x - new XFix64(y);
    //}
    ///// <summary>
    ///// 乗算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator *(XFix64 x, XFix64 y)
    //{
    //    return new XFix64(x.m_numerator * y.m_numerator, x.m_denominator * y.m_denominator);
    //}
    ///// <summary>
    ///// 乗算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator *(XFix64 x, int y)
    //{
    //    return new XFix64(x.m_numerator * y, x.m_denominator);
    //}
    ///// <summary>
    ///// 除算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator /(XFix64 x, XFix64 y)
    //{
    //    if (y.m_numerator == 0) {
    //        //分母が0なら例外を吐く
    //        throw new ArgumentException("割る数に0が指定されました");
    //    }
    //    return new XFix64(x.m_numerator * y.m_denominator, x.m_denominator * y.m_numerator);
    //}
    ///// <summary>
    ///// 除算
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static XFix64 operator /(XFix64 x, int y)
    //{
    //    if (y == 0) {
    //        //分母が0なら例外を吐く
    //        throw new ArgumentException("分母に0が指定されました");
    //    }
    //    return new XFix64(x.m_numerator, x.m_denominator * y);
    //}
    //#endregion

    //#region 比较操作
    ///// <summary>
    ///// objと自分自身が等価のときはtrueを返す
    ///// </summary>
    ///// <param name="obj"></param>
    ///// <returns></returns>
    //public override bool Equals(object obj)
    //{
    //    //objがnullか、型が違うときは、等価でない
    //    if ((object)obj == null || this.GetType() != obj.GetType()) {
    //        return false;
    //    }
    //    XFix64 c = (XFix64)obj;
    //    return (this.m_numerator == c.m_numerator) && (this.m_denominator == c.m_denominator);
    //    //return base.Equals(obj);
    //}
    ///// <summary>
    ///// Equalがtrueを返すときに同じ値を返す
    ///// </summary>
    ///// <returns></returns>
    //public override int GetHashCode()
    //{
    //    //XOR
    //    return this.m_numerator ^ this.m_denominator.GetHashCode();
    //    //return base.GetHashCode();
    //}
    ///// <summary>
    ///// 自分自身がotherより小さいときはマイナスの数、大きいときはプラスの数、
    ///// 同じときは0を返す
    ///// </summary>
    ///// <param name="obj"></param>
    ///// <returns></returns>
    //public int CompareTo(object obj)
    //{
    //    //nullより大きい
    //    if (obj == null) {
    //        return 1;
    //    }
    //    //違う型とは比較できない
    //    if (this.GetType() != obj.GetType()) {
    //        throw new ArgumentException("別の型とは比較できません。", "obj");
    //    }
    //    //単純な減算の結果を返す
    //    return this.valueFloat.CompareTo(((XFix64)obj).valueFloat);
    //}

    ///// <summary>
    ///// 比較演算子(==)
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator ==(XFix64 x, XFix64 y)
    //{
    //    //nullの確認
    //    if ((object)x == null) {
    //        return ((object)y == null);
    //    }
    //    if ((object)y == null) {
    //        return false;
    //    }
    //    //Equalメソッドを呼び出す
    //    return x.Equals(y);
    //}
    ///// <summary>
    ///// 比較演算子(!=)
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator !=(XFix64 x, XFix64 y)
    //{
    //    return !(x == y);
    //}
    ///// <summary>
    ///// 比較（<）
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator <(XFix64 x, XFix64 y)
    //{
    //    //nullの確認
    //    if ((object)x == null || (object)y == null) {
    //        throw new ArgumentException();
    //    }
    //    //CompareToメソッド
    //    //XFix64 temp = x - y;

    //    return ((x - y).m_numerator < 0);
    //}
    ///// <summary>
    ///// 比較（>）
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator >(XFix64 x, XFix64 y)
    //{
    //    return (y < x);
    //}
    ///// <summary>
    ///// 比較（<=）
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator <=(XFix64 x, XFix64 y)
    //{
    //    //nullの確認
    //    if ((object)x == null || (object)y == null) {
    //        throw new ArgumentException();
    //    }
    //    //CompareToメソッド
    //    return ((x - y).m_numerator <= 0);
    //}
    ///// <summary>
    ///// 比較（>=）
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="y"></param>
    ///// <returns></returns>
    //public static bool operator >=(XFraction x, XFraction y)
    //{
    //    return (y <= x);
    //}
    //#endregion
    //#endregion

    //public static implicit operator int(XFraction value)
    //{
    //    return value;
    //}

    public static implicit operator XFraction(int value)
    {
        return new XFraction(value);
    }

    //public static implicit operator float(XFraction value)
    //{
    //    return value.valueFloat;
    //}

    public static implicit operator XFraction(float value)
    {
        return new XFraction(value);
    }

    ///// <summary>
    ///// 获取最大公约数
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns></returns>
    //static public int GCD(int a, int b)
    //{
    //    a = Math.Abs(a);
    //    b = Math.Abs(b);

    //    while (a != 0 && b != 0) {
    //        if (a > b)
    //            a %= b;
    //        else
    //            b %= a;
    //    }

    //    return a == 0 ? b : a;

    //    //a = Math.Abs(a);
    //    //b = Math.Abs(b);
    //    //if (a < b) {
    //    //    return GCD(b, a);//使b为最小值
    //    //}
    //    //while (b != 0) {
    //    //    int r = a % b;
    //    //    a = b;
    //    //    b = r;
    //    //}
    //    //return a;
    //}
    ///// <summary>
    ///// 最小公倍数を求めう関数
    ///// </summary>
    ///// <param name="a"></param>
    ///// <param name="b"></param>
    ///// <returns></returns>
    //public static int LCM(int a, int b)
    //{
    //    return a * b / GCD(a, b);
    //}

}


