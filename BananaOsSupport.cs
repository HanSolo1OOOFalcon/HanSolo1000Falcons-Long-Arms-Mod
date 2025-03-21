using System;
using System.Collections.Generic;
using System.Text;
using BananaOS;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using static BananaOS.Plugin;
using static LongArms.Plugin;

namespace BananaOS.Pages
{
    public class BananaOsSupport : WatchPage
    {
        public override string Title => "HanSolo1000Falcons Long Arms";

        public override bool DisplayOnMainMenu => true;

        public override void OnPostModSetup()
        {
            selectionHandler.maxIndex = 3;
        }

        public override string OnGetScreenContent()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<color=yellow>==</color> HanSolo1000Falcons Long Arms <color=yellow>==</color>");
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(0, "Toggled"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(1, "Reset Arm Length"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(2, "Longer Arms"));
            stringBuilder.AppendLine(selectionHandler.GetOriginalBananaOSSelectionText(3, "Shorter Arms"));
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
                        return;
                    }
                    if (selectionHandler.currentIndex == 1)
                    {
                        playerScale = 1f;
                        return;
                    }
                    if (selectionHandler.currentIndex == 2)
                    {
                        playerScale += 0.1f;
                        return;
                    }
                    if (selectionHandler.currentIndex == 3)
                    {
                        playerScale -= 0.1f;
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
