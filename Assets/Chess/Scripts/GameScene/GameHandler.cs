using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Chess.Scripts.GameScene.Tiles;
using Chess.Scripts.GameScene.Players;
using MonoBehaviour = Photon.MonoBehaviour;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene {
    public class GameHandler : MonoBehaviour, IPunObservable {
        #region Variables

        [SerializeField] private TextMeshProUGUI gameStatus;
        [SerializeField] private GameObject touchBlockPanel, gameOverPanel;
        internal static GameHandler Instance { get; private set; }

        private Camera _mainCamera;
        private GameObject _currentSelectedPlayer;
        private PlayerType _clientPlayerType, _currentTurnPlayer = PlayerType.White;

        private const float MaxAnimDuration = 1.5f;
        private const string YourTurn = "<color=\"green\">Your Turn</color>";
        private const string OpponentsTurn = "<color=\"yellow\">Opponents Turn</color>";
        private const string YouWin = "<color=\"green\">You Win!</color>";
        private const string YouLose = "<color=\"red\">You Lose!</color>";

        #endregion

        private void Awake() {
            Instance = this;
            _mainCamera = Camera.main;
            gameOverPanel.SetActive(false);
        }

        #region Player type getter & setter functions

        /// <summary>
        /// Sets the client player type.
        /// </summary>
        /// <param name="playerType">Player type</param>
        internal void SetClientPlayerType(PlayerType playerType) {
            _clientPlayerType = playerType;
            if (_clientPlayerType == PlayerType.Black) {
                //Rotating camera for black player for ease fo use.
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

        #endregion


        #region Player actions & movements

        /// <summary>
        /// Function is called when a tile is selection, the players is moved to the provided tile using tween.
        /// Kills the opponent player if the selected tile is already occupied.
        /// Switches the player turn after completion of tween.
        /// </summary>
        /// <param name="tile">Selected tile.</param>
        public void OnTileSelected(Tile tile) {
            touchBlockPanel.SetActive(true);
            TilesHandler.DeselectAllTiles();
            if (tile.OccupiedPlayer != null) {
                //Kill opponent player
                Debug.Log("Kill opponent = " + tile.OccupiedPlayer);
                tile.OccupiedPlayer.GetComponent<Player>().photonView.RPC("DestroyPlayer", PhotonTargets.All);
            }

            _currentSelectedPlayer.transform.DOMove(tile.GameObj.transform.position, GetAnimationDuration(_currentSelectedPlayer.transform, tile.Transform)).OnComplete(() => {
                var currentPlayer = _currentSelectedPlayer.GetComponent<Player>();
                currentPlayer.UpdateCurrentTile(tile);

                if (currentPlayer.TryGetComponent<PawnHandler>(out var pawnHandler))
                    pawnHandler.IsFirstTime = false;

                photonView.RPC(nameof(ChangePlayerTurn), PhotonTargets.All);
            });
        }

        /// <summary>
        /// Function is called whenever a player is selected.
        /// Deselects any previous tiles.
        /// </summary>
        /// <param name="playerGameObj">Selected player game object</param>
        public void OnPlayerSelected(GameObject playerGameObj) {
            _currentSelectedPlayer = playerGameObj;
            TilesHandler.DeselectAllTiles();
            var iPlayer = playerGameObj.GetComponent<Player>();
            iPlayer.PrintName();
            iPlayer.FindPossibleMove();
        }

        /// <summary>
        /// Return the animation duration for two positions.
        /// </summary>
        /// <param name="trans1">Origin transform</param>
        /// <param name="trans2">Destination transform</param>
        /// <returns></returns>
        private static float GetAnimationDuration(Transform trans1, Transform trans2) {
            var estimatedTime = Vector3.Distance(trans1.position, trans2.position) / 2f;
            return estimatedTime < MaxAnimDuration ? estimatedTime : MaxAnimDuration;
        }

        #endregion


        #region Game state functions

        internal PlayerType GetCurrentTurnPlayerType() {
            return _currentTurnPlayer;
        }

        private void UpdateGameStatus() {
            var isCurrentPlayersTurn = _currentTurnPlayer == GetClientPlayerType();
            gameStatus.text = isCurrentPlayersTurn ? YourTurn : OpponentsTurn;
            touchBlockPanel.SetActive(!isCurrentPlayersTurn); //enables touch for active player
        }

        /// <summary>
        /// Invokes the PUN function when the game is over.
        /// </summary>
        /// <param name="winnerPlayerType">Winner player type</param>
        internal void NotifyGameOver(PlayerType winnerPlayerType) {
            photonView.RPC(nameof(GameOver), PhotonTargets.All, winnerPlayerType);
        }

        #endregion

        #region Photon & PUN functions

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }


        [PunRPC]
        private void ChangePlayerTurn() {
            _currentTurnPlayer = _currentTurnPlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
            UpdateGameStatus();
        }

        /// <summary>
        /// Game over PUN function.
        /// Enables the game over panel.
        /// Updates game text - win or lose.
        /// Adds listener to exit button.
        /// </summary>
        /// <param name="winnerPlayerTypeIndex"></param>
        [PunRPC]
        private void GameOver(PlayerType winnerPlayerTypeIndex) {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.Find("WinStatusText").GetComponent<TextMeshProUGUI>().text = _clientPlayerType == winnerPlayerTypeIndex ? YouWin : YouLose;
            gameOverPanel.transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(() => {
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(0);
            });
        }

        #endregion


        private void OnDestroy() {
            Instance = null;
        }

        private void OnApplicationQuit() {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
    }
}