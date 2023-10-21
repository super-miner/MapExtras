using System;
using UnityEngine;

namespace MapExtras {
    public class MapCircle {
        private const string IMAGE_PREFAB_PATH = "Assets/MapExtras/MapExtras_Circle.prefab";
        private GameObject imagePrefab = null;
        
        public GameObject gameObject;
        public SpriteRenderer spriteRenderer;
        public float radius = 0.0f;
        public float lineWidth = 0.0f;
        public Color color;
        
        public MapCircle(Transform parent, float radius, float lineWidth, Color color) {
            imagePrefab = MapExtrasMod.assetBundle.LoadAsset<GameObject>(IMAGE_PREFAB_PATH);
            gameObject = UnityEngine.Object.Instantiate(imagePrefab, parent, true);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = new Vector3(1.0f / 32.0f, 1.0f / 32.0f, 0.0f);

            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            
            this.lineWidth = lineWidth;
            this.color = color;
            
            SetRadius(radius);
        }

        public void SetRadius(float value) {
            radius = value;
            UpdateTexture();
        }

        public void UpdateTexture() {
            if (radius > 0) {
                spriteRenderer.enabled = true;
                
                Texture2D texture = GenerateCircleTexture();
                Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16.0f, 0U, 0);
            
                spriteRenderer.sprite = sprite;
            }
            else {
                spriteRenderer.enabled = false;
            }
        }
        
        private Texture2D GenerateCircleTexture() {
            float smallRadius = radius - lineWidth / 2;
            float largeRadius = radius + lineWidth / 2;

            if (largeRadius <= 0) {
                return null;
            }

            if (smallRadius < 0) {
                smallRadius = 0;
            }
            
            float sqrSmallRadius = smallRadius * smallRadius;
            float sqrLargeRadius = largeRadius * largeRadius;
            
            int textureSize = Mathf.CeilToInt((largeRadius + 100) * 2);
            Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
            texture.filterMode = FilterMode.Point;
    
            Color[] colors = new Color[texture.width * texture.height];
            Array.Fill(colors, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    
            int maxHeight = Mathf.CeilToInt(largeRadius * 0.7071f); // 0.7071 = cos(45 degrees)
    
            for (int y = 0; y <= maxHeight; y++) {
                int startX = y <= smallRadius ? Mathf.RoundToInt(Mathf.Sqrt(sqrSmallRadius - y * y)) : 0;
                int endX = Mathf.RoundToInt(Mathf.Sqrt(sqrLargeRadius - y * y));
    
                for (int x = startX; x <= endX; x++) {
                    SetMultiplePixels(ref colors, x, y, color, textureSize);
                }
            }
            
            texture.SetPixels(colors);
            texture.Apply();

            return texture;
        }
    
        private void SetMultiplePixels(ref Color[] colors, int x, int y, Color color, int textureSize) {
            SetPixel(ref colors, x, y, color, textureSize);
            SetPixel(ref colors, y, x, color, textureSize);
            SetPixel(ref colors, -y, x, color, textureSize);
            SetPixel(ref colors, -x, y, color, textureSize);
            SetPixel(ref colors, -x, -y, color, textureSize);
            SetPixel(ref colors, -y, -x, color, textureSize);
            SetPixel(ref colors, y, -x, color, textureSize);
            SetPixel(ref colors, x, -y, color, textureSize);
        }
        
        private void SetPixel(ref Color[] colors, int x, int y, Color color, int textureSize) {
            int realX = x + textureSize / 2;
            int realY = y + textureSize / 2;
    
            colors[realY * textureSize + realX] = color;
        }
    }
}