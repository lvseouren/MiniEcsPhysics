using System;
using MiniEcs.Core;
using MiniEcs.Core.Systems;
using Unity.Mathematics;

namespace Physics
{
	[EcsUpdateInGroup(typeof(PhysicsSystemGroup))]
	[EcsUpdateAfter(typeof(BroadphaseCalculatePairSystem))]
	public class ResolveCollisionsSystem : IEcsSystem
	{
		public void Update(XFix64 deltaTime, EcsWorld world)
		{
			BroadphaseSAPComponent bpChunks =
				world.GetOrCreateSingleton<BroadphaseSAPComponent>();

			foreach (BroadphasePair pair in bpChunks.Pairs)
			{
				IEcsEntity entityA = pair.EntityA;
				IEcsEntity entityB = pair.EntityB;

				TransformComponent ta = entityA.GetComponent<TransformComponent>();
				TransformComponent tb = entityB.GetComponent<TransformComponent>();
				ColliderComponent ca = entityA.GetComponent<ColliderComponent>();
				ColliderComponent cb = entityB.GetComponent<ColliderComponent>();

				ContactInfo info;
				if (ca.ColliderType == ColliderType.Circle && cb.ColliderType == ColliderType.Circle)
				{
					OnCircleCircleCollision(ca, ta, cb, tb, out info);
				}
				else if (ca.ColliderType == ColliderType.Circle && cb.ColliderType == ColliderType.Rect)
				{
					OnCircleRectCollision(ca, ta, cb, tb, out info);
				}
				else if (ca.ColliderType == ColliderType.Rect && cb.ColliderType == ColliderType.Circle)
				{
					OnCircleRectCollision(cb, tb, ca, ta, out info);
					info.Normal = -info.Normal;
				}
				else
				{
					throw new InvalidOperationException();
				}

				if (!info.Hit)
					continue;
				
				RigBodyComponent ra = entityA.GetComponent<RigBodyComponent>();
				RigBodyComponent rb = entityB.GetComponent<RigBodyComponent>();

				float2 rv = rb.Velocity - ra.Velocity;

				XFix64 contactVel = math.dot(rv, info.Normal);
				XFix64 invMassSum = ra.InvMass + rb.InvMass;

				XFix64 f = -contactVel / invMassSum;
				float2 impulse = info.Normal * f * deltaTime;

				ra.Velocity -= ra.InvMass * impulse;
				rb.Velocity += rb.InvMass * impulse;

				float2 correction = info.Penetration / (ra.InvMass + rb.InvMass) * info.Normal * 0.5f;
				ta.Position -= correction * ra.InvMass;
				tb.Position += correction * rb.InvMass;
			}
		}

		private struct ContactInfo
		{
			public XFix64 Penetration;
			public float2 Normal;
			public float2 HitPoint;
			public bool Hit;
		}

		private static void OnCircleRectCollision(ColliderComponent ca, TransformComponent ta,
			ColliderComponent cb, TransformComponent tb, out ContactInfo contactInfo)
		{
			contactInfo = new ContactInfo {Hit = false};

			float2x2 rotate = float2x2.Rotate(tb.Rotation);
			float2 center = MathHelper.Mul(MathHelper.Transpose(rotate), ta.Position - tb.Position);

			XFix64 radius = ca.Size.x;
			XFix64 separation = XFix64.MinValue;
			int faceNormal = 0;
			for (int i = 0; i < 4; ++i)
			{
				XFix64 s = math.dot(cb.Normals[i], center - cb.Vertices[i]);
				if (s > radius)
					return;

				if (!(s > separation))
					continue;

				separation = s;
				faceNormal = i;
			}

			if (separation < MathHelper.EPSILON)
			{
				contactInfo.Hit = true;
				contactInfo.Normal = -MathHelper.Mul(rotate, cb.Normals[faceNormal]);
				contactInfo.HitPoint = contactInfo.Normal * radius + ta.Position;
				contactInfo.Penetration = radius;
				return;
			}
			
			contactInfo.Penetration = radius - separation;

			float2 v1 = cb.Vertices[faceNormal];
			float2 v2 = cb.Vertices[faceNormal + 1 < 4 ? faceNormal + 1 : 0];

			if (math.dot(center - v1, v2 - v1) <= 0.0f)
			{
				if (math.distancesq(center, v1) > radius * radius)
					return;

				contactInfo.Hit = true;
				contactInfo.Normal = math.normalizesafe(MathHelper.Mul(rotate, v1 - center));
				contactInfo.HitPoint = MathHelper.Mul(rotate, v1) + tb.Position;
				return;
			}
			
			if (math.dot(center - v2, v1 - v2) <= 0.0f)
			{
				if (math.distancesq(center, v2) > radius * radius)
					return;

				contactInfo.Hit = true;
				contactInfo.Normal = math.normalizesafe(MathHelper.Mul(rotate, v2 - center));
				contactInfo.HitPoint = MathHelper.Mul(rotate, v2) + tb.Position;
				return;
			}

			float2 n = cb.Normals[faceNormal];
			if (math.dot(center - v1, n) > radius)
				return;

			contactInfo.Normal = -MathHelper.Mul(rotate, n);
			contactInfo.HitPoint = contactInfo.Normal * radius + ta.Position;
			contactInfo.Hit = true;
		}

		private static void OnCircleCircleCollision(ColliderComponent ca, TransformComponent ta, ColliderComponent cb,
			TransformComponent tb, out ContactInfo contactInfo)
		{
			contactInfo = new ContactInfo();
			XFix64 caRadius = ca.Size.x;
			XFix64 cbRadius = cb.Size.x;
			
			float2 normal = tb.Position - ta.Position;
			XFix64 distSqr = math.lengthsq(normal);
			XFix64 radius = caRadius + cbRadius;

			if (distSqr >= radius * radius)
			{
				contactInfo.Hit = false;
				return;
			}

			XFix64 distance = math.sqrt(distSqr);
			contactInfo.Hit = true;

			if (MathHelper.Equal(distance, 0.0f))
			{
				contactInfo.Penetration = caRadius;
				contactInfo.Normal = new float2(1.0f, 0.0f);
				contactInfo.HitPoint = ta.Position;
			}
			else
			{
				contactInfo.Penetration = radius - distance;
				contactInfo.Normal = normal / distance;
				contactInfo.HitPoint = contactInfo.Normal * caRadius + ta.Position;
			}
		}
	}
}