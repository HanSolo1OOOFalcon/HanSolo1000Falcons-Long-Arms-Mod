using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static HanSolo1000FalconsLongArmsMod.Plugin;

namespace BananaOS.Pages
{
    public class BananaOsSupport : WatchPage
    {
        public override string Title => "HanSolo Long Arms";

        public override bool DisplayOnMainMenu => ciEnabled;

        public override void OnPostModSetup()
        {
            selectionHandler.maxIndex = 6;
        }

        public override string OnGetScreenContent()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<color=yellow>==</color> HanSolo1000Falcons Long Arms Mod <color=yellow>==</color>");
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(0, "Toggled"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(1, "Reset Arm Length\n"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(2, "Longer Arms"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(3, "Shorter Arms\n"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(4, "Toggle Balls"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(5, "Bigger BALLS"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(6, "Smaller BALLS"));
            return stringBuilder.ToString();
        }

        public override void OnButtonPressed(WatchButtonType buttonType)
        {
            switch (buttonType)
            {
                case WatchButtonType.Up:
                    selectionHandler.MoveSelectionUp();
                    break;

                case WatchButtonType.Down:
                    selectionHandler.MoveSelectionDown();
                    break;

                case WatchButtonType.Enter:
                    if (selectionHandler.currentIndex == 0)
                    {
                        osToggled = !osToggled;
                        BananaNotifications.DisplayNotification(osToggled ? "<align=center><size=7> Enabled" : "<align=center><size=7> Disabled", Color.yellow, osToggled ? Color.green : Color.red, 1f);
                        return;
                    }
                    if (selectionHandler.currentIndex == 1)
                    {
                        playerScale = 1f;
                        return;
                    }
                    if (selectionHandler.currentIndex == 2)
                    {
                        playerScale += increment;
                        return;
                    }
                    if (selectionHandler.currentIndex == 3)
                    {
                        playerScale -= increment;
                        return;
                    }
                    if (selectionHandler.currentIndex == 4)
                    {
                        BALLS = !BALLS;
                        BananaNotifications.DisplayNotification(BALLS ? "<align=center><size=7> Enabled" : "<align=center><size=7> Disabled", Color.magenta, BALLS ? Color.green : Color.red, 1f);
                        return;
                    }
                    if (selectionHandler.currentIndex == 5)
                    {
                        ballScale += 0.025f;
                        BananaNotifications.DisplayNotification("<align=center><size=5> Ball size is now: " + ballScale, Color.yellow, Color.white, 1f);
                        return;
                    }
                    if (selectionHandler.currentIndex == 6)
                    {
                        ballScale -= 0.025f;
                        BananaNotifications.DisplayNotification("<align=center><size=5> Ball size is now: " + ballScale, Color.magenta, Color.white, 1f);
                        return;
                    }
                    break;

                //It is recommended that you keep this unless you're nesting pages if so you should use the SwitchToPage method
                case WatchButtonType.Back:
                    ReturnToMainMenu();
                    break;
            }
        }
    }
}
