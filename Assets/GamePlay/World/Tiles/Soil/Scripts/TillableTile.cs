using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/Tillable Tile")]
public class TillableTile : Tile
{
    public bool isTillable = true;
}
