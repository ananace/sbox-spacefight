using Sandbox;

namespace SpaceFight.Entities
{
	public abstract partial class Missile : Sandbox.BasePhysics
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

		/// <summary>Fuel In Seconds</summary>
		[Net] public float Fuel { get; set; }
		[Net] public MissileState State { get; set; } = MissileState.Inactive;
		[Net] public Entity Target { get; set; }

		[Net] private float Timer { get; set; } = 0.0f;

		public SpaceFight.Assets.MissileDefinition Definition { get; set; }

		public override void Spawn()
		{
			SetModel(Definition.Model);
			SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);

			base.Spawn();

			Fuel = Definition.FuelSeconds;

			Tags.Add("targetable", "missile", "ordnance");

			PhysicsBody.GravityEnabled = false;
			PhysicsBody.DragEnabled = false;
		}

		public override void TakeDamage(DamageInfo info)
		{
			// Don't damage an un-launched missile
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
				// TODO Fuel explosion?
			}

			Fuel = 0;
			State = MissileState.Dead;

			base.OnKilled();
		}

		public virtual void Seek()
		{
			if (Target == null)
				return;

			var destination = Target.Position + Target.Velocity; // * (Distance / this.Velocity)
		}

		[Event.Tick]
		public void OnTick()
		{
			if (State == MissileState.Inactive)
				return;

			if (State == MissileState.Launched)
			{
				Timer += Time.Delta;
				if (Timer > Definition.IgnitionTime)
				{
					State = MissileState.Ignited;
					Timer = 0;
				}
				return;
			}
			else if (State == MissileState.Ignited)
			{
				Timer += Time.Delta;
				if (Timer > Definition.ArmingTime)
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
				if (Timer > Definition.CoastingTime)
				{
					var warhead = Components.Get<SpaceFight.Components.Warhead>();
					warhead?.Detonate();
					Health = 0;
					Timer = 0;
				}
				return;
			}
			Fuel -= Time.Delta;
			if (State == MissileState.SuicideBurn)
				Fuel -= Time.Delta; // 2x fuel burn

			if (State != MissileState.SuicideBurn)
				Seek();

			if (Target != null && Definition.SuicideBurnDistance > 0)
			{
				if (Position.Distance(Target.Position) <= Definition.SuicideBurnDistance)
					State = MissileState.SuicideBurn;
			}

			if (Fuel <= 0)
				State = MissileState.BurnedOut;
		}

		[Sandbox.Event.Physics.PostStep]
		protected void ApplyForces()
		{
			if (State < MissileState.Ignited || State >= MissileState.BurnedOut)
				return;

			var body = PhysicsBody;
			if (!body.IsValid())
				return;

			float acceleration = Definition.AccelerationForce;
			if (State == MissileState.SuicideBurn)
				acceleration *= 2.0f;
			body.ApplyForce(body.Rotation.Forward * body.Mass * acceleration);
		}
	}
}
