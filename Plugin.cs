using BepInEx;
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

		private int _currentColorIndex;
		private Coroutine _rainbowCoroutine;
		private const float COLOR_CYCLE_INTERVAL = 0.5f;

		private void Awake()
		{
			logger = Logger;
			SceneManager.sceneLoaded += OnSceneLoaded;
			Logger.LogMessage($"guid: {Info.Metadata.GUID}, name: {Info.Metadata.Name}, version: {Info.Metadata.Version}");
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			// Stop coroutine from previous scene
			if (_rainbowCoroutine != null)
				StopCoroutine(_rainbowCoroutine);

			if (scene.name == "Main menu")
				return;

			_rainbowCoroutine = StartCoroutine(RainbowCycle());
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

				yield return new WaitForSeconds(COLOR_CYCLE_INTERVAL);
			}
		}
	}
}
