using Sandbox;

namespace SpaceFight
{
	partial class Player : Sandbox.Player
	{
		Vector3 _currentInput = new Vector3();
		Rotation _currentRotation = new Rotation();

		public override void Respawn()
		{
			SetModel("models/citizen_props/hotdog01.vmdl");

			Camera = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			base.Respawn();

			SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);

			MoveType = MoveType.Physics;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
			for (int i = 0; i < PhysicsGroup.BodyCount; ++i)
				PhysicsGroup.GetBody(i).GravityEnabled = false;

			PhysicsBody.DragEnabled = true;
		}

		public virtual float mainSpeed => 200;
		public virtual float strafeSpeed => 40;

		[Sandbox.Event.Physics.PostStep]
		protected void ApplyForces()
		{
			var body = PhysicsBody;
			if (!body.IsValid())
				return;

			//body.AngularDrag = 1.00f;
			//body.AngularDamping = 4.00f;
			body.LinearDrag = 0.05f;
			body.LinearDamping = 0.20f;

			body.ApplyForce(body.Rotation.Forward * _currentInput.z * body.Mass * mainSpeed);
			body.ApplyForce(body.Rotation.Left * _currentInput.x * body.Mass * strafeSpeed);
			body.ApplyForce(body.Rotation.Up * _currentInput.y * body.Mass * strafeSpeed);

			//var rot = new Vector3(_currentRotation.Pitch() - body.Rotation.Pitch(), _currentRotation.Roll() - body.Rotation.Roll(), _currentRotation.Yaw() - body.Rotation.Yaw());
			//body.Rotation += (_currentRotation - body.Rotation) * Time.Delta;

			body.Rotation = _currentRotation;
			//body.ApplyTorque((rot) * body.Mass * 0.1f);
		}

		public override void Simulate(Client cl)
		{
			base.Simulate(cl);

			if (IsServer)
			{
				_currentInput.x = Input.Left;
				_currentInput.y = Input.Up;
				_currentInput.z = Input.Forward;

				_currentInput.y = (Input.Down(InputButton.Jump) ? 1.0f : -1.0f) - (Input.Down(InputButton.Duck) ? 1.0f : -1.0f);

				_currentRotation = Input.Rotation;

				EyeRot = _currentRotation;
			}

			// TODO: Move handling onto child, to separate the player and entity concerns
			SimulateActiveChild(cl, ActiveChild);
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
