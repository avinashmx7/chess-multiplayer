using System.Collections.Generic;
using Chess.Scripts.GameScene.Players.BasePlayer;
using Chess.Scripts.GameScene.Tiles;
using ExitGames.Client.Photon;
using UnityEngine;


namespace Chess.Scripts.GameScene.Players {
    public class PlayerGenerator : MonoBehaviour {
        private readonly string[] _playerPositions = {"Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook"};
        private const string Pawn = "Pawn";

        private void Start() {
            Invoke(nameof(SpawnPlayers), 1f);
        }

        internal void SpawnPlayers() {
            var onlyPawns = false;
            string pawn = null;

            var whitePlayersParentTransform = new GameObject("WhitePlayers").transform;
            var blackPlayersParentTransform = new GameObject("BlackPlayers").transform;


            if (PhotonNetwork.isMasterClient) {
                GameHandler.Instance.SetClientPlayerType(PlayerType.White);

                for (var yIndex = 0; yIndex < 2; yIndex++) {
                    for (var xIndex = 0; xIndex < 8; xIndex++) {
                        var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);
                        var playerName = $"White_{(!onlyPawns ? _playerPositions[xIndex] : pawn)}";

                        var playerGameObj = PhotonNetwork.Instantiate(playerName, tile.Transform.position, Quaternion.identity, 0);
                        playerGameObj.transform.parent = whitePlayersParentTransform;

                        var player = playerGameObj.GetComponent<Player>();
                        player.UpdateCurrentTile(tile);

                        playerGameObj.AddComponent<PlayerTouchHandler>();
                    }

                    onlyPawns = true;
                    pawn = Pawn;
                }
            } else {
                GameHandler.Instance.SetClientPlayerType(PlayerType.Black);

                for (var yIndex = 7; yIndex >= 6; yIndex--) {
                    for (var xIndex = 0; xIndex < 8; xIndex++) {
                        var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);
                        
                        var playerName = $"Black_{(!onlyPawns ? _playerPositions[xIndex] : pawn)}";

                        var playerGameObj = PhotonNetwork.Instantiate(playerName, tile.Transform.position, Quaternion.identity, 0);
                        playerGameObj.transform.parent = blackPlayersParentTransform;

                        var player = playerGameObj.GetComponent<Player>();
                        player.UpdateCurrentTile(tile);

                        playerGameObj.AddComponent<PlayerTouchHandler>();
                    }

                    onlyPawns = true;
                    pawn = Pawn;
                }
            }
        }
    }
}