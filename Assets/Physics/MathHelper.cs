using System.Runtime.CompilerServices;
using Unity.Mathematics;
using XFixMath.NET;

namespace Physics
{
    public class MathHelper
    {
        public static readonly XFix64 EPSILON = 1.1920928955078125e-7f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equal(XFix64 a, XFix64 b)
        {
            return a.Equals(b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XFix64Vector2 Mul(float2x2 lhs, XFix64Vector2 rhs)
        {
            return new XFix64Vector2(lhs.c0.x * rhs.x + lhs.c1.x * rhs.y, lhs.c0.y * rhs.x + lhs.c1.y * rhs.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2x2 Transpose(float2x2 v)
        {
            return new float2x2(v.c0.x, v.c0.y, v.c1.x, v.c1.y);
        }
    }
}