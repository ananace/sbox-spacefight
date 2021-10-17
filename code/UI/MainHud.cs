using Sandbox.UI;

namespace SpaceFight.UI
{
	public partial class MainHud : Sandbox.HudEntity<RootPanel>
	{
		public MainHud()
		{
			if (!IsClient)
				return;

			RootPanel.StyleSheet.Load("/UI/MainHud.scss");

			RootPanel.AddChild<Velocity>();
		}
	}
}
