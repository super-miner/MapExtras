using UnityEngine;

namespace MapExtras {
    public class MapManager : MonoBehaviour {
        public static MapManager instance = null;

        private const string TEXT_FIELD_PREFAB_PATH = "Assets/MapExtras/MapExtras_CircleRadiusInputField.prefab";
        private GameObject textFieldPrefab = null;
        
        public bool foundMapPartsContainer = false;
        public MapUI mapUI = null;
        public MapCircle circle = null;
        public CircleRadiusInputField circleRadiusInputField = null;
        
        void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(transform.gameObject);
                return;
            }
			
            DontDestroyOnLoad(transform.gameObject);
            
            textFieldPrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(TEXT_FIELD_PREFAB_PATH);
			
            MapExtrasMod.Log("Successfully initialized the Map Manager object.");
        }

        void Update() {
            if (!foundMapPartsContainer) {
                Transform mapUITransform = FindMapUI();

                mapUI = mapUITransform.GetComponent<MapUI>();
                
                Transform mapUIContainer = FindMapUIContainer(mapUITransform);
                Transform mapPartsContainer = FindMapPartsContainer(mapUIContainer);
                Transform largeMapBorder = FindLargeMapBorder(mapUIContainer);

                circle = new MapCircle(mapPartsContainer, 0, 35, new Color(0.96f, 0.62f, 0.11f, 0.5f));
                
                GameObject circleRadiusInputFieldGO = UnityEngine.Object.Instantiate(textFieldPrefab, largeMapBorder, true);
                circleRadiusInputField = circleRadiusInputFieldGO.GetComponent<CircleRadiusInputField>();
                circleRadiusInputField.transform.localScale = Vector3.one;
                circleRadiusInputField.transform.localPosition = new Vector3(7.5f, -7.25f, 0.0f);
                circleRadiusInputField.circle = circle;
                
                MapExtrasMod.Log("Found map parts container.");
                foundMapPartsContainer = true;
            }
            
            /*BoxCollider boxCollider = inputField.GetComponent<BoxCollider>();

            if (boxCollider != null) {
                boxCollider.center = new Vector3(2.25f, 0.0f, 0.0f);
                boxCollider.size = new Vector3(4.5f, 1.0f, 1.0f);
            }*/
        }

        Transform FindMapUI() {
            if (!Manager.ui) {
                MapExtrasMod.Log("Could not find UI Manager.");
                return null;
            }
            
            Transform renderingParent = Manager.ui.transform.parent.parent.GetChild(2); // GlobalObjects (Main Manager)(Clone)/Rendering

            Transform uiCamera = renderingParent.transform.GetChild(1);

            Transform ingameUI = uiCamera.GetChild(0);

            return ingameUI.GetChild(13);
        }

        Transform FindMapUIContainer(Transform mapUI) {
            return mapUI.GetChild(0);
        }

        Transform FindMapPartsContainer(Transform mapUIContainer) { // Global Objects (Main Manager)(Clone)/Rendering/UI Camera/IngameUI/MapUI/container/miniMapPositionOffset/Zoom/userPositionOffset/playerPositionOffset/mapPartsContainer/
            Transform mapUIMinimapPositionOffset = mapUIContainer.GetChild(0);
            
            Transform mapUIZoomOffset = mapUIMinimapPositionOffset.GetChild(0);
            
            Transform mapUIUserPositionOffset = mapUIZoomOffset.GetChild(0);
            
            Transform mapUIPlayerPositionOffset = mapUIUserPositionOffset.GetChild(1);

            return mapUIPlayerPositionOffset.GetChild(0);
        }

        Transform FindLargeMapBorder(Transform mapUIContainer) {
            return mapUIContainer.GetChild(1);
        }
        
        GameObject FindTextPrefab(Transform ingameUI) { // TODO: Figure out how to create a prefab for this instead of using existing text.
            Transform playerHealthBar = ingameUI.GetChild(0);

            Transform playerHealthContainer = playerHealthBar.GetChild(0);

            Transform playerHealthTextContainer = playerHealthContainer.GetChild(6);

            Transform healthTextNumber = playerHealthTextContainer.GetChild(1);

            return healthTextNumber.gameObject;
        }
    }
}