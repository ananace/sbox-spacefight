using sandbox;

namespace SpaceFight.Assets
{
	[Library("weapon")]
	public class WeaponDefinition : Asset
	{
		public enum AimType
		{
			Fixed,
			Gimballed,
			Turreted
		}
		public enum WeaponType
		{
			/// <summary>Short pulse, hitscan</summary>
			Laser,
			/// <summary>Long pulse, hitscan</summary>
			Beam,
			/// <summary>Projectile - Star Wars style</summary>
			Plasma,
			/// <summary>Projectile - tracer style</summary>
			Ballistic,
			/// <summary>Launches ordnance entitites</summary>
			Ordnance
		}

		[Property]
		public AimType AimType { get; set; }
		[Property]
		public WeaponType WeaponType { get; set; }
		[Property]
		public string Ordnance { get; set; }

		[Property]
		public float AimCone { get; set; }

		[Property, ResourceType("vmdl")]
		public string Model { get; set; }

		[Property]
		public float RoundsPerMinute { get; set; }
		[Property]
		public float ReloadTime { get; set; }
		[Property]
		public float Damage { get; set; }

		[Property]
		public int RoundsPerMagazine { get; set; }
		[Property]
		public int MaximumMagazines { get; set; } = -1;
	}
}
