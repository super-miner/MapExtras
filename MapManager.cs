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


        public float startingCircleRadius = 0;
        public Color circleColor = new Color(0.96f, 0.62f, 0.11f, 0.5f);
        public float circleWidth = 35;
        
        public bool foundMapPartsContainer = false;
        public Transform mapPartsContainer = null;
        public Material mapContentMaterial = null;
        public int maskRectId = -1;
        public MapUI mapUI = null;
        public MapCircle circle = null;
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
			
            MapExtrasMod.Log("Successfully initialized the Map Manager object.");
        }

        void Update() {
            if (!foundMapPartsContainer) {
                Transform mapUITransform = Util.GetChildByPath(Manager.ui.transform.parent.parent, "Rendering/UI Camera/IngameUI/MapUI");

                mapUI = mapUITransform.GetComponent<MapUI>();
                
                Transform mapUIContainer = Util.GetChildByPath(mapUITransform, "container");
                Transform mapUIUserPositionOffset = Util.GetChildByPath(mapUIContainer, "miniMapPositionOffset/Zoom/userPositionOffset");
                mapPartsContainer = Util.GetChildByPath(mapUIUserPositionOffset, "playerPositionOffset/mapPartsContainer");
                Transform largeMapBorder = Util.GetChildByPath(mapUIContainer, "largeMapBorder");
                mapContentMaterial = Instantiate(mapUI.mapContentMaterial);
                maskRectId = Shader.PropertyToID("_MaskRect");

                circle = new MapCircle(mapPartsContainer, startingCircleRadius, circleWidth, circleColor);
                
                GameObject circleRadiusInputFieldGO = Instantiate(textFieldPrefab, largeMapBorder, true);
                circleRadiusInputField = circleRadiusInputFieldGO.GetComponent<CircleRadiusInputField>();
                circleRadiusInputField.transform.localScale = Vector3.one;
                circleRadiusInputField.transform.localPosition = new Vector3(7.5f, -7.25f, 0.0f);
                circleRadiusInputField.circle = circle;
                if (startingCircleRadius > 0) circleRadiusInputField.SetInputText("" + startingCircleRadius);
                
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
            }
        }

        void LoadConfig() {
            UpdateConfig();
            
            ConfigSystem.GetFloat("General", "CircleRadius", ref startingCircleRadius, startingCircleRadius);
            ConfigSystem.GetColor("General", "CircleColor", ref circleColor, circleColor);
            ConfigSystem.GetFloat("General", "CircleWidth", ref circleWidth, circleWidth);
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