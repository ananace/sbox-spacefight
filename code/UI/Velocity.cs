using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace SpaceFight.UI
{
    class Velocity : Panel
    {
        public Label infoLabel;
        public Label velocityLabel;

        public Velocity()
        {
            infoLabel = Add.Label("Velocity:", "velocityDescription");
            velocityLabel = infoLabel.Add.Label("0", "velocity");
            infoLabel.Add.Label("m/s", "velocityUnit");
        }

        public override void Tick()
        {
            var pawn = Local.Pawn;
            if (pawn == null)
                return;

            velocityLabel.Text = pawn.Velocity.Length.CeilToInt().ToString();
        }
    }
}
