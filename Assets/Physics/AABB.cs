using Unity.Mathematics;

namespace Physics
{
    public struct AABB
    {
        public float2 Min;
        public float2 Max;
        
        public AABB(float2 size, float2 position, XFix64 rotation)
        {
            math.sincos(rotation, out XFix64 sin, out XFix64 cos);

            XFix64 ex = math.max(math.abs(size.x * cos + size.y * sin), math.abs(size.x * cos - size.y * sin));
            XFix64 ey = math.max(math.abs(size.x * sin - size.y * cos), math.abs(size.x * sin + size.y * cos));

            Min = new float2(position.x - ex, position.y - ey);
            Max = new float2(position.x + ex, position.y + ey);
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