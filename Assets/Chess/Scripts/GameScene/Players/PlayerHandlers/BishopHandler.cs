using UnityEngine;
using Chess.Scripts.GameScene.Players.BasePlayer;
using Chess.Scripts.GameScene.Tiles;

namespace Chess.Scripts.GameScene.Players {
    public class BishopHandler : Player {
        private readonly int[,] _possibleMoves = {{1, 1}, {1, -1}, {-1, 1}, {-1, -1}};

        internal override void PrintName() {
            Debug.Log($"I'm {PlayerType} Bishop!");
        }

        internal override void FindPossibleMove() {
            for (var i = 0; i < _possibleMoves.GetLength(0); i++) {
                FindRecursiveTiles(CurrentTile.XIndex, CurrentTile.YIndex, _possibleMoves[i, 0], _possibleMoves[i, 1]);
            }
        }
    }
}