using Sandbox;
using System;

namespace SpaceFight.Components
{
	[Flags]
	public enum IFCSAssists
	{
		None = 0,

		AngularAssist = 0x01,
		LinearAssist = 0x02,
	}

	public partial class IFCS : Sandbox.EntityComponent
	{
		[Net] public IFCSAssists Assists { get; set; } = IFCSAssists.None;

		[Event.Tick]
		public void OnTick()
		{
			var body = Entity.PhysicsGroup.GetBody(0);
			if (!body.IsValid())
				return;

			// TODO: Do this properly through counter-thrust

			if (Assists.HasFlag(IFCSAssists.LinearAssist))
			{
				body.LinearDrag = 1.0f;
				body.LinearDamping = 4.0f;
			}
			else
			{
				body.LinearDrag = 0.0f;
				body.LinearDamping = 0.0f;
			}

			if (Assists.HasFlag(IFCSAssists.AngularAssist))
			{
				body.AngularDrag = 1.0f;
				body.AngularDamping = 4.0f;
			}
			else
			{
				body.AngularDrag = 0.0f;
				body.AngularDamping = 0.0f;
			}
		}
	}
}
