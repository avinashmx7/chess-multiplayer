using System;
using Chess.Scripts.GameScene.Players;
using Chess.Scripts.GameScene.Players.BasePlayer;
using Chess.Scripts.GameScene.Tiles;
using DG.Tweening;
using TMPro;
using UnityEngine;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Chess.Scripts.GameScene {
    public class GameHandler : MonoBehaviour, IPunObservable {
        [SerializeField] private TextMeshProUGUI gameStatus;
        [SerializeField] private GameObject touchBlockPanel;
        internal static GameHandler Instance { get; private set; }

        private GameObject _currentSelectedPlayer;
        private PlayerType _clientPlayerType, _currentTurnPlayer = PlayerType.White;

        private const string YourTurn = "Your Turn";
        private const string OpponentsTurn = "Opponents Turn";
        private const float MaxAnimDuration = 1.5f;

        private void Awake() {
            Instance = this;
        }

        internal void SetClientPlayerType(PlayerType playerType) {
            _clientPlayerType = playerType;
            UpdateGameStatus();
        }

        private PlayerType GetClientPlayerType() {
            return _clientPlayerType;
        }

        internal PlayerType GetOppositePlayerType() {
            return _clientPlayerType == PlayerType.White ? PlayerType.Black : PlayerType.White;
        }

        #region Player actions & movements

        public void OnTileSelected(Tile tile) {
            touchBlockPanel.SetActive(true);
            TilesHandler.DeselectAllTiles();
            if (tile.OccupiedPlayer != null) {
                //Kill opponent player
                Debug.Log("Kill opponent = " + tile.OccupiedPlayer);
                tile.OccupiedPlayer.GetComponent<Player>().photonView.RPC("DestroyPlayer", PhotonTargets.All);
            }

            _currentSelectedPlayer.transform.DOMove(tile.GameObj.transform.position, GetDuration(_currentSelectedPlayer.transform, tile.Transform)).OnComplete(() => {
                var currentPlayer = _currentSelectedPlayer.GetComponent<Player>();
                currentPlayer.UpdateCurrentTile(tile);

                if (currentPlayer.TryGetComponent<PawnHandler>(out var pawnHandler))
                    pawnHandler.IsFirstTime = false;

                photonView.RPC(nameof(ChangePlayerTurn), PhotonTargets.All);
            });
        }

        public void OnPlayerSelected(GameObject playerGameObj) {
            _currentSelectedPlayer = playerGameObj;
            TilesHandler.DeselectAllTiles();
            var iPlayer = playerGameObj.GetComponent<Player>();
            iPlayer.PrintName();
            iPlayer.FindPossibleMove();
        }

        private static float GetDuration(Transform trans1, Transform trans2) {
            var estimatedTime = Vector3.Distance(trans1.position, trans2.position) / 2f;
            return estimatedTime < MaxAnimDuration ? estimatedTime : MaxAnimDuration;
        }

        #endregion

        [PunRPC]
        private void ChangePlayerTurn() {
            _currentTurnPlayer = _currentTurnPlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
            UpdateGameStatus();
        }

        internal PlayerType GetCurrentTurnPlayerType() {
            return _currentTurnPlayer;
        }

        private void UpdateGameStatus() {
            var isCurrentPlayersTurn = _currentTurnPlayer == GetClientPlayerType();
            gameStatus.text = isCurrentPlayersTurn ? YourTurn : OpponentsTurn;
            touchBlockPanel.SetActive(!isCurrentPlayersTurn); //enables touch for active player
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

        private void OnDestroy() {
            Instance = null;
        }
    }
}