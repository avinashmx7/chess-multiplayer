using Chess.Scripts.GameScene.Players.Interfaces;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class KnightHandler : Player {
        internal override void PrintName() {
            Debug.Log("I'm Knight!");
        }

        internal override void GetPossibleTiles() {
            
        }
    }
}