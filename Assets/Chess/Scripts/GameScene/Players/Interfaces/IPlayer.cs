namespace Chess.Scripts.GameScene.Players.Interfaces {
    public interface IPlayer {
        public void PrintName();
        public void GetPossibleTiles();
        public void MoveToTile();
    }
}