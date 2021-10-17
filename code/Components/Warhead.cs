using Sandbox;
using System;

namespace SpaceFight.Components
{
	public partial class Warhead : Sandbox.EntityComponent
	{
		[Net] public bool Armed { get; set; } = false;
		[Net] public float Power { get; set; } = 100.0f;

		// public string ExplosionType { get; set; } = "generic";

		public void Detonate()
		{
			if (!Armed)
				return;

			// TODO explode
			Log.Info("Warhead goes Boom.");
		}

		public void Arm()
		{
			Armed = true;
			Log.Info("Warhead is Armed.");
		}
	}
}
