using UnityEngine;

namespace Chess.Scripts.GameScene.Tiles {
    public static class TilesHandler {
        private static readonly Tile[,] ChessTiles = new Tile[8, 8];

        internal static void SetTile(Tile tile) {
            if (ValidateIndex(tile.XIndex) && ValidateIndex(tile.YIndex)) {
                ChessTiles[tile.XIndex, tile.YIndex] = tile;
            }
        }

        internal static void PrintTilesTable() {
            var tableContents = string.Empty;
            for (var j = ChessTiles.GetLength(1) - 1; j >= 0; j--) {
                for (var i = 0; i < ChessTiles.GetLength(0); i++) {
                    var occupiedPlayer = ChessTiles[i, j].OccupiedPlayer;
                    var playerName = "<color=\"red\">X</color>";
                    if (occupiedPlayer != null) {
                        playerName = "" + occupiedPlayer.name.Split('_')[1][0];
                        if (occupiedPlayer.name[0] == 'B') {
                            playerName = $"<color=\"black\">{playerName}</color>";
                        }
                    }

                    tableContents += playerName + " ";
                }

                tableContents += "\n";
            }

            Debug.Log(tableContents);
        }

        internal static Tile GetTileByIndex(int xIndex, int yIndex) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            if (ValidateIndex(xIndex) && ValidateIndex(yIndex)) {
                return ChessTiles[xIndex, yIndex];
            }

            //Debug.LogError("Invalid index value.");
            return null;
        }

        internal static Tile GetTileByGameObject(GameObject tileGameObj) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var tile in ChessTiles) {
                if (tile.GameObj == tileGameObj) {
                    return tile;
                }
            }

            return null;
        }

        internal static void DeselectAllTiles() {
            foreach (var tile in ChessTiles) {
                tile.SetIdle();
            }
        }


        private static bool ValidateIndex(int indexValue) {
            // ReSharper disable once MergeIntoPattern
            return indexValue >= 0 && indexValue < 8;
        }
    }
}