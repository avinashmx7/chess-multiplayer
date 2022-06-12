using UnityEngine;
using Chess.Scripts.GameScene.Tiles;
using Chess.Scripts.GameScene.Players.BasePlayer;


namespace Chess.Scripts.GameScene.Players {
    public class PlayerGenerator : MonoBehaviour {
        private readonly string[] _playerPositions = {"Rook", "Knight", "Bishop", "Queen", "King", "Bishop", "Knight", "Rook"};
        private const string Pawn = "Pawn";

        private void Start() {
            Invoke(nameof(SpawnPlayers), 1f);
        }

        /// <summary>
        /// Function to spawn player when game begins.
        /// White player - master or host player who creates the room.
        /// Black player - second player who joins the room.
        /// </summary>
        internal void SpawnPlayers() {
            var onlyPawns = false;
            string pawn = null;

            if (PhotonNetwork.isMasterClient) {
                for (var yIndex = 0; yIndex < 2; yIndex++) {
                    for (var xIndex = 0; xIndex < 8; xIndex++) {
                        var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);
                        var playerName = $"White_{(!onlyPawns ? _playerPositions[xIndex] : pawn)}";

                        var playerGameObj = PhotonNetwork.Instantiate(playerName, tile.Transform.position, Quaternion.identity, 0);

                        var player = playerGameObj.GetComponent<Player>();
                        player.UpdateCurrentTile(tile);

                        playerGameObj.AddComponent<PlayerTouchHandler>();
                        playerGameObj.GetComponent<PhotonView>().RPC("SetParent", PhotonTargets.All);
                    }

                    onlyPawns = true;
                    pawn = Pawn;
                }

                GameHandler.Instance.SetClientPlayerType(PlayerType.White);
            } else {
                for (var yIndex = 7; yIndex >= 6; yIndex--) {
                    for (var xIndex = 0; xIndex < 8; xIndex++) {
                        var tile = TilesHandler.GetTileByIndex(xIndex, yIndex);

                        var playerName = $"Black_{(!onlyPawns ? _playerPositions[xIndex] : pawn)}";

                        var playerGameObj = PhotonNetwork.Instantiate(playerName, tile.Transform.position, Quaternion.identity, 0);

                        var player = playerGameObj.GetComponent<Player>();
                        player.UpdateCurrentTile(tile);

                        playerGameObj.AddComponent<PlayerTouchHandler>();
                        playerGameObj.GetComponent<PhotonView>().RPC("SetParent", PhotonTargets.All);
                    }

                    onlyPawns = true;
                    pawn = Pawn;
                }

                GameHandler.Instance.SetClientPlayerType(PlayerType.Black);
            }
        }
    }
}