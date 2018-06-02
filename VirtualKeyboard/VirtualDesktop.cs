using FrooxEngine.UI;
using BaseX;
using FrooxEngine;

namespace VirtualKeyboard
{
    public class VirtualDesktop : Component, IButtonReceiver
    {
        public bool Shift { get { return _shift; } }

        readonly Sync<bool> _shift;

        Button _button;

        public void Pressed(Button button)
        {
            if (button.PressingUser.IsLocal)
            {
                if (_button == null)
                    _button = Slot.GetComponentInChildren<Button>();

                if (!_shift.Value) {
                    _shift.Value = true;
                    _button.NormalColor.Value = color.Red;
                }
                else
                {
                    _shift.Value = false;
                    _button.NormalColor.Value = color.White;
                }
            }
        }

        public void Released(Button button) { }
    }
}
