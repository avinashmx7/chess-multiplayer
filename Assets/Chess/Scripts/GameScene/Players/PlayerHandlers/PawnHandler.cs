using System;
using Chess.Scripts.GameScene.Players.Interfaces;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class PawnHandler : Player {
        private bool _isFirstMove = true;

        internal override void PrintName() {
            Debug.Log("I'm Pawn!");
        }

        internal override void GetPossibleTiles() {
            var isFirstStepPossible = false;
            //Single step
            var singleStepTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());
            if (singleStepTile != null && singleStepTile.OccupiedPlayer == null) {
                singleStepTile.SetHighlighted();
                isFirstStepPossible = true;
            }

            //First time double step
            if (_isFirstMove && isFirstStepPossible) {
                var twoStepTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex, CurrentTile.YIndex + 2 * GetPlayerDirectionValue());
                if (twoStepTile != null && twoStepTile.OccupiedPlayer == null) {
                    twoStepTile.SetHighlighted();
                }
            }


            //Check for diagonal kill
            var leftDiagonalTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex + 1, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());
            var rightDiagonalTile = TilesHandler.GetTileByIndex(CurrentTile.XIndex - 1, CurrentTile.YIndex + 1 * GetPlayerDirectionValue());
            var oppositePlayer = CurrentPlayerType == PlayerType.White ? PlayerType.Black : PlayerType.White;

            if (leftDiagonalTile != null && leftDiagonalTile.OccupiedPlayer != null && leftDiagonalTile.OccupiedPlayer.GetPlayerType() == oppositePlayer) {
                leftDiagonalTile.SetHighlighted();
            }

            if (rightDiagonalTile != null && rightDiagonalTile.OccupiedPlayer != null && rightDiagonalTile.OccupiedPlayer.GetPlayerType() == oppositePlayer) {
                rightDiagonalTile.SetHighlighted();
            }
        }

        private int GetPlayerDirectionValue() {
            return CurrentPlayerType == PlayerType.White ? 1 : -1;
        }
    }
}