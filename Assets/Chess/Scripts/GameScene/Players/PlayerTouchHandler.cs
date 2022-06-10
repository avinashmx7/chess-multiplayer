using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess.Scripts.GameScene.Players {
    public class PlayerTouchHandler : MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick(PointerEventData eventData) {
            GameHandler.Instance.OnPlayerSelected(gameObject);
        }
    }
}