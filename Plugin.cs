using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace rainbow_skin
{
	[BepInPlugin($"dev.cmax.{MyPluginInfo.PLUGIN_GUID}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	[BepInProcess("Super Battle Golf.exe")]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;

		private ConfigEntry<KeyboardShortcut> rainbowKeybind;
		private ConfigEntry<float> colorCycleInterval;

		private Scene scene => SceneManager.GetActiveScene();

		private bool _rainbowState;
		private int _currentColorIndex;
		private Coroutine _rainbowCoroutine;

		private void Awake()
		{

			rainbowKeybind = Config.Bind("General", "Rainbow Skin Toggle", new KeyboardShortcut(KeyCode.F9), "Keybind to toggle rainbow skin effect");
			colorCycleInterval = Config.Bind("General", "Color Cycle Interval", 0.5f, new ConfigDescription("Interval in seconds (0.01 - 5) for cycling rainbow colors", new AcceptableValueRange<float>(0.01f, 5f)));

			logger = Logger;
			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			Logger.LogMessage($"guid: {Info.Metadata.GUID}, name: {Info.Metadata.Name}, version: {Info.Metadata.Version}");
		}


		private void OnSceneUnloaded(Scene current)
		{
			// Stop coroutine when scene is unloaded to prevent errors
			if (_rainbowCoroutine != null)
				StopCoroutine(_rainbowCoroutine);
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.name == "Main menu")
				return;

			ManageRainbow();
		}

		private void ManageRainbow()
		{
			if (_rainbowState)
			{
				_rainbowCoroutine = StartCoroutine(RainbowCycle());
				Logger.LogInfo("Rainbow skin enabled");
			}
			else
			{
				if (_rainbowCoroutine != null){
					StopCoroutine(_rainbowCoroutine);
					Logger.LogInfo("Rainbow skin disabled");
				} else {
					Logger.LogWarning("Failed to disable rainbow skin: Coroutine not found");
				}
			}
		}

		private void Update()
		{
			if (rainbowKeybind.Value.IsDown())
			{
				_rainbowState = !_rainbowState;
				ManageRainbow();
			}
		}

		private IEnumerator RainbowCycle()
		{
			// Wait 1 second to ensure everything is loaded correctly
			yield return new WaitForSeconds(1f);

			while (true)
			{
				var localPlayer = GameManager.LocalPlayerInfo;
				if (localPlayer != null)
				{
					var switcher = localPlayer.Cosmetics?.Switcher;
					if (switcher != null)
					{
						int colorCount = switcher.cosmeticsSettings.skinColors?.Length ?? 0;
						if (colorCount > 0)
						{
							_currentColorIndex = (_currentColorIndex + 1) % colorCount;
							switcher.SetSkinColor(_currentColorIndex);
						}
					}
				}

				yield return new WaitForSeconds(colorCycleInterval.Value);
			}
		}
	}
}
