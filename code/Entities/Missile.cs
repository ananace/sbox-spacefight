using Sandbox;
namespace SpaceFight.Entities
{
	public partial class Missile : Sandbox.BasePhysics
	{
		public enum MissileState
		{
			Inactive,
			Launched,
			Ignited,
			Armed,
			SuicideBurn,
			BurnedOut,
			Dead
		}

		[Net] float Fuel { get; set; } = 100.0f;
		[Net] float Timer { get; set; } = 0.0f;
		[Net] MissileState State { get; set; } = MissileState.Inactive;

		public override void TakeDamage(DamageInfo info)
		{
			if (State == MissileState.Inactive)
			{
				ApplyDamageForces(info);
				return;
			}

			base.TakeDamage( info );
		}

		public override void OnKilled()
		{
			var warhead = Components.Get<SpaceFight.Components.Warhead>();
			if (warhead != null)
				warhead.Detonate();
			else if (Fuel > 0)
			{
				// TODO Fuel explosion
			}

			Fuel = 0;
			State = MissileState.Dead;

			base.OnKilled();
		}

		[Event.Tick]
		public void OnTick()
		{
			if (State == MissileState.Inactive)
				return;

			if (State == MissileState.Launched)
			{
				Timer += Time.Delta;
				if (Timer > 1.0f)
				{
					State = MissileState.Ignited;
					Timer = 0;
				}
				return;
			}
			else if (State == MissileState.Ignited)
			{
				Timer += Time.Delta;
				const float armingTime = 0.5f;
				if (Timer > armingTime)
				{
					var warhead = Components.Get<SpaceFight.Components.Warhead>();
					warhead?.Arm();

					State = MissileState.Armed;
					Timer = 0;
				}
			}
			else if (State >= MissileState.BurnedOut)
			{
				Timer += Time.Delta;
				const float coastLifetime = 10.0f;
				if (Timer > coastLifetime)
				{
					var warhead = Components.Get<SpaceFight.Components.Warhead>();
					warhead?.Detonate();
					// Destroy();
					Timer = 0;
				}
				return;
			}
			Fuel -= Time.Delta;
			if (State == MissileState.SuicideBurn)
				Fuel -= Time.Delta; // 2x fuel burn

			if (State != MissileState.SuicideBurn)
				;// Seek();

			const float suicideFuelPoint = 10.0f;
			if (Fuel <= 0)
				State = MissileState.BurnedOut;
			else if (Fuel <= suicideFuelPoint)
				State = MissileState.SuicideBurn;
		}

		[Sandbox.Event.Physics.PostStep]
		protected void ApplyForces()
		{
			if (State < MissileState.Ignited || State >= MissileState.BurnedOut)
				return;

			var body = PhysicsBody;
			if (!body.IsValid())
				return;

			float targetSpeed = 1000.0f;
			if (State == MissileState.SuicideBurn)
				targetSpeed *= 2.0f;
			body.ApplyForce(body.Rotation.Forward * body.Mass * targetSpeed);
		}
	}
}
