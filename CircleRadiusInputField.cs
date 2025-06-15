using UnityEngine;

namespace MapExtras {
    public class CircleRadiusInputField : RadicalMenuOptionTextInput {
        public Vector3 textBoxSize = Vector3.one;

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

            oldText = pugText.GetText();
        }

        protected override void Update() {
            base.Update();

            if (oldText != pugText.displayedTextString) {
                OnTextChange(oldText, ref pugText.displayedTextString);
                pugText.Render(true);
                oldText = pugText.displayedTextString;
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
            MapManager.instance.circleRadius = 0;
            
            if (string.IsNullOrEmpty(newText)) {
                return;
            }

            if (!int.TryParse(newText, out int newRadius)) {
                return;
            }

            MapManager.instance.circleRadius = newRadius;
        }
    }
}