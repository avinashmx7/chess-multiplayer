using System;
using System.Collections.Generic;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;


namespace Chess.Scripts.GameScene.Players {
    public class PlayerGenerator : MonoBehaviour {
        [SerializeField] private List<GameObject> whitePieces, blackPieces;
        private readonly string[] PlayerPositions = {"Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook"};
        private const string Pawn = "Pawn";

        private void Start() {
            Invoke(nameof(SpawnPlayers), 1f);
        }

        internal void SpawnPlayers() {
            var onlyPawns = false;
            GameObject pawn = null;

            var whitePlayersParentTransform = new GameObject("WhitePlayers").transform;
            for (var yIndex = 0; yIndex < 2; yIndex++) {
                for (var xIndex = 0; xIndex < 8; xIndex++) {
                    var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);
                    var playerToSpawn = !onlyPawns ? whitePieces.Find(whitePiece => whitePiece.name.Contains(PlayerPositions[xIndex])) : pawn;
                    var playerGameObj = Instantiate(playerToSpawn, tile.Transform.position, Quaternion.identity);
                    playerGameObj.transform.parent = whitePlayersParentTransform;
                }

                onlyPawns = true;
                pawn = whitePieces.Find(whitePiece => whitePiece.name.Contains(Pawn));
            }

            onlyPawns = false;
            pawn = null;
            
            var blackPlayersParentTransform = new GameObject("BlackPlayers").transform;
            for (var yIndex = 7; yIndex >= 6; yIndex--) {
                for (var xIndex = 0; xIndex < 8; xIndex++) {
                    var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);
                    var playerToSpawn = !onlyPawns ? blackPieces.Find(whitePiece => whitePiece.name.Contains(PlayerPositions[xIndex])) : pawn;
                    var playerGameObj = Instantiate(playerToSpawn, tile.Transform.position, Quaternion.identity);
                    playerGameObj.transform.parent = blackPlayersParentTransform;
                }

                onlyPawns = true;
                pawn = blackPieces.Find(whitePiece => whitePiece.name.Contains(Pawn));
            }
        }
    }
}