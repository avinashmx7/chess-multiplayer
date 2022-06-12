using System;
using UnityEngine;
using Chess.Scripts.GameScene.Tiles;
using JetBrains.Annotations;
using MonoBehaviour = Photon.MonoBehaviour;

namespace Chess.Scripts.GameScene.Players.BasePlayer {
    public enum PlayerType {
        White,
        Black
    }

    public abstract class Player : MonoBehaviour, IPunObservable {
        protected Tile CurrentTile { get; private set; }
        internal PlayerType PlayerType { get; private set; }
        internal abstract void PrintName();
        internal abstract void FindPossibleMove();
        private const string Black = "Black";

        protected virtual void Awake() {
            GetComponent<Collider2D>().enabled = photonView.isMine;
            PlayerType = gameObject.name.Contains(Black) ? PlayerType.Black : PlayerType.White;
        }

        internal void UpdateCurrentTile(Tile currentTile) {
            if (CurrentTile != null) {
                CurrentTile.OccupiedPlayer = null;
            }

            CurrentTile = currentTile;
            CurrentTile.OccupiedPlayer = this;
        }

        protected static void FindRecursiveTiles(int tileIndexX, int tileIndexY, int directionX, int directionY) {
            var tile = TilesHandler.GetTileByIndex(tileIndexX + directionX, tileIndexY + directionY);
            if (IsPossibleTile(tile)) {
                // ReSharper disable once TailRecursiveCall
                FindRecursiveTiles(tile.XIndex, tile.YIndex, directionX, directionY);
            }
        }

        protected static bool IsPossibleTile(Tile tile) {
            if (tile == null) return false;
            if (tile.OccupiedPlayer == null) {
                tile.SetHighlighted();
                return true;
            }

            if (tile.OccupiedPlayer.PlayerType != GameHandler.Instance.GetOppositePlayerType()) return false;
            tile.SetHighlighted();
            return false; // Cannot go further.
        }

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

        [PunRPC]
        [UsedImplicitly]
        internal void SetParent() {
            const string whitePlayers = "WhitePlayers";
            const string blackPlayers = "BlackPlayers";
            transform.parent = PlayerType == PlayerType.White ? GameObject.Find(whitePlayers).transform : GameObject.Find(blackPlayers).transform;

            transform.rotation = PlayerType == PlayerType.White ? Quaternion.Euler(0f, 0f, photonView.isMine ? 0f : 180f) : Quaternion.Euler(0f, 0f, photonView.isMine ? 180f : 0f);
        }

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