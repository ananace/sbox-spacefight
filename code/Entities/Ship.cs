namespace SpaceFight.Entities
{
	public abstract partial class Ship : Sandbox.BasePhysics
	{
		[Net] Vector3 _InputForces { get; set; }
		[Net] Rotation _InputRotation { get; set; }
		[Net] Vector3 _RequestedForces { get; set; }
		[Net] Rotation _RequestedRotation { get; set; }

		public override void Spawn()
		{
			SetModel("models/citizen_props/hotdog01.vmdl");

			base.Spawn();

			PhysicsBody.GravityEnabled = false;
			PhysicsBody.DragEnabled = false;
		}

		[Sandbox.Event.Physics.PostStep]
		protected void ApplyForces()
		{
			var body = PhysicsBody;
			if (!body.IsValid())
				return;


		}

		public override void Simulate(Client cl)
		{
			base.Simulate(cl);

			if (IsServer)
			{
				_InputForces.x = Input.Left;
				_InputForces.y = Input.Up;
				_InputForces.z = Input.Forward;

				_InputForces.y = (Input.Down(InputButton.Jump) ? 1.0f : -1.0f) - (Input.Down(InputButton.Duck) ? 1.0f : -1.0f);

				_InputRotation = Input.Rotation;
			}

			var body = PhysicsBody;
			if (body.IsValid())
				EyeRot = body.Rotation;
		}
	}
}
