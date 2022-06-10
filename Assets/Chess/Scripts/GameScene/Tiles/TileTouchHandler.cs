using Chess.Scripts.GameScene;
using Chess.Scripts.GameScene.Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileTouchHandler : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        GameHandler.Instance.OnTileSelected(TilesHandler.GetTileByGameObject(gameObject));
    }
}