namespace SpaceFight.Entities
{
	public abstract partial class Ship : Sandbox.BasePhysics
	{
		[Net] Vector3 _InputForces { get; set; }
		[Net] Rotation _InputRotation { get; set; }
		[Net] Vector3 _RequestedForces { get; set; }
		[Net] Rotation _RequestedRotation { get; set; }

		[BindComponent]
		public SpaceFight.Components.IFCS IFCS { get; }

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

			body.ApplyForce(body.Rotation.Forward * _RequestedForces.z * body.Mass);
			body.ApplyForce(body.Rotation.Left * _RequestedForces.y * body.Mass);
			body.ApplyForce(body.Rotation.Up * _RequestedForces.x * body.Mass);

			if (!IsServer)
				return;
			_RequestedForces.Clear();
			_RequestedRotation.Clear();
		}

		// When being directly controlled by a player
		public override void Simulate(Client cl)
		{
			base.Simulate(cl);
			{
				_InputForces.x = Input.Left;
				_InputForces.y = Input.Up;
				_InputForces.z = Input.Forward;

				if (_InputForces.y == 0)
					_InputForces.y = (Input.Down(InputButton.Jump) ? 1.0f : -1.0f) - (Input.Down(InputButton.Duck) ? 1.0f : -1.0f);

				_InputRotation = Input.Rotation;

				if (IFCS != null)
				{
					IFCS.Simulate(cl, _InputForces, _InputRotation, _RequestedForces, _RequestedRotation);
				}
				else
				{
					_RequestedForces = _InputForces;
					_RequestedRotation = _InputRotation;
				}
				_HasForces = true;
			}

			var body = PhysicsBody;
			if (body.IsValid())
				EyeRot = body.Rotation;
		}
	}
}
