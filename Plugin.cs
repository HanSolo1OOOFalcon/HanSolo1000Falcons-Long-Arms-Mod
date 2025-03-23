using System;
using BepInEx;
using UnityEngine;
using Utilla;

namespace HanSolo1000FalconsLongArmsMod
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
	public static bool ciEnabled = true;
		
		void OnEnable()
		{
			ciEnabled = true;

			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			ciEnabled = false;

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
                if (ciEnabled)
				{
                    if (GorillaLocomotion.GTPlayer.Instance != null)
                    {
                        GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                    }
                }
		else
		{
			GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }
        private void OnLeftRoom()
        {
            if (GorillaLocomotion.GTPlayer.Instance != null)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

public static float playerScale = 1f;
private static bool yPress = false;
private static float lastTime = 0f;
private static bool toggled = true;
public static bool osToggled = true;

public static float increment = 0.025f;
public static float cooldown = 0.05f;
public static float max = 3f;
public static float least = 0.5f;
void FixedUpdate()
{
	bool lT = ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
	bool lG = ControllerInputPoller.instance.leftGrab;
	bool Y = ControllerInputPoller.instance.leftControllerSecondaryButton;
	bool X = ControllerInputPoller.instance.leftControllerPrimaryButton;

	if (NetworkSystem.Instance.InRoom)
	{
                if (NetworkSystem.Instance.GameModeString.Contains("MODDED"))
                {
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

                    if (!ciEnabled)
                    {
                        if (playerScale != 1f)
                        {
                            playerScale = 1f;
                        }
                    }

                    if (GorillaLocomotion.GTPlayer.Instance != null)
                    {
                        GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(playerScale, playerScale, playerScale);
                    }

                    lastTime += Time.deltaTime;
                }
            }
	}
    }
}
