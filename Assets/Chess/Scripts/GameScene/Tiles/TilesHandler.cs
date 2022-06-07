using UnityEngine;

namespace Chess.Scripts.GameScene.Tiles {
    public static class TilesHandler {
        private static readonly Tile[,] ChessTiles = new Tile[8, 8];

        internal static void SetTile(Tile tile) {
            if (ValidateIndex(tile.XIndex) && ValidateIndex(tile.YIndex)) {
                ChessTiles[tile.XIndex, tile.YIndex] = tile;
            }
        }

        internal static Tile GetTileByIndex(int xIndex, int yIndex) {
            // ReSharper disable once LoopCanBeConvertedToQuery
            if (ValidateIndex(xIndex) && ValidateIndex(yIndex)) {
                return ChessTiles[xIndex, yIndex];
            }

            Debug.LogError("Invalid index value.");
            return null;
        }

        private static bool ValidateIndex(int indexValue) {
            // ReSharper disable once MergeIntoPattern
            if (indexValue >= 0 && indexValue <= 8) return true;
            Debug.LogError($"Invalid X index value = {indexValue}");
            return false;
        }
    }
}