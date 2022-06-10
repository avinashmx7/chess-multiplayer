using Chess.Scripts.GameScene.Players.Interfaces;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;

namespace Chess.Scripts.GameScene.Players {
    public class QueenHandler : Player {
        internal override void PrintName() {
            Debug.Log("I'm Queen!");
        }

        internal override void GetPossibleTiles() {
            
        }
    }
}