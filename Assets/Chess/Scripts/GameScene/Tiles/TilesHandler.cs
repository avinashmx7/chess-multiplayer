using UnityEngine;

namespace Chess.Scripts.GameScene.Tiles {
    public static class TilesHandler {
        private static readonly Tile[,] ChessTiles = new Tile[8, 8];

        #region Tile getter & setter functions.

        internal static void SetTile(Tile tile) {
            if (ValidateIndex(tile.XIndex) && ValidateIndex(tile.YIndex)) {
                ChessTiles[tile.XIndex, tile.YIndex] = tile;
            }
        }

        /// <summary>
        /// Get tile by index number.
        /// </summary>
        /// <param name="xIndex">X index</param>
        /// <param name="yIndex">Y index</param>
        /// <returns>Return tile if valid X & Y index else returns NULL</returns>
        internal static Tile GetTileByIndex(int xIndex, int yIndex) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            if (ValidateIndex(xIndex) && ValidateIndex(yIndex)) {
                return ChessTiles[xIndex, yIndex];
            }

            //Debug.LogError("Invalid index value.");
            return null;
        }

        /// <summary>
        /// Get tile object by tile game object.
        /// </summary>
        /// <param name="tileGameObj">Tile game object.</param>
        /// <returns>Returns tile object or NULL</returns>
        internal static Tile GetTileByGameObject(GameObject tileGameObj) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var tile in ChessTiles) {
                if (tile.GameObj == tileGameObj) {
                    return tile;
                }
            }

            return null;
        }

        /// <summary>
        /// Deselect all tiles.
        /// </summary>
        internal static void DeselectAllTiles() {
            foreach (var tile in ChessTiles) {
                tile.SetIdle();
            }
        }

        #endregion


        #region Helper functions.

        /// <summary>
        /// Private function to validate the index of tile
        /// </summary>
        /// <param name="indexValue">Index value</param>
        /// <returns>Return true or false if index is valid or invalid respectively.</returns>
        private static bool ValidateIndex(int indexValue) {
            // ReSharper disable once MergeIntoPattern
            return indexValue >= 0 && indexValue < 8;
        }

        #endregion

        #region Debug functions

        /// <summary>
        /// Prints 2D array of tiles and player data for debugging, whenever a player is tapped or selected.
        /// </summary>
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

        #endregion
    }
}