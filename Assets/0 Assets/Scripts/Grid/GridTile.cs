using Sleep0.Logic.Grid;
using UnityEngine;

namespace Sleep0.Visual
{
    public class GridTile : MonoBehaviour, ITileable
    {
        [SerializeField]
        private TileData _tileData;

        public bool IsWalkable() => _tileData.IsWalkable;

        public void SetRandomTileSprite()
        {
            GetComponent<SpriteRenderer>().sprite = _tileData.Tiles[Random.Range(0, _tileData.Tiles.Length - 1)];
        }
    }
}