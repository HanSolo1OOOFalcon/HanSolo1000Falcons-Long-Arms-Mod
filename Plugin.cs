using System;
using BepInEx;
using UnityEngine;
using Utilla;

namespace LongArms
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
		void OnEnable()
		{
			/* Set up your mod here */
			/* Code here runs at the start and whenever your mod is enabled*/

			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			/* Undo mod setup here */
			/* This provides support for toggling mods with ComputerInterface, please implement it :) */
			/* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
		}

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(Init);
        }

        private void Init()
        {
            NetworkSystem.Instance.OnJoinedRoomEvent += OnJoinedRoom;
            NetworkSystem.Instance.OnReturnedToSinglePlayer += OnLeftRoom;
        }

        private void OnJoinedRoom()
        {
            if (NetworkSystem.Instance.GameModeString.Contains("MODDED"))
            {
                if (GorillaLocomotion.Player.Instance != null)
                {
                    GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                }
            }
        }
        private void OnLeftRoom()
        {
            if (GorillaLocomotion.Player.Instance != null)
            {
                GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

	public static float playerScale = 1f;
	private static bool yPress = false;
        private static float lastTime = 0f;
       	private static bool toggled = true;
	public static bool osToggled = true;
        void FixedUpdate()
        {
	bool lT = ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
	bool lG = ControllerInputPoller.instance.leftGrab;
	bool Y = ControllerInputPoller.instance.leftControllerSecondaryButton;
	bool X = ControllerInputPoller.instance.leftControllerPrimaryButton;

	if (NetworkSystem.Instance.GameModeString.Contains("MODDED"))
	{
		float increment = 0.05f;
		float cooldown = 0.05f;
		float max = 3f;
		float least = 0.5f;

		if (lT && toggled && !lG && osToggled)
		{
			if (lastTime >= cooldown)
			{
				lastTime = 0f;
				playerScale += increment;
			}
		}

                if (lG && toggled && !lT && osToggled)
                {
                    	if (lastTime >= cooldown)
                    	{
                        	lastTime = 0f;
                        	playerScale -= increment;
                    	}
                }

		if (Y && osToggled)
		{
			if (!yPress)
			{
				yPress = true;
				toggled = !toggled;
			}
		}
		else
		{
			yPress = false;
		}

		if (playerScale > max)
		{
			playerScale = max;
		}

		if (playerScale < least)
		{
			playerScale = least;
		}

		if (X && toggled && osToggled)
		{
			playerScale = 1f;
		}
				
		if (GorillaLocomotion.Player.Instance != null)
		{
                    	GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                }

		lastTime += Time.deltaTime;
            }
	}
    }
}
