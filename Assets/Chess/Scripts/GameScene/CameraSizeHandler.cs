using UnityEngine;

namespace Chess.Scripts.GameScene {
    /// <summary>
    /// Camera size handler script to ensure the chess board and players are not cropped in any aspect ratio.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class CameraSizeHandler : MonoBehaviour {
        private const float HorizontalPadding = 0.5f;
        
        private void Start() {
            if (Camera.main != null)
                Camera.main.orthographicSize =
                    (GetComponent<SpriteRenderer>().bounds.size.x + HorizontalPadding) * Screen.height / Screen.width * 0.5f;
        }
    }
}