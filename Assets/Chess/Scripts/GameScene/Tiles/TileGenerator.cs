using System;
using UnityEngine;

namespace Chess.Scripts.GameScene.Tiles {
    public class TileGenerator : MonoBehaviour {
        [SerializeField] private GameObject tilePrefab;
        private const float InitialX = -2.26f, InitialY = -2.255f, GridSize = 8;
        private float _offsetX, _offsetY;

        private void Start() {
            var tileBound = tilePrefab.GetComponent<SpriteRenderer>().bounds;
            _offsetX = tileBound.size.x;
            _offsetY = tileBound.size.y;
            GenerateTiles();
        }

        private void GenerateTiles() {
            for (var yIndex = 0; yIndex < GridSize; yIndex++) {
                
                //Creating empty parent game object for each row.
                var rowTransform = new GameObject($"Row_{yIndex + 1}") {
                    transform = {
                        parent = transform
                    }
                }.transform;

                
                for (var xIndex = 0; xIndex < GridSize; xIndex++) {
                    var xPos = InitialX + _offsetX * xIndex;
                    var yPos = InitialY + _offsetY * yIndex;
                    var tileGameObj = Instantiate(tilePrefab, new Vector2(xPos, yPos), Quaternion.identity);
                    tileGameObj.transform.parent = rowTransform;
                    
                    var tile = new Tile(tileGameObj, xIndex, yIndex);
                    TilesHandler.SetTile(tile);
                    
                    tileGameObj.name = $"Tile_{tile.TileName}";
                }
            }
        }
    }
}