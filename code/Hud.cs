using Sandbox.UI;

namespace SpaceFight
{
	/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class HudEntity : Sandbox.HudEntity<RootPanel>
	{
		public HudEntity()
		{
			if (IsClient)
			{
				RootPanel.SetTemplate("/minimalhud.html");
			}
		}
	}

}
