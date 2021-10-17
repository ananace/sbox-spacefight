using Sandbox;
using System;
using System.Linq;

namespace SpaceFight
{
	partial class Player : Sandbox.Player
	{
		public override void Respawn()
		{
			SetModel("models/citizen_props/hotdog01.vmdl");

			Camera = new ThirdPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			MoveType = MoveType.Physics;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
			PhysicsBody.GravityEnabled = false;

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate(Client cl)
		{
			base.Simulate(cl);

			if (Input.Pressed(InputButton.Forward))
				PhysicsBody.ApplyImpulse(Vector3.Forward);
			if (Input.Pressed(InputButton.Back))
				PhysicsBody.ApplyImpulse(-Vector3.Forward);

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild(cl, ActiveChild);

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//
			if (IsServer && Input.Pressed(InputButton.Attack1))
			{
				var ragdoll = new ModelEntity();
				ragdoll.SetModel("models/citizen/citizen.vmdl");  
				ragdoll.Position = EyePos + EyeRot.Forward * 40;
				ragdoll.Rotation = Rotation.LookAt(Vector3.Random.Normal);
				ragdoll.SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);
				ragdoll.PhysicsGroup.Velocity = EyeRot.Forward * 1000;
			}
		}

		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
		}
	}
}
