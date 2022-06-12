using UnityEngine;
using Chess.Scripts.GameScene.Tiles;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene.Players {
    public class PawnHandler : Player {
        internal bool IsFirstTime { set; private get; }

        protected override void Awake() {
            base.Awake();
            IsFirstTime = true;
        }

        //For debugging.
        internal override void PrintName() {
            Debug.Log($"I'm {PlayerType} Pawn!");
        }

        /// <summary>
        /// Find possible move for pawn.
        /// First time pawn can move 2 tiles.
        /// After first only 1 tile at a time.
        /// </summary>
        internal override void FindPossibleMove() {
            var isFirstStepPossible = false;
            //Single step
            var singleStepTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());
            if (singleStepTile != null && singleStepTile.OccupiedPlayer == null) {
                singleStepTile.SetHighlighted();
                isFirstStepPossible = true;
            }

            //First time double step
            if (IsFirstTime && isFirstStepPossible) {
                var twoStepTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex, CurrentTile.YIndex + 2 * GetPlayerDirectionValue());
                if (twoStepTile != null && twoStepTile.OccupiedPlayer == null) {
                    twoStepTile.SetHighlighted();
                }
            }

            //Check for diagonal kill
            var leftDiagonalTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex + 1, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());
            var rightDiagonalTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex - 1, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());

            if (leftDiagonalTile != null && leftDiagonalTile.OccupiedPlayer != null && leftDiagonalTile.OccupiedPlayer.PlayerType == GameHandler.Instance.GetOppositePlayerType()) {
                leftDiagonalTile.SetHighlighted();
            }

            if (rightDiagonalTile != null && rightDiagonalTile.OccupiedPlayer != null && rightDiagonalTile.OccupiedPlayer.PlayerType == GameHandler.Instance.GetOppositePlayerType()) {
                rightDiagonalTile.SetHighlighted();
            }
        }

        private int GetPlayerDirectionValue() {
            return PlayerType == PlayerType.White ? 1 : -1;
        }
    }
}