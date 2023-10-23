using PugMod;
using UnityEngine;

namespace MapExtras {
    public class PlayerMapMarker : MonoBehaviour {
        [HideInInspector] public MapMarkerUIElement mapMarker;
        public PugText nameText;
        
        public static void CreatePlayerMapMarkers(Transform parent) {
            for (int i = 0; i < parent.childCount; i++) {
                Transform child = parent.GetChild(i);

                if (child.childCount > 0) {
                    Transform mapMarkerIconTransform = child.GetChild(0);
                    MapMarkerUIElement mapMarkerIcon = mapMarkerIconTransform.GetComponent<MapMarkerUIElement>();

                    if (mapMarkerIcon != null && mapMarkerIcon.markerType == MapMarkerType.Player) {
                        CreatePlayerMapMarker(mapMarkerIcon);
                    }
                }
            }
        }
        
        public static void CreatePlayerMapMarker(MapMarkerUIElement mapMarker) {
            GameObject playerMapMarkerGO = UnityEngine.Object.Instantiate(MapManager.instance.playerMapMarkerPrefab, mapMarker.transform, true);
            playerMapMarkerGO.transform.localScale = Vector3.one;
            playerMapMarkerGO.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);

            PlayerMapMarker playerMapMarker = playerMapMarkerGO.GetComponent<PlayerMapMarker>();
            playerMapMarker.mapMarker = mapMarker;
        }

        void Update() {
            if (mapMarker.player != null) {
                nameText.Render(mapMarker.player.activeCustomization.name.ToString(), false);
            }
            else if (Manager.main.player != null) {
                nameText.Render(Manager.main.player.activeCustomization.name.ToString(), false);
            }
        }
    }
}