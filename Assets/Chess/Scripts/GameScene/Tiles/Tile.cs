using UnityEngine;

namespace Chess.Scripts.GameScene.Tiles {
    public class Tile {
        private static readonly char[] RowIndex = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'};

        public Tile(GameObject gameObj,int xIndex, int yIndex) {
            GameObj = gameObj;
            XIndex = xIndex;
            YIndex = yIndex;
            TileName = $"{RowIndex[xIndex]}{yIndex}";
            Transform = gameObj.transform;
        }
        

        internal GameObject GameObj { get; private set; }
        internal Transform Transform { get; private set; }
        internal int XIndex { get; private set; }
        internal int YIndex { get; private set; }
        internal string TileName { get; private set; }
    }
}