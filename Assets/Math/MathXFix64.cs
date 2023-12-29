using System;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using XFixMath.NET;
using Random = System.Random;

public partial class MathXFix64
{
    public static readonly XFix64 Rad2Deg = 360 / XFix64.PiTimes2;
    public static readonly XFix64 Deg2Rad = XFix64.PiTimes2 / 360;
    public static Random random = new Random(1);
    #region uint
    public static uint asuint(XFix64 x)
    {
        return math.asuint((uint)x);
    }

    public static uint2 asuint(XFix64Vector2 x)
    {
        return math.uint2(asuint(x.x), asuint(x.y));
    }

    public static uint3 asuint(XFix64Vector3 x)
    {
        return math.uint3(asuint(x.x), asuint(x.y), asuint(x.z));
    }

    public static uint4 asuint(XFix64Vector4 x)
    {
        return math.uint4(asuint(x.x), asuint(x.y), asuint(x.z), asuint(x.w));
    }
    #endregion   
    public static XFix64Vector2 RandomXFix64Vec2()
    {
        var num = random.NextXFix64();
        return new XFix64Vector2(num);
    }

    public static XFix64Vector3 RandomXFix64Vec3()
    {
        var num = random.NextXFix64();
        return new XFix64Vector3(num);
    }
}