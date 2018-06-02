using FrooxEngine.UI;
using BaseX;
using System.Runtime.InteropServices;
using System;
using System.Text;
using FrooxEngine;

namespace VirtualKeyboard
{
    [Category("Utility")]
    public class VirtualKeyboard : Component
    {
        const string loweralpha = @"1234567890-=qwertyuiop[]asdfghjkl;'\zxcvbnm,./";
        const string upperalpha = "!@#$%^&*()_+QWERTYUIOP{}ASDFGHJKL:\"|ZXCVBNM<>?";

        const string numeric = @"7894561230.-+*/";

        public bool IsShown
        {
            get => Slot.ActiveSelf;
            set => Slot.ActiveSelf = value;
        }

        protected override void OnAttach()
        {
            var grabbable = Slot.AttachComponent<Grabbable>();
            grabbable.Scalable = true;
            grabbable.ShouldPreserveUp.Value = false;

            // build the keyboard
            var ui = new UIBuilder(Slot, 640, 160, 0.001f);
            ui.Image(new color(1f, 1f, 1f, 0.2f));
            ui.Nest();

            var panels = ui.SplitHorizontally(0.1f, 0.6f, 0.15f, 0.15f);
            Button keyButton;

            // Leftmost, Escape, tab, shift, desktop 
            ui = new UIBuilder(panels[0].Slot);
            ui.GridLayout(new float2(62, 30), new float2(2f, 2f)).PaddingTop.Value = 2;

            keyButton = ui.Button("Esc");
            // Setup individual panels
            VirtualKey virtualKeyEsc = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKeyEsc.TargetKey.Value = Key.Escape;

            //ui.Empty("Filler");
            ui.Button("<i>(hide)</i>", new color(1f, 0.5f), HidePressed);

            keyButton = ui.Button("Tab");
            VirtualKey virtualKeyTab = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKeyTab.AppendString.Value = "\t";
            virtualKeyTab.TargetKey.Value = Key.Tab;

            keyButton = ui.Button("Shift");
            var shiftKey = keyButton.Slot.AttachComponent<VirtualShift>();

            keyButton = ui.Button("Desktop");
            var desktopKey = keyButton.Slot.AttachComponent<VirtualDesktop>();
            virtualKeyEsc.DesktopKey.Target = desktopKey;
            virtualKeyTab.DesktopKey.Target = desktopKey;

            // Alphanumeric
            ui = new UIBuilder(panels[1].Slot);

            ui.VerticalLayout(2f);

            ui.GridLayout(float2.One * 30, new float2(2f, 2f)).PaddingTop.Value = 2;

            VirtualKey virtualKey;
            for (int i = 0; i < loweralpha.Length; i++)
            {
                keyButton = ui.Button(loweralpha[i].ToString());
                virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
                virtualKey.AppendString.Value = loweralpha[i].ToString();
                virtualKey.ShiftAppendString.Value = upperalpha[i].ToString();
                virtualKey.ShiftKey.Target = shiftKey;
                virtualKey.DesktopKey.Target = desktopKey;
                // TODO!!! Mapping to a key
            }
            
            ui.NestOut();

            ui.Style.PreferredHeight = 30;
            ui.Panel();

            keyButton = ui.Button();
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.AppendString.Value = " ";
            virtualKey.TargetKey.Value = Key.Space;
            virtualKey.DesktopKey.Target = desktopKey;

            var rect = keyButton.Slot.GetComponent<RectTransform>();
            rect.AnchorMin.Value = new float2(0.1f, 0f);
            rect.AnchorMax.Value = new float2(0.9f, 1f);

            // Special section
            ui = new UIBuilder(panels[2].Slot);
            var vertLayout = ui.VerticalLayout(2f);
            vertLayout.PaddingTop.Value = 2;
            vertLayout.PaddingRight.Value = 2;

            ui.Style.PreferredWidth = 32 * 3 - 2;
            ui.Style.PreferredHeight = 30;

            keyButton = ui.Button("<-- Backspace");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.AppendString.Value = "\b";
            virtualKey.TargetKey.Value = Key.Backspace;
            virtualKey.DesktopKey.Target = desktopKey;

            ui.Style.PreferredHeight = 30 + 32;
            keyButton = ui.Button("Enter\n⏎");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.AppendString.Value = "\n";
            virtualKey.TargetKey.Value = Key.Return;
            virtualKey.DesktopKey.Target = desktopKey;

            ui.Style.PreferredHeight = 32;
            ui.GridLayout(new float2(30f, 30f), new float2(2f, 2f));

            ui.Empty("Filler");

            keyButton = ui.Button("↑");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.TargetKey.Value = Key.UpArrow;
            virtualKey.DesktopKey.Target = desktopKey;

            ui.Empty("Filler");

            keyButton = ui.Button("←");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.TargetKey.Value = Key.LeftArrow;
            virtualKey.DesktopKey.Target = desktopKey;

            keyButton = ui.Button("↓");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.TargetKey.Value = Key.DownArrow;
            virtualKey.DesktopKey.Target = desktopKey;

            keyButton = ui.Button("→");
            virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
            virtualKey.TargetKey.Value = Key.RightArrow;
            virtualKey.DesktopKey.Target = desktopKey;

            // Numeric
            ui = new UIBuilder(panels[3].Slot);
            ui.GridLayout(float2.One * 30, new float2(2f, 2f)).PaddingTop.Value = 2;

            for (int i = 0; i < numeric.Length; i++)
            {
                keyButton = ui.Button(numeric[i].ToString());
                virtualKey = keyButton.Slot.AttachComponent<VirtualKey>();
                virtualKey.AppendString.Value = numeric[i].ToString();
            }

            Debug.Log("done Loading Keyboard");
        }

        void HidePressed(Button button)
        {
            IsShown = false;
        }
    }
}
