namespace SpaceFight.Entities
{
	public abstract partial class Weapon : Sandbox.Entity
	{
		[Net] public SpaceFight.Assets.WeaponDefinition Definition { get; set; }
		public float FireRate { get { return 60.0f / WeaponDefinition.RoundsPerMinute; } }

		[Net] public float RefireTimer { get; protected set; } = 0;
		[Net] public int CurrentRounds { get; protected set; }
		[Net] public int CurrentMagazines { get; protected set; }

		public bool CanFire { get { return CurrentRounds > 0 && RefireTimer <= 0; } }

		SpaceFight.Assets.MissileDefinition _OrdnanceDefinition { get; set; }

		public void RefreshAmmunition()
		{
			if (!IsServer)
				return;
			CurrentMagazines = MaximumMagazines;
			CurrentRounds = RoundsPerMagazine;
			RefireTimer = 0;
		}

		public void Reload()
		{
			if (!IsServer)
				return;
			if (CurrentMagazines <= 0)
				return;

			RefireTimer = ReloadTime;
			CurrentRounds = RoundsPerMagazine;
			CurrentMagazines--;
		}

		public virtual void Fire()
		{
			if (!IsServer)
				return;
			if (!CanFire)
				return;

			CurrentRounds--;
			RefireTimer = FireRate;

			switch (Definition.WeaponType)
			{
			case Definition.WeaponType.Ordnance:
				{
					var ordnance = new SpaceFight.Entities.Missile {
						Definition = _OrdnanceDefinition
					};
					ordnance.Position = EyePos + EyeRot.Forward * missile.OOBBox.Size.Length * 1.5f;
					ordnance.Rotation = EyeRot;
					ordnance.Rotation.RotateAroundAxis(missile.Rotation.Forward, Sandbox.Rand.Float() * 360.0f);
					ordnance.PhysicsGroup.Velocity = Velocity + EyeRot.Forward * 100;
					ordnance.State = MissileState.Launched;
					ordnance.Spawn();
				}
				break;
			}
		}

		[Event.Tick]
		public void OnTick()
		{
			if (RefireTimer > 0)
				RefireTimer -= Time.Delta;
		}
	}
}
