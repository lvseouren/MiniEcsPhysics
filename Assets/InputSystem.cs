using MiniEcs.Core;
using MiniEcs.Core.Systems;
using Physics;
using Unity.Mathematics;
using UnityEngine;
using XFixMath.NET;

[EcsUpdateBefore(typeof(PhysicsSystemGroup))]
public class InputSystem : IEcsSystem
{
    private readonly EcsFilter _heroFilter;

    public InputSystem()
    {
        _heroFilter = new EcsFilter().AllOf<TransformComponent, RigBodyComponent, HeroComponent>();
    }

    public void Update(XFix64 deltaTime, EcsWorld world)
    {
        world.Filter(_heroFilter).ForEach((IEcsEntity entity, TransformComponent transform, RigBodyComponent rigBody) =>
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotation += 2 * deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotation -= 2 * deltaTime;
            }

            rigBody.Velocity = XFix64Vector2.zero;

            if (!Input.GetKey(KeyCode.W))
                return;

            XFix64 rad = transform.Rotation;
            XFix64.Sin(rad, out var sin);
            XFix64Vector2 dir = new XFix64Vector2(-sin, XFix64.Cos(rad));
            rigBody.Velocity = 25 * dir;
        });
    }
}