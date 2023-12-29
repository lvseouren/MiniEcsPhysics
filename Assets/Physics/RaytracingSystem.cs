using MiniEcs.Core;
using MiniEcs.Core.Systems;
using Unity.Mathematics;
using XFixMath.NET;

namespace Physics
{
    [EcsUpdateInGroup(typeof(PhysicsSystemGroup))]
    [EcsUpdateAfter(typeof(ResolveCollisionsSystem))]
    public class RaytracingSystem : IEcsSystem
    {
        private readonly CollisionMatrix _collisionMatrix;
        private readonly EcsFilter _rayFilter;

        public RaytracingSystem(CollisionMatrix collisionMatrix)
        {
            _collisionMatrix = collisionMatrix;
            _rayFilter = new EcsFilter().AllOf<RayComponent, TransformComponent>();
        }

        private int[] _chunksBuffer = new int[100];
        private XFix64Vector2[] _pointsBuffer = new XFix64Vector2[100];

        public unsafe void Update(XFix64 deltaTime, EcsWorld world)
        {
            BroadphaseSAPComponent bpChunks = world.GetOrCreateSingleton<BroadphaseSAPComponent>();

            world.Filter(_rayFilter).ForEach((IEcsEntity entity, TransformComponent tr, RayComponent ray) =>
            {
                ray.Hit = false;
                ray.Source = tr.Position;
                ray.Rotation = tr.Rotation;

                XFix64 minDist = XFix64.MaxValue;
                RayTrace(ray, ref _chunksBuffer, ref _pointsBuffer, out int length);

                for (int i = 0; i < length - 1; i++)
                {
                    SAPChunk chunk = BroadphaseHelper.GetOrCreateChunk(_chunksBuffer[i], bpChunks);
                    XFix64Vector2 p1 = _pointsBuffer[i];
                    XFix64Vector2 p2 = _pointsBuffer[i + 1];

                    AABB sAABB = new AABB
                    {
                        Min = new XFix64Vector2(XFix64.Min(p1.x, p2.x), XFix64.Min(p1.y, p2.y)),
                        Max = new XFix64Vector2(XFix64.Max(p1.x, p2.x), XFix64.Max(p1.y, p2.y))
                    };

                    for (int j = 0; j < chunk.Length; j++)
                    {
                        BroadphaseAABB item = chunk.Items[j];
                        if (!item.AABB->Overlap(sAABB))
                            continue;

                        if (!_collisionMatrix.Check(ray.Layer, item.Layer))
                            continue;

                        IEcsEntity targetEntity = item.Entity;
                        if (entity == targetEntity)
                            continue;

                        tr = targetEntity.GetComponent<TransformComponent>();
                        ColliderComponent col = targetEntity.GetComponent<ColliderComponent>();

                        XFix64Vector2 hitPoint = XFix64Vector2.zero;
                        if (col.ColliderType == ColliderType.Circle && !OnCircleIntersection(ray, col, tr, out hitPoint) ||
                            col.ColliderType == ColliderType.Rect && !OnRectIntersection(ray, col, tr, out hitPoint))
                            continue;

                        XFix64Vector2.Dot(p1, hitPoint, out var dotResult);
                        XFix64 dist = dotResult;
                        if (!(dist < minDist))
                            continue;

                        minDist = dist;
                        ray.HitPoint = hitPoint;
                    }

                    if (!(minDist < XFix64.MaxValue))
                        continue;

                    ray.Hit = true;
                    break;
                }
            });
        }

        private static void RayTrace(RayComponent ray, ref int[] chunks, ref XFix64Vector2[] points, out int length)
        {
            XFix64 cellSize = BroadphaseHelper.ChunkSize;
            XFix64 offset = ushort.MaxValue * cellSize;

            XFix64Vector2 source = ray.Source + offset;
            XFix64Vector2 target = ray.Target + offset;

            XFix64 x0 = source.x / cellSize;
            XFix64 y0 = source.y / cellSize;
            XFix64 x1 = target.x / cellSize;
            XFix64 y1 = target.y / cellSize;
            XFix64 dx = XFix64.Abs(x1 - x0);
            XFix64 dy = XFix64.Abs(y1 - y0);

            int x = (int) x0;
            int y = (int) y0;

            XFix64 dtDx = dx > 0 ? 1 / dx : 0;
            XFix64 dtDy = dy > 0 ? 1 / dy : 0;
            XFix64 t = 0;

            int n = 1;
            int xInc, yInc;
            XFix64 tnv, tnh;

            if (dx == XFix64.Zero)
            {
                xInc = 0;
                tnh = dtDx;
            }
            else if (x1 > x0)
            {
                xInc = 1;
                n += XFix64.Floor(x1) - x;
                tnh = (XFix64.Floor(x0) + 1 - x0) * dtDx;
            }
            else
            {
                xInc = -1;
                n += x - XFix64.Floor(x1);
                tnh = (x0 - XFix64.Floor(x0)) * dtDx;
            }

            if (dy == XFix64.Zero)
            {
                yInc = 0;
                tnv = dtDy;
            }
            else if (y1 > y0)
            {
                yInc = 1;
                n += XFix64.Floor(y1) - y;
                tnv = (XFix64.Floor(y0) + 1 - y0) * dtDy;
            }
            else
            {
                yInc = -1;
                n += y - XFix64.Floor(y1);
                tnv = (y0 - XFix64.Floor(y0)) * dtDy;
            }

            length = n + 1;
            for (int i = 0; n > 0; n--, i++)
            {
                XFix64 xPos = t * (x1 - x0) * cellSize;
                XFix64 yPos = t * (y1 - y0) * cellSize;
                XFix64Vector2 pos = new XFix64Vector2(xPos, yPos);
                points[i] = source + pos - offset;

                short xChunk = (short) (x - ushort.MaxValue);
                short yChunk = (short) (y - ushort.MaxValue);
                chunks[i] = (xChunk << 16) | (ushort) yChunk;

                if (tnv < tnh)
                {
                    y += yInc;
                    t = tnv;
                    tnv += dtDy;
                }
                else
                {
                    x += xInc;
                    t = tnh;
                    tnh += dtDx;
                }
            }

            points[length - 1] = target - offset;
        }

        private static bool OnCircleIntersection(RayComponent ray, ColliderComponent col, TransformComponent tr,
            out XFix64Vector2 hitPoint)
        {
            hitPoint = XFix64Vector2.zero;
            XFix64Vector2 source = ray.Source;
            XFix64Vector2 target = ray.Target;
            XFix64Vector2 pos = tr.Position;
            XFix64 r = col.Size.x;

            XFix64 t;
            XFix64 dx = target.x - source.x;
            XFix64 dy = target.y - source.y;

            XFix64 a = dx * dx + dy * dy;
            XFix64 spDx = source.x - pos.x;
            XFix64 spDy = source.y - pos.y;
            XFix64 b = 2 * (dx * spDx + dy * spDy);
            XFix64 c = spDx * spDx + spDy * spDy - r * r;

            XFix64 det = b * b - 4 * a * c;
            if (a == XFix64.Zero || det < 0)
                return false;

            if (MathHelper.Equal(det, 0))
            {
                t = -b / (2 * a);
                hitPoint = new XFix64Vector2(source.x + t * dx, source.y + t * dy);
                return true;
            }

            XFix64 sqrtDet = XFix64.Sqrt(det);

            t = (-b + sqrtDet) / (2 * a);
            XFix64Vector2 p1 = new XFix64Vector2(source.x + t * dx, source.y + t * dy);

            t = (-b - sqrtDet) / (2 * a);
            XFix64Vector2 p2 = new XFix64Vector2(source.x + t * dx, source.y + t * dy);

            XFix64Vector2.Dot(ray.Source, p1, out var dotResult1);
            XFix64Vector2.Dot(ray.Source, p2, out var dotResult2);
            hitPoint = dotResult1 < dotResult2 ? p1 : p2;
            return true;
        }

        private static bool OnRectIntersection(RayComponent ray, ColliderComponent col, TransformComponent tr,
            out XFix64Vector2 hitPoint)
        {
            hitPoint = XFix64Vector2.zero;
            XFix64 minDist = XFix64.MaxValue;

            float2x2 rotate = float2x2.Rotate(tr.Rotation);
            XFix64Vector2x4 vertices = XFix64Vector2x4.zero;
            for (int i = 0; i < 4; i++)
                vertices[i] = MathHelper.Mul(rotate, col.Vertices[i]) + tr.Position;

            for (int i = 0; i < 4; i++)
            {
                int j = i + 1;
                if (j == 4)
                    j = 0;

                XFix64Vector2 p1 = vertices[i];
                XFix64Vector2 p2 = vertices[j];

                XFix64Vector2 b = ray.Target - ray.Source;
                XFix64Vector2 d = p2 - p1;

                XFix64 cross = b.x * d.y - b.y * d.x;
                if (MathHelper.Equal(cross, 0))
                    continue;

                XFix64Vector2 c = p1 - ray.Source;
                XFix64 t = (c.x * d.y - c.y * d.x) / cross;
                if (t < 0 || t > 1)
                    continue;

                XFix64 u = (c.x * b.y - c.y * b.x) / cross;
                if (u < 0 || u > 1)
                    continue;

                XFix64Vector2 p = ray.Source + t * b;

                XFix64Vector2.Dot(ray.Source, p, out var dotResult);
                XFix64 dist = dotResult;
                if (!(dist < minDist))
                    continue;

                minDist = dist;
                hitPoint = p;
            }

            return minDist < XFix64.MaxValue;
        }
    }
}