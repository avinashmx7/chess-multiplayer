using Chess.Scripts.GameScene.Players.Interfaces;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class RookHandler : MonoBehaviour, IPlayer {
        public void PrintName() {
            Debug.Log("I'm Rook!");
        }

        public void GetPossibleTiles() {
            throw new System.NotImplementedException();
        }

        public void MoveToTile() {
            throw new System.NotImplementedException();
        }
    }
}