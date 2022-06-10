using Chess.Scripts.GameScene.Tiles;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players.Interfaces {
    public enum PlayerType {
        White,
        Black
    }

    public abstract class Player : MonoBehaviour {
        protected Tile CurrentTile { get; private set; }
        protected PlayerType CurrentPlayerType { get; private set; }
        internal abstract void PrintName();
        internal abstract void GetPossibleTiles();

        internal void SetPlayerType(PlayerType playerType) {
            CurrentPlayerType = playerType;
        }

        internal PlayerType GetPlayerType() {
            return CurrentPlayerType;
        }

        internal void UpdateCurrentTile(Tile currentTile) {
            if (CurrentTile != null) {
                CurrentTile.OccupiedPlayer = null;
            }

            CurrentTile = currentTile;
            CurrentTile.OccupiedPlayer = this;
        }
    }
}