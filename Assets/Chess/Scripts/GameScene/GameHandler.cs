using Chess.Scripts.GameScene.Players.Interfaces;
using UnityEngine;

namespace Chess.Scripts.GameScene {
    public class GameHandler : MonoBehaviour {
        internal static GameHandler Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        private void Start() { }

        public void OnPlayerSelected(GameObject playerGameObj) {
            playerGameObj.GetComponent<IPlayer>().PrintName();
        }

        private void OnDestroy() {
            Instance = null;
        }
    }
}