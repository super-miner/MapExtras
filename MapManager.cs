using MapExtras.Common;
using PugMod;
using UnityEngine;

namespace MapExtras {
    public class MapManager : MonoBehaviour {
        public static MapManager instance = null;

        public const int CONFIG_VERSION = 1;
        
        private const string TEXT_FIELD_PREFAB_PATH = "Assets/MapExtras/MapExtras_CircleRadiusInputField.prefab";
        private GameObject textFieldPrefab = null;
        private const string PLAYER_MAP_MARKER_PREFAB_PATH = "Assets/MapExtras/MapExtras_PlayerMapMarker.prefab";
        public GameObject playerMapMarkerPrefab = null;
        private const string LARGE_MAP_OVERLAY_PREFAB_PATH = "Assets/MapExtras/MapExtras_LargeMapOverlay.prefab";
        public GameObject largeMapOverlayPrefab = null;
        private const string MINI_MAP_OVERLAY_PREFAB_PATH = "Assets/MapExtras/MapExtras_MiniMapOverlay.prefab";
        public GameObject miniMapOverlayPrefab = null;

        public float circleRadius = 0;
        public Color circleColor = new Color(0.96f, 0.62f, 0.11f, 0.5f);
        public float circleThickness = 30;
        
        public bool foundMapPartsContainer = false;
        public Transform mapPartsContainer = null;
        public Material mapContentMaterial = null;
        public int maskRectId = -1;
        public MapUI mapUI = null;
        public Transform mapZoom = null;
        public Transform mapUIUserPositionOffset = null;
        public Transform mapUIPlayerPositionOffset = null;
        public SpriteRenderer largeMapOverlay;
        public SpriteRenderer miniMapOverlay;
        public CircleRadiusInputField circleRadiusInputField = null;
        public PugText testMarkerText = null;
        public MapMarkerUIElement mapMarkerTest = null;
        
        void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(transform.gameObject);
                return;
            }
			
            DontDestroyOnLoad(transform.gameObject);
            
            LoadConfig();
            
            textFieldPrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(TEXT_FIELD_PREFAB_PATH);
            playerMapMarkerPrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(PLAYER_MAP_MARKER_PREFAB_PATH);
            largeMapOverlayPrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(LARGE_MAP_OVERLAY_PREFAB_PATH);
            miniMapOverlayPrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(MINI_MAP_OVERLAY_PREFAB_PATH);
			
            MapExtrasMod.Log("Successfully initialized the Map Manager object.");
        }

        void Update() {
            if (!foundMapPartsContainer) {
                Transform mapUITransform = Util.GetChildByPath(Manager.ui.transform.parent.parent, "Rendering/UI Camera/IngameUI/MapUI");

                mapUI = mapUITransform.GetComponent<MapUI>();
                
                Transform mapUIContainer = Util.GetChildByPath(mapUITransform, "container");
                mapZoom = Util.GetChildByPath(mapUIContainer, "miniMapPositionOffset/Zoom");
                mapUIUserPositionOffset = Util.GetChildByPath(mapZoom, "userPositionOffset");
                mapUIPlayerPositionOffset = Util.GetChildByPath(mapUIUserPositionOffset, "playerPositionOffset");
                mapPartsContainer = Util.GetChildByPath(mapUIPlayerPositionOffset, "mapPartsContainer");
                Transform largeMapBorder = Util.GetChildByPath(mapUIContainer, "largeMapBorder");
                Transform miniMapBorder = Util.GetChildByPath(mapUIContainer, "miniMapBorder");
                mapContentMaterial = Instantiate(mapUI.mapContentMaterial);
                maskRectId = Shader.PropertyToID("_MaskRect");
                
                GameObject circleRadiusInputFieldGO = Instantiate(textFieldPrefab, largeMapBorder, true);
                circleRadiusInputField = circleRadiusInputFieldGO.GetComponent<CircleRadiusInputField>();
                circleRadiusInputField.transform.localScale = Vector3.one;
                circleRadiusInputField.transform.localPosition = new Vector3(7.5f, -7.25f, 0.0f);
                //circleRadiusInputField.circle = circle;
                if (circleRadius > 0) circleRadiusInputField.SetInputText("" + circleRadius);
                
                GameObject largeMapOverlayGO = Instantiate(largeMapOverlayPrefab, largeMapBorder);
                largeMapOverlayGO.transform.localScale = new Vector3(24.1875f, 12.1875f, 1.0f);
                largeMapOverlayGO.transform.localPosition = new Vector3(0.0f, 0.0625f, 0.0f);
                largeMapOverlay = largeMapOverlayGO.GetComponent<SpriteRenderer>();
                
                GameObject miniMapOverlayGO = Instantiate(miniMapOverlayPrefab, miniMapBorder);
                miniMapOverlayGO.transform.localScale = new Vector3(3.75f, 2.0f, 1.0f);
                miniMapOverlay = miniMapOverlayGO.GetComponent<SpriteRenderer>();
                
                //PlayerMapMarker.CreatePlayerMapMarkers(mapUIUserPositionOffset);
                
                MapExtrasMod.Log("Found map parts container.");
                foundMapPartsContainer = true;
            }

            if (mapPartsContainer != null) {
                //PlayerMapMarker.CreatePlayerMapMarkers(mapPartsContainer);
            }
        }

        void LateUpdate() {
            if (foundMapPartsContainer) {
                Vector4 maskRect = GetMaskRect(mapUI.isShowingBigMap ? mapUI.largeMapBackground.bounds : mapUI.miniMapBackground.bounds);
                mapContentMaterial.SetVector(maskRectId, new Vector4(maskRect.x, maskRect.y, 1f / maskRect.z, 1f / maskRect.w));
                
                Vector2 largeMapOffset = mapUIUserPositionOffset.localPosition + mapUIPlayerPositionOffset.localPosition;
                float pixelScale = mapZoom.transform.localScale.x;
                
                largeMapOverlay.material.SetVector("_Offset", largeMapOffset * 16.0f);
                largeMapOverlay.material.SetVector("_WorldSize", largeMapOverlay.transform.localScale * 16.0f / pixelScale);
                largeMapOverlay.material.SetFloat("_PixelScale", pixelScale);
                largeMapOverlay.material.SetFloat("_CircleRadius", circleRadius);
                largeMapOverlay.material.SetFloat("_CircleThickness", circleThickness);
                
                Vector2 miniMapOffset = mapUIPlayerPositionOffset.localPosition;
                
                miniMapOverlay.material.SetVector("_Offset", miniMapOffset * 16.0f);
                miniMapOverlay.material.SetVector("_WorldSize", miniMapOverlay.transform.localScale * 16.0f);
                miniMapOverlay.material.SetFloat("_PixelScale", 1);
                miniMapOverlay.material.SetFloat("_CircleRadius", circleRadius);
                miniMapOverlay.material.SetFloat("_CircleThickness", circleThickness);
            }
        }

        void LoadConfig() {
            UpdateConfig();
            
            ConfigSystem.GetFloat("General", "CircleRadius", ref circleRadius, circleRadius);
            ConfigSystem.GetColor("General", "CircleColor", ref circleColor, circleColor);
            ConfigSystem.GetFloat("General", "CircleWidth", ref circleThickness, circleThickness);
        }

        void UpdateConfig() {
            int configVersion = -1;
            ConfigSystem.GetInt("ConfigVersion", "DoNotEdit", ref configVersion, CONFIG_VERSION);
        }
        
        Vector4 GetMaskRect(Bounds bounds) {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            return new Vector4(min.x, min.y, max.x - min.x, max.y - min.y);
        }
    }
}