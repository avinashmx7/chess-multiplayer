using TMPro;
using DG.Tweening;
using UnityEngine;
using Chess.Scripts.GameScene.Tiles;
using Button = UnityEngine.UI.Button;
using Chess.Scripts.GameScene.Players;
using MonoBehaviour = Photon.MonoBehaviour;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene {
    public class GameHandler : MonoBehaviour, IPunObservable {
        [SerializeField] private TextMeshProUGUI gameStatus;
        [SerializeField] private GameObject touchBlockPanel, gameOverPanel;
        internal static GameHandler Instance { get; private set; }

        private Camera _mainCamera;
        private GameObject _currentSelectedPlayer;
        private PlayerType _clientPlayerType, _currentTurnPlayer = PlayerType.White;

        private const string YourTurn = "<color=\"green\">Your Turn</color>";
        private const string OpponentsTurn = "<color=\"yellow\">Opponents Turn</color>";
        private const string YouWin = "<color=\"green\">You Win!</color>";
        private const string YouLose = "<color=\"red\">You Lose!</color>";
        private const float MaxAnimDuration = 1.5f;

        private void Awake() {
            Instance = this;
            _mainCamera = Camera.main;
            gameOverPanel.SetActive(false);
        }

        internal void SetClientPlayerType(PlayerType playerType) {
            _clientPlayerType = playerType;
            if (_clientPlayerType == PlayerType.Black) {
                _mainCamera.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }

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

        internal void NotifyGameOver(PlayerType winnerPlayerType) {
            photonView.RPC(nameof(GameOver), PhotonTargets.All, winnerPlayerType);
        }

        [PunRPC]
        private void ChangePlayerTurn() {
            _currentTurnPlayer = _currentTurnPlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
            UpdateGameStatus();
        }

        [PunRPC]
        private void GameOver(PlayerType winnerPlayerTypeIndex) {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.Find("WinStatusText").GetComponent<TextMeshProUGUI>().text = _clientPlayerType == winnerPlayerTypeIndex ? YouWin : YouLose;
            gameOverPanel.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(() => {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(0);
            });
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

        private void OnApplicationQuit() {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
    }
}