using System;
using Chess.Scripts.GameScene.Players.BasePlayer;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chess.Scripts.GameScene.Players {
    public class PlayerTouchHandler : MonoBehaviour, IPointerClickHandler {
        private PlayerType _playerType;

        private void Awake() {
            _playerType = GetComponent<Player>().PlayerType;
        }

        public void OnPointerClick(PointerEventData eventData) {
            Debug.Log("touched.." + _playerType);
            if (_playerType != GameHandler.Instance.GetCurrentTurnPlayerType()) return;
            GameHandler.Instance.OnPlayerSelected(gameObject);
            TilesHandler.PrintTilesTable();
        }
    }
}