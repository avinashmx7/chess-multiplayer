using Chess.Scripts.GameScene.Players.BasePlayer;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class RookHandler : Player {
        private readonly int[,] _possibleMoves = {{0, 1}, {0, -1}, {1, 0}, {-1, 0}};

        internal override void PrintName() {
            Debug.Log($"I'm {PlayerType} Rook!");
        }

        internal override void FindPossibleMove() {
            for (var i = 0; i < _possibleMoves.GetLength(0); i++) {
                FindRecursiveTiles(CurrentTile.XIndex, CurrentTile.YIndex, _possibleMoves[i, 0], _possibleMoves[i, 1]);
            }
        }
    }
}