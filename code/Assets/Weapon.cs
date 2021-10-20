using sandbox;

namespace SpaceFight.Assets
{
	[Library("weapon")]
	public class WeaponDefinition : Asset
	{
		public enum WeaponType
		{
			FixedWeapon,
			GimballedWeapon,
			TurretedWeapon,
			MissileSource
		}

		[Property]
		public WeaponType WeaponType { get; set; }
		[Property]
		public string Model { get; set; }

		[Property]
		public float RoundsPerMinute { get; set; }
		[Property]
		public float ReloadTime { get; set; }

		[Property]
		public int RoundsPerMagazine { get; set; }
		[Property]
		public int MaximumMagazines { get; set; } = -1;
	}
}
