using UnityEngine;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public Sprite[] Tiles;
    public bool IsWalkable;
    public int Weight;
}