using System.Collections.Generic;
using MiniEcs.Core;
using Unity.Mathematics;
using XFixMath.NET;

namespace Physics
{
    public class TransformComponent : IEcsComponent
    {
        public float2 Position = float2.zero;
        public XFix64 Rotation = 0;
    }

    public class RigBodyComponent : IEcsComponent
    {
        public float2 Velocity = float2.zero;
        private XFix64 _mass = 1.0f;

        public XFix64 Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                InvMass = !MathHelper.Equal(Mass, 0.0f) ? 1.0f / Mass : 0.0f;
            }
        }

        public XFix64 InvMass { get; private set; } = 0.1f;
    }

    public enum ColliderType : byte
    {
        Circle = 0,
        Rect = 1
    }

    public class ColliderComponent : IEcsComponent
    {
        public int Layer;
        public ColliderType ColliderType;
        public float2x4 Vertices { get; private set; }
        public float2x4 Normals { get; private set; }

        private float2 _size;
        public float2 Size
        {
            get => _size;
            set
            {
                _size = value;
                XFix64 w = _size.x;
                XFix64 h = _size.y;
                Vertices = new float2x4(-w, w, w, -w, -h, -h, h, h);
                Normals = new float2x4(0.0f, 1.0f, 0.0f, -1.0f, -1.0f, 0.0f, 1.0f, 0.0f);
            }
        }
    } 

    public class RigBodyStaticComponent : IEcsComponent
    {
    }

    public class BroadphaseRefComponent : IEcsComponent
    {
        public List<SAPChunk> Chunks;
        public int ChunksHash;
        public AABB AABB;
    }

    public class BroadphaseSAPComponent : IEcsComponent
    {
        public readonly Dictionary<int, SAPChunk> Chunks = new Dictionary<int, SAPChunk>();
        public readonly HashSet<BroadphasePair> Pairs = new HashSet<BroadphasePair>();
    }

    public class RayComponent : IEcsComponent
    {
        public int Layer;
        public float2 Source;
        public XFix64 Rotation;
        public XFix64 Length;

        public float2 Target
        {
            get
            {
                float2 dir = new float2(-math.sin(Rotation), math.cos(Rotation));
                return Source + Length * dir;
            }
        }

        public bool Hit;
        public float2 HitPoint;
    }

    
}