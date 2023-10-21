using UnityEngine;

namespace MapExtras {
    public class CircleRadiusInputField : RadicalMenuOptionTextInput {
        public Vector3 textBoxSize = Vector3.one;
        [HideInInspector] public MapCircle circle = null;

        protected string oldText = "";
        
        protected override void Awake() {
            maxWidth = textBoxSize.x;
            
            base.Awake();

            float xOffset = 0.0f;
            if (pugText.style.horizontalAlignment == PugTextStyle.HorizontalAlignment.left) {
                xOffset = textBoxSize.x / 2.0f;
            }
            else if (pugText.style.horizontalAlignment == PugTextStyle.HorizontalAlignment.right) {
                xOffset = -textBoxSize.x / 2.0f;
            }
            float yOffset = 0.0f;
            if (pugText.style.verticalAlignment == PugTextStyle.VerticalAlignment.bottom) {
                yOffset = textBoxSize.x / 2.0f;
            }
            else if (pugText.style.verticalAlignment == PugTextStyle.VerticalAlignment.top) {
                yOffset = -textBoxSize.x / 2.0f;
            }
            
            clickCollider.center = new Vector3(xOffset, yOffset, 0.0f);
            clickCollider.size = textBoxSize;

            oldText = pugText.textString;
        }

        protected override void Update() {
            base.Update();

            if (oldText != pugText.textString) {
                OnTextChange(oldText, ref pugText.textString);
                pugText.Render(true);
                oldText = pugText.textString;
            }

            if (MapManager.instance.mapUI.isShowingBigMap) {
                if (Manager.input.singleplayerInputModule.rewiredPlayer.GetButtonDown((int)PlayerInput.InputType.TOGGLE_MAP)
                    || Manager.input.singleplayerInputModule.rewiredPlayer.GetButtonDown((int)PlayerInput.InputType.TOGGLE_INVENTORY)
                    || Manager.input.singleplayerInputModule.rewiredPlayer.GetButtonDown((int)PlayerInput.InputType.CANCEL)
                    ) {
                    OnDeselected();
                    MapManager.instance.mapUI.ToggleMap();
                }
            }
        }

        protected virtual void OnTextChange(string oldText, ref string newText) {
            if (pugText.textString != "") {
                int newRadius = int.Parse(newText);
                
                if (newRadius <= 4096) {
                    circle.SetRadius(newRadius);
                }
                else {
                    newText = "4096";
                    circle.SetRadius(4096);
                }
            }
            else {
                circle.SetRadius(0);
            }
        }
    }
}