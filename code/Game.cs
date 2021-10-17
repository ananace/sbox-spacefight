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
				new UI.MainHud();
			}

			if (IsClient)
			{
				// Do something
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
