using Chess.Scripts.GameScene.Players.Interfaces;
using Chess.Scripts.GameScene.Tiles;
using DG.Tweening;
using UnityEngine;

namespace Chess.Scripts.GameScene {
    public class GameHandler : MonoBehaviour {
        internal static GameHandler Instance { get; private set; }
        private GameObject _currentSelectedPlayer;

        private void Awake() {
            Instance = this;
        }

        private void Start() { }

        public void OnTileSelected(Tile tile) {
            TilesHandler.DeselectAllTiles();
            _currentSelectedPlayer.transform.DOMove(tile.GameObj.transform.position, GetDuration(_currentSelectedPlayer.transform, tile.Transform))
                .OnComplete(() => {
                    var currentPlayer = _currentSelectedPlayer.GetComponent<Player>();
                    currentPlayer.UpdateCurrentTile(tile);
                });
        }

        public void OnPlayerSelected(GameObject playerGameObj) {
            _currentSelectedPlayer = playerGameObj;
            TilesHandler.DeselectAllTiles();
            var iPlayer = playerGameObj.GetComponent<Player>();
            iPlayer.PrintName();
            iPlayer.GetPossibleTiles();
        }

        private void OnDestroy() {
            Instance = null;
        }

        private float GetDuration(Transform trans1, Transform trans2) {
            return Vector3.Distance(trans1.position, trans2.position);
        }
    }
}