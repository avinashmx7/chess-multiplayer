using Chess.Scripts.GameScene.Players.Interfaces;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class BishopHandler : MonoBehaviour, IPlayer {
        public void PrintName() {
            Debug.Log("I'm Bishop!");
        }

        public void GetPossibleTiles() {
            throw new System.NotImplementedException();
        }

        public void MoveToTile() {
            throw new System.NotImplementedException();
        }
    }
}