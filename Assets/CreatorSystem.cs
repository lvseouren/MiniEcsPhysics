using System;
using System.Collections.Generic;
using System.Linq;
using MiniEcs.Core;
using MiniEcs.Core.Systems;
using Physics;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using XFixMath.NET;

[EcsUpdateBefore(typeof(InputSystem))]
public class CreatorSystem : IEcsSystem
{
	private readonly PhysicsScene _physicsScene;
	private readonly CollisionMatrix _collisionMatrix;
		
	private readonly EcsFilter _staticRectFilter;
	private readonly EcsFilter _staticCircleFilter;
	private readonly EcsFilter _dynamicBlueCircleFilter;
	private readonly EcsFilter _dynamicYellowCircleFilter;
	private readonly EcsFilter _heroFilter;

	public CreatorSystem(PhysicsScene physicsScene, CollisionMatrix collisionMatrix)
	{
		_physicsScene = physicsScene;
		_collisionMatrix = collisionMatrix;
			
		_heroFilter = new EcsFilter().AllOf<HeroComponent>();
			
		_staticRectFilter = new EcsFilter().AllOf<StaticRectComponent, CharacterComponent, BroadphaseRefComponent>();
		_staticCircleFilter = new EcsFilter().AllOf<StaticCircleComponent, CharacterComponent, BroadphaseRefComponent>();
		_dynamicBlueCircleFilter = new EcsFilter().AllOf<BlueCircleComponent, CharacterComponent, BroadphaseRefComponent>();
		_dynamicYellowCircleFilter = new EcsFilter().AllOf<YellowCircleComponent, CharacterComponent, BroadphaseRefComponent>();
	}

	public void Update(XFix64 deltaTime, EcsWorld world)
	{
		CreateOrDestroyEntities(world, _staticRectFilter, _physicsScene.StaticRectCount, CreateStaticRect);
		CreateOrDestroyEntities(world, _staticCircleFilter, _physicsScene.StaticCircleCount, CreateStaticCircle);
		CreateOrDestroyEntities(world, _dynamicBlueCircleFilter, _physicsScene.DynamicBlueCircleCount, CreateDynamicBlueCircle);
		CreateOrDestroyEntities(world, _dynamicYellowCircleFilter, _physicsScene.DynamicYellowCircleCount, CreateDynamicYellowCircle);

		if (world.Filter(_heroFilter).CalculateCount() > 0) 
			return;
			
		IEcsEntity heroEntity = CreateCircleEntity(world, XFix64Vector2.zero, 0, 5, 1, "Default", 150);
		heroEntity.AddComponent(new HeroComponent());
		Instantiate(_physicsScene.Hero, heroEntity);
	}
		
	private static void CreateOrDestroyEntities(EcsWorld world, EcsFilter filter, int count, Action<EcsWorld> createEntity)
	{
		IEcsGroup group = world.Filter(filter);
		if (group.CalculateCount() == count) 
			return;

		IEcsEntity[] entities = group.ToEntityArray();
		for (int i = entities.Length; i < count; i++)
		{
			createEntity(world);
		}
			
		for (int i = count; i < entities.Length; i++)
		{
			IEcsEntity entity = entities[i];
			BroadphaseRefComponent brRef = entity.GetComponent<BroadphaseRefComponent>();
			CharacterComponent character = entity.GetComponent<CharacterComponent>();

			Object.Destroy(character.Ref.gameObject);

			foreach (SAPChunk chunk in brRef.Chunks)
				BroadphaseHelper.RemoveFormChunk(chunk, entity.Id);

			entity.Destroy();
		}
	}
		
	private static void CalculateTransform(out XFix64Vector2 position, out XFix64 rotation)
	{
		XFix64 x = MathXFix64.random.NextXFix64(-1000, 1000);
		XFix64 y = MathXFix64.random.NextXFix64(-1000, 1000);
        position = new XFix64Vector2(x, y);
		rotation = MathXFix64.random.NextXFix64(-XFix64.Pi, XFix64.Pi);
	}

	private void CreateDynamicYellowCircle(EcsWorld world)
	{
		CalculateTransform(out XFix64Vector2 position, out XFix64 rotation);
			
		XFix64 radius = MathXFix64.random.NextXFix64(2, 4);
		IEcsEntity circleEntity = CreateCircleEntity(world, position, rotation, radius, 1, "yellow", 0);
		circleEntity.AddComponent(new YellowCircleComponent());
		Instantiate(_physicsScene.DynamicYellowCircle, circleEntity);
	}

	private void CreateDynamicBlueCircle(EcsWorld world)
	{
		CalculateTransform(out XFix64Vector2 position, out XFix64 rotation);

		XFix64 radius = MathXFix64.random.NextXFix64(2, 4);
		IEcsEntity circleEntity = CreateCircleEntity(world, position, rotation, radius, 1, "blue", 100);
		circleEntity.AddComponent(new BlueCircleComponent());
		Instantiate(_physicsScene.DynamicBlueCircle, circleEntity);
	}

	private void CreateStaticRect(EcsWorld world)
	{
		CalculateTransform(out XFix64Vector2 position, out XFix64 rotation);

		XFix64Vector2 size = new XFix64Vector2(MathXFix64.random.NextXFix64(5, 10), MathXFix64.random.NextXFix64(5, 10));
		IEcsEntity rectEntity = CreateRectEntity(world, position, rotation, size, 0, "Default", 0);
		rectEntity.AddComponent(new StaticRectComponent());
		Instantiate(_physicsScene.StaticRect, rectEntity);
	}

	private void CreateStaticCircle(EcsWorld world)
	{
		CalculateTransform(out XFix64Vector2 position, out XFix64 rotation);

		XFix64 radius = MathXFix64.random.NextXFix64(5, 10);
		IEcsEntity circleEntity = CreateCircleEntity(world, position, rotation, radius, 0, "Default", 0);
		circleEntity.AddComponent(new StaticCircleComponent());
		Instantiate(_physicsScene.StaticCircle, circleEntity);
	}

	private static void Instantiate(GameObject prefab, IEcsEntity entity)
	{
		TransformComponent tr = entity.GetComponent<TransformComponent>();
		ColliderComponent col = entity.GetComponent<ColliderComponent>();

		Vector3 pos = new Vector3(tr.Position.x, 0, tr.Position.y);
		Quaternion rotation = Quaternion.Euler(0, -Mathf.Rad2Deg * tr.Rotation, 0);
		GameObject go = Object.Instantiate(prefab, pos, Quaternion.identity);
		go.transform.position = pos;
		go.transform.rotation = rotation;

		Character character = go.GetComponent<Character>();
		character.ScaleTransform.localScale = new Vector3(2 * col.Size.x, 1, 2 * col.Size.y);
		if (character.RayGameObject)
			character.RayGameObject.SetActive(entity.HasComponent<RayComponent>());
		entity.AddComponent(new CharacterComponent {Ref = character});
	}

	private IEcsEntity CreateCircleEntity(EcsWorld world, XFix64Vector2 position, XFix64 rotation, XFix64 radius, XFix64 mass, string colliderLayer, XFix64 rayLength)
	{
		return CreateEntity(world, position, rotation, new ColliderComponent
			{
				ColliderType = ColliderType.Circle,
				Size = radius,

				Layer = _collisionMatrix.GetLayer(colliderLayer)
			}, 
			mass, rayLength);
	}

	private IEcsEntity CreateRectEntity(EcsWorld world, XFix64Vector2 position, XFix64 rotation, XFix64Vector2 size, XFix64 mass,
		string colliderLayer, XFix64 rayLength)
	{
		return CreateEntity(world, position, rotation, new ColliderComponent
			{
				ColliderType = ColliderType.Rect,
				Size = size,
				Layer = _collisionMatrix.GetLayer(colliderLayer)
			},
			mass, rayLength);
	}

	private IEcsEntity CreateEntity(EcsWorld world, XFix64Vector2 position, XFix64 rotation, ColliderComponent col, XFix64 mass, XFix64 rayLength)
	{
		IEcsEntity entity = world.CreateEntity(
			col,
			new RigBodyComponent
			{
				Velocity = new XFix64Vector2(Random.Range(-10, 10), Random.Range(-10, 10)),
				Mass = mass
			},
			new TransformComponent {Position = position, Rotation = rotation}
		);

		if (mass <= 0)
		{
			entity.AddComponent(new RigBodyStaticComponent());
		}

		if (rayLength > 0)
		{
			entity.AddComponent(new RayComponent
			{
				Length = rayLength,
				Layer = _collisionMatrix.GetLayer("Default")
			});
		}

		return entity;
	}
}