namespace SpaceFight.Entities.Weapons
{
	public partial class DumbfireMissilePod : SpaceFight.Entities.Weapon
	{
		DumbfireMissilePod()
		{
			Weapon();

			RoundsPerMinute = 120;
			// 180 rounds in total
			RoundsPerMagazine = 60;
			MaximumMagazines = 2;
		}

		void Fire()
		{
			base.Fire();

			if (IsServer)
			{
				var missile = new SpaceFight.Entities.Ordnance.DumbfireMissile();
				missile.Position = EyePos + EyeRot.Forward * missile.OOBBox.Size.Length * 1.5f;
				missile.Rotation = EyeRot;
				missile.Rotation.RotateAroundAxis(missile.Rotation.Forward, Sandbox.Rand.Float() * 360.0f);
				missile.PhysicsGroup.Velocity = Velocity + EyeRot.Forward * 100;
				missile.State = MissileState.Launched;
				missile.Spawn();
			}
		}
	}
}
