using sandbox;

namespace SpaceFight.Assets
{
	[Library("missile")]
	public class MissileDefinition : Asset
	{
		public enum SeekType
		{
			Dumbfire,
			FireAndForget,
			LockRequired
		}

		[Property, ResourceType("vmdl")]
		public string Model { get; set; }
		[Property]
		public float FuelSeconds { get; set; }

		[Property]
		public SeekType SeekType { get; set; }
		[Property]
		public float TargetConeDegrees { get; set; }

		[Property]
		public float IgnitionTime { get; set; } = 1.0f;
		[Property]
		public float ArmingTime { get; set; } = 0.5f;
		[Property]
		public float CoastingTime { get; set; } = 5.0f;
		[Property]
		public float AccelerationForce { get; set; } = 1000.0f;
		[Property]
		public float SuicideBurnDistance { get; set; } = 100.0f;

		[Property]
		public int SwarmCountOnDestruction { get; set; } = 0;
		[Property]
		public string SwarmDefinitionOnDestruction { get; set; } = null;
	}
}
