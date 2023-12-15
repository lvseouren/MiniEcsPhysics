using MiniEcs.Core;
using MiniEcs.Core.Systems;
using XFixMath.NET;

namespace Physics
{
    [EcsUpdateInGroup(typeof(PhysicsSystemGroup))]
    [EcsUpdateAfter(typeof(RaytracingSystem))]
    public class BroadphaseClearPairSystem : IEcsSystem
    {
        public void Update(XFix64 deltaTime, EcsWorld world)
        {
            BroadphaseSAPComponent bpChunks = world.GetOrCreateSingleton<BroadphaseSAPComponent>();
            
            bpChunks.Pairs.Clear();
        }
    }
}