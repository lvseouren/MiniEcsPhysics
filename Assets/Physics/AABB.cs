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
            var sin = XFix64.Sin(rotation);
            var cos = XFix64.Cos(rotation);            

            XFix64 ex = XFix64.Max(XFix64.Abs(size.x * cos + size.y * sin), XFix64.Abs(size.x * cos - size.y * sin));
            XFix64 ey = XFix64.Max(XFix64.Abs(size.x * sin - size.y * cos), XFix64.Abs(size.x * sin + size.y * cos));

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