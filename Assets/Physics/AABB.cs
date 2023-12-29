using Unity.Mathematics;
using XFixMath.NET;

namespace Physics
{
    public struct AABB
    {
        public XFix64Vector2 Min;
        public XFix64Vector2 Max;
        
        public AABB(XFix64Vector2 size, XFix64Vector2 position, XFix64 rotation)
        {
            XFix64.Sin(rotation, out var sin);
            var cos = XFix64.Cos(rotation);

            XFix64.Abs(size.x * cos + size.y * sin, out var abs1);
            XFix64.Abs(size.x * cos - size.y * sin, out var abs2);
            XFix64.Max(abs1, abs2, out var ex);

            XFix64.Abs(size.x * sin - size.y * cos, out var abs3);
            XFix64.Abs(size.x * sin + size.y * cos, out var abs4);
            XFix64.Max(abs3, abs4, out var ey);

            Min = new XFix64Vector2(position.x - ex, position.y - ey);
            Max = new XFix64Vector2(position.x + ex, position.y + ey);
        }

        public bool Overlap(AABB aabb)
        {
            return !(Max.x < aabb.Min.x) && !(Min.x > aabb.Max.x) && !(Max.y < aabb.Min.y) && !(Min.y > aabb.Max.y);
        }

        public override string ToString()
        {
            return $"({Min}, {Max})";
        }
    }
}