using MiniEcs.Core;
using MiniEcs.Core.Systems;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using XFixMath.NET;

namespace Physics
{
    [EcsUpdateInGroup(typeof(PhysicsSystemGroup))]
    [EcsUpdateAfter(typeof(BroadphaseInitSystem))]
    [EcsUpdateBefore(typeof(BroadphaseUpdateSystem))]
    public class IntegrateVelocitySystem : IEcsSystem
    {
        private readonly EcsFilter _filter;
        public IntegrateVelocitySystem()
        {
            _filter = new EcsFilter().AllOf<TransformComponent, RigBodyComponent>().NoneOf<RigBodyStaticComponent>();
        }

        public void Update(XFix64 deltaTime, EcsWorld world)
        {
            world.Filter(_filter).ForEach((IEcsEntity entity, TransformComponent transform, RigBodyComponent rigBody) =>
            {
                transform.Position += rigBody.Velocity * deltaTime;
            });
        }
    }
}