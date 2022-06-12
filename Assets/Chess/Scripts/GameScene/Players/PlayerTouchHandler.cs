using UnityEngine;
using UnityEngine.EventSystems;
using Chess.Scripts.GameScene.Tiles;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene.Players {
    public class PlayerTouchHandler : MonoBehaviour, IPointerClickHandler {
        private PlayerType _playerType;

        private void Awake() {
            _playerType = GetComponent<Player>().PlayerType;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (_playerType != GameHandler.Instance.GetCurrentTurnPlayerType()) return;
            GameHandler.Instance.OnPlayerSelected(gameObject);
            TilesHandler.PrintTilesTable();
        }
    }
}