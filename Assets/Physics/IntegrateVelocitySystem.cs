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
        List<TransformComponent> transformComponents = new List<TransformComponent>();
        List<XFix64Vector2> velocities = new List<XFix64Vector2>();
        public IntegrateVelocitySystem()
        {
            _filter = new EcsFilter().AllOf<TransformComponent, RigBodyComponent>().NoneOf<RigBodyStaticComponent>();
        }

        public void Update(XFix64 deltaTime, EcsWorld world)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //transformComponents.Clear();
            //velocities.Clear();
            world.Filter(_filter).ForEach((IEcsEntity entity, TransformComponent transform, RigBodyComponent rigBody) =>
            {
                //transformComponents.Add(transform);
                //velocities.Add(rigBody.Velocity);
                transform.Position += rigBody.Velocity * deltaTime;
            });
            //NativeArray<XFix64Vector2> pos = new NativeArray<XFix64Vector2>(transformComponents.Count, Allocator.TempJob);
            //NativeArray<XFix64Vector2> vel = new NativeArray<XFix64Vector2>(transformComponents.Count, Allocator.TempJob);
            //for (int i = 0; i < transformComponents.Count; i++)
            //{
            //    pos[i] = transformComponents[i].Position;
            //    vel[i] = velocities[i];
            //}
            //var job = new TestJob2()
            //{
            //    positions = pos,
            //    velocities = vel,
            //    deltaTime = deltaTime,
            //};
            //var jobHandler = job.Schedule(transformComponents.Count,64);
            //jobHandler.Complete();
            //for(int i = 0; i < transformComponents.Count; ++i)
            //{
            //    transformComponents[i].Position = pos[i];
            //}
            stopwatch.Stop();
            UnityEngine.Debug.Log($"ºÄÊ±£º{stopwatch.ElapsedMilliseconds}");
        }

        [BurstCompile]
        public struct TestJob2 : IJobParallelFor
        {
            public NativeArray<XFix64Vector2> positions;
            public NativeArray<XFix64Vector2> velocities;
            public float deltaTime;

            public void Execute(int index)
            {
                positions[index] += velocities[index] * deltaTime;
            }
        }
    }
}