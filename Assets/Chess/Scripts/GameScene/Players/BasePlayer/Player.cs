using UnityEngine;
using JetBrains.Annotations;
using Chess.Scripts.GameScene.Tiles;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Chess.Scripts.GameScene.Players.BasePlayer {
    public enum PlayerType {
        White,
        Black
    }

    public abstract class Player : MonoBehaviour, IPunObservable {
        protected Tile CurrentTile { get; private set; }
        internal PlayerType PlayerType { get; private set; }

        protected virtual void Awake() {
            GetComponent<Collider2D>().enabled = photonView.isMine;
            PlayerType = gameObject.name.Contains("Black") ? PlayerType.Black : PlayerType.White;
        }

        #region Abstract functions

        internal abstract void PrintName();
        internal abstract void FindPossibleMove();

        #endregion


        #region Tile functions & calculations

        /// <summary>
        /// Updates current tile of player
        /// </summary>
        /// <param name="currentTile">Tile to set</param>
        internal void UpdateCurrentTile(Tile currentTile) {
            if (CurrentTile != null) {
                CurrentTile.OccupiedPlayer = null;
            }

            CurrentTile = currentTile;
            CurrentTile.OccupiedPlayer = this;
        }

        /// <summary>
        /// Finds recursive tiles in given direction.
        /// </summary>
        /// <param name="tileIndexX">Current tile X index</param>
        /// <param name="tileIndexY">Current tile Y index</param>
        /// <param name="directionX">Direction X index</param>
        /// <param name="directionY">Direction Y index</param>
        protected static void FindRecursiveTiles(int tileIndexX, int tileIndexY, int directionX, int directionY) {
            var tile = TilesHandler.GetTileByIndex(tileIndexX + directionX, tileIndexY + directionY);
            if (ValidateTile(tile)) {
                // ReSharper disable once TailRecursiveCall
                FindRecursiveTiles(tile.XIndex, tile.YIndex, directionX, directionY);
            }
        }

        /// <summary>
        /// Validates tiles
        /// </summary>
        /// <param name="tile">Tile to validate</param>
        /// <returns>Returns bool, whether user can go further.</returns>
        protected static bool ValidateTile(Tile tile) {
            if (tile == null) return false;
            if (tile.OccupiedPlayer == null) {
                tile.SetHighlighted();
                return true;
            }

            if (tile.OccupiedPlayer.PlayerType != GameHandler.Instance.GetOppositePlayerType()) return false;
            tile.SetHighlighted();
            return false; // Cannot go further.
        }

        #endregion


        #region Photon related methods.

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting) {
                var tileIndex = new Vector2(CurrentTile.XIndex, CurrentTile.YIndex);
                stream.SendNext(tileIndex);
            } else {
                var tileIndex = (Vector2) stream.ReceiveNext();
                if (CurrentTile != null)
                    CurrentTile.OccupiedPlayer = null;

                CurrentTile = TilesHandler.GetTileByIndex((int) tileIndex.x, (int) tileIndex.y);
                CurrentTile.OccupiedPlayer = this;
            }
        }

        /// <summary>
        /// Pun RPC function to set parent of player.
        /// </summary>
        [PunRPC]
        [UsedImplicitly]
        internal void SetParent() {
            const string whitePlayers = "WhitePlayers";
            const string blackPlayers = "BlackPlayers";
            transform.parent = PlayerType == PlayerType.White ? GameObject.Find(whitePlayers).transform : GameObject.Find(blackPlayers).transform;
            transform.rotation = PlayerType == PlayerType.White ? Quaternion.Euler(0f, 0f, photonView.isMine ? 0f : 180f) : Quaternion.Euler(0f, 0f, photonView.isMine ? 180f : 0f);
        }

        /// <summary>
        /// Pun RPC function to destroy player.
        /// </summary>
        [PunRPC]
        [UsedImplicitly]
        internal void DestroyPlayer() {
            if (!photonView.isMine) return;
            CurrentTile.OccupiedPlayer = null;
            if (gameObject.name.Contains("King")) {
                Debug.Log("You lose");
                GameHandler.Instance.NotifyGameOver(GameHandler.Instance.GetOppositePlayerType());
            }

            PhotonNetwork.Destroy(gameObject);
        }

        #endregion
    }
}