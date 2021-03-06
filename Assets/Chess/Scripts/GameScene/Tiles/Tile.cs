using System;
using UnityEngine;
using Chess.Scripts.GameScene.Players.BasePlayer;

namespace Chess.Scripts.GameScene.Tiles {
    [Serializable]
    public class Tile {
        private static readonly char[] RowIndex = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'};
        internal GameObject GameObj { get; private set; }
        internal Transform Transform { get; private set; }
        internal int XIndex { get; private set; }
        internal int YIndex { get; private set; }
        internal string TileName { get; private set; }

        internal Player OccupiedPlayer;
        private readonly GameObject _indicatorGameObj;
        private readonly Collider2D _tileCollider;

        public Tile(GameObject gameObj, int xIndex, int yIndex) {
            GameObj = gameObj;
            XIndex = xIndex;
            YIndex = yIndex;
            TileName = $"{RowIndex[xIndex]}{yIndex + 1}";
            Transform = gameObj.transform;
            _indicatorGameObj = gameObj.transform.GetChild(0).gameObject;
            _tileCollider = gameObj.GetComponent<Collider2D>();
            SetIdle();
        }

        /// <summary>
        /// Sets the tile as idle. Disables the highlight game object.
        /// </summary>
        internal void SetIdle() {
            _indicatorGameObj.SetActive(false);
            _tileCollider.enabled = false;
        }

        /// <summary>
        /// Sets the tile highlighted. Enables the highlight game object.
        /// </summary>
        internal void SetHighlighted() {
            _indicatorGameObj.SetActive(true);
            _tileCollider.enabled = true;
        }
    }
}