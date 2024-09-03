using System.Linq;
using PugMod;
using UnityEngine;

namespace MapExtras {
	public class MapExtrasMod : IMod {
		public static string MOD_VERSION = "1.0.2";
		public static string MOD_NAME = "Map Extras";
		public static string MOD_ID = "MapExtras";
		
		public static LoadedMod modInfo = null;
		public static AssetBundle assetBundle;
		
		private GameObject mapManager = null;
		
		public static void Log(object message) {
			Debug.Log(MOD_NAME + ": " + message.ToString());
		}
		
		public void EarlyInit() {
			modInfo = GetModInfo();

			if (modInfo != null) {
				assetBundle = modInfo.AssetBundles[0];
				MapExtrasMod.Log("Found mod info.");
			}
			else {
				MapExtrasMod.Log("Could not find mod info.");
			}
		}

		public void Init() {
			mapManager = new GameObject("MapExtras_MapManager", typeof(MapManager));
			
			MapExtrasMod.Log("Loaded " + MapExtrasMod.MOD_NAME + " version " + MapExtrasMod.MOD_VERSION + ".");
		}

		public void Shutdown() {
			MapExtrasMod.Log("Unloaded " + MapExtrasMod.MOD_NAME + " version " + MapExtrasMod.MOD_VERSION + ".");
		}

		public void ModObjectLoaded(UnityEngine.Object obj) {
			
		}

		public void Update() {
			
		}

		private LoadedMod GetModInfo() { // Code taken from Better Chat
			return API.ModLoader.LoadedMods.FirstOrDefault(modInfo => modInfo.Handlers.Contains(this));
		}
	}	
}
