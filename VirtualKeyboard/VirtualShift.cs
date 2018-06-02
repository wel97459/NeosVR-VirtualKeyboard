using FrooxEngine.UI;
using BaseX;
using FrooxEngine;

namespace VirtualKeyboard1
{
    public class VirtualShift : Component, IButtonReceiver
    {
        public bool Shift { get { return _shift; } }

        readonly Sync<bool> _shift;
        readonly Sync<bool> _hold;
        readonly Sync<double> _lastPress;

        Button _button;

        public void KeyPressed()
        {
            if (!_hold.Value)
                _shift.Value = false;
        }

        public void Pressed(Button button)
        {
            if (button.PressingUser.IsLocal)
            {
                if (_button == null)
                    _button = Slot.GetComponentInChildren<Button>();

                if (!_shift.Value)
                    _shift.Value = true;
                else if (!_hold.Value && (Time.WorldTime - _lastPress.Value < 0.5f))
                {
                    _hold.Value = true;
                    _button.NormalColor.Value = color.Yellow;
                }
                else
                {
                    _shift.Value = false;
                    _hold.Value = false;
                    _button.NormalColor.Value = color.White;
                }

                _lastPress.Value = Time.WorldTime;
            }
        }

        public void Released(Button button) { }
    }
}
