using System;
using BepInEx;
using UnityEngine;
using UnityEngine.XR;
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

                    if (lBall == null)
                    {
                        lBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        lBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
                        lBall.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        lBall.GetComponent<Renderer>().material.color = Color.yellow;
                        lBall.GetComponent<Collider>().enabled = false;
                        lBall.transform.SetParent(GorillaTagger.Instance.leftHandTransform);
                        lBall.transform.localPosition = Vector3.zero;
                        lBall.transform.rotation = Quaternion.identity;
                        lBall.SetActive(false);
                    }

                    if (rBall == null)
                    {
                        rBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        rBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
                        rBall.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        rBall.GetComponent<Renderer>().material.color = Color.magenta;
                        rBall.GetComponent<Collider>().enabled = false;
                        rBall.transform.SetParent(GorillaTagger.Instance.rightHandTransform);
                        rBall.transform.localPosition = Vector3.zero;
                        rBall.transform.rotation = Quaternion.identity;
                        rBall.SetActive(false);
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

            if (lBall != null)
            {
                Destroy(lBall);
            }

            if (rBall != null)
            {
                Destroy(rBall);
            }
        }

		public static float playerScale = 1f;
		private static bool yPress = false;
        private static float lastTime = 0f;
        private static bool toggled = true;
		public static bool osToggled = true;
        public static bool BALLS = false;

        private static GameObject lBall = null;
        private static GameObject rBall = null;

        public static float increment = 0.025f;
        public static float cooldown = 0.05f;
        public static float max = 25f;
        public static float least = 0.2f;
        public static float ballScale = 0.3f;
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

                    if (BALLS)
                    {
                        if (!lBall.activeSelf || !rBall.activeSelf)
                        {
                            lBall.SetActive(true);
                            rBall.SetActive(true);
                        }

                        lBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
                        rBall.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
                    }
                    else
                    {
                        if (lBall.activeSelf || rBall.activeSelf)
                        {
                            lBall.SetActive(false);
                            rBall.SetActive(false);
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
