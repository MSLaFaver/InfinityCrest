using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using InfinityCrest.Patches;
using InfinityCrest.Patches.Localization;
using UnityEngine.SceneManagement;
using Needleforge.Data;
using Needleforge;

namespace InfinityCrest
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class InfinityCrest : BaseUnityPlugin
	{
		public static ManualLogSource logger;
		public static Harmony harmony;
		public static CrestData infinityCrest;

		private void Awake()
		{			
			logger = Logger;
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} has loaded!");
			harmony = new("com.mslafaver.infinitycrest");
			harmony.PatchAll(typeof(AlterLayering));

			infinityCrest = NeedleforgePlugin.AddCrest("INFINITY", null, null);

			infinityCrest.BindEvent += (healValue, healAmount, healTime, playMaker) =>
			{
				healValue.Value = 2;
				healAmount.Value = 1;
				healTime.Value = 0.6f;
			};

			var toolWidth = 1.4f;
			var toolLeft = -2.9f;
			var toolTop = 3.35f;

			for (var i = 0; i < 6; i++)
            {
				for (var j = 0; j < 5; j++)
                {
					infinityCrest.slots.Add(new()
					{
						AttackBinding = AttackToolBinding.Neutral,
						Type = i >= 4 ? ToolItemType.Yellow : ToolItemType.Blue,
						Position = new(toolLeft + toolWidth * j, toolTop - toolWidth * i),
						IsLocked = false,
						NavUpIndex = -1,
						NavUpFallbackIndex = -1,
						NavRightIndex = (j == 4) ? -1 : i * 5 + j + 1,
						NavRightFallbackIndex = -1,
						NavLeftIndex = (j == 0) ? -1 : i * 5 + j -1,
						NavLeftFallbackIndex = -1,
						NavDownIndex = (i == 5) ? -1 : (i + 1) * 5 + j,
						NavDownFallbackIndex = -1,
					});
                }
            }

			SceneManager.activeSceneChanged += OnSceneChanged;
		}

		private void OnSceneChanged(Scene prev, Scene next)
		{
			if (next.name != "Menu_Title")
			{
				harmony.PatchAll(typeof(Localization));
			}
		}
	}
}