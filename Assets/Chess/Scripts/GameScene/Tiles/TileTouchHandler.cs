using UnityEngine;
using Chess.Scripts.GameScene;
using UnityEngine.EventSystems;
using Chess.Scripts.GameScene.Tiles;

public class TileTouchHandler : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        GameHandler.Instance.OnTileSelected(TilesHandler.GetTileByGameObject(gameObject));
    }
}