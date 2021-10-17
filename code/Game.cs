using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SpaceFight
{
	public partial class Game : Sandbox.Game
	{
		public Game()
		{
			if (IsServer) 
			{
				Log.Info("My Gamemode Has Created Serverside!");

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
				new HudEntity();
			}

			if (IsClient)
			{
				Log.Info("My Gamemode Has Created Clientside!");
			}
		}

		public override void ClientJoined(Client client)
		{
			base.ClientJoined(client);

			var player = new Player();
			client.Pawn = player;

			player.Respawn();
		}
	}

}
