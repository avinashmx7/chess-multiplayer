using UnityEngine;
using Chess.Scripts.GameScene.Tiles;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene.Players {
    public class KnightHandler : Player {
        private readonly int[,] _possibleMoves = {{-2, 1}, {-1, 2}, {1, 2}, {2, 1}, {2, -1}, {1, -2}, {-1, -2}, {-2, -1}};

        //For debugging.
        internal override void PrintName() {
            Debug.Log($"I'm {PlayerType} Knight!");
        }

        internal override void FindPossibleMove() {
            for (var i = 0; i < _possibleMoves.GetLength(0); i++) {
                var tile = TilesHandler.GetTileByIndex(CurrentTile.XIndex - _possibleMoves[i, 0], CurrentTile.YIndex - _possibleMoves[i, 1]);
                ValidateTile(tile);
            }
        }
    }
}