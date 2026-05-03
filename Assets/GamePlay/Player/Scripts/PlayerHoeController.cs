using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHoeController : MonoBehaviour
{
    [Header("Player Refs")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerActionState playerActionState;
    [SerializeField] private Animator animator;
    [SerializeField] private FacingDirection faceDir;

    public FacingDirection FaceDir => faceDir;

    [Header("Impact Positions")]
    [SerializeField] private Transform impactN;
    [SerializeField] private Transform impactS;
    [SerializeField] private Transform impactE;
    [SerializeField] private Transform impactW;

    [Header("Tilemap Refs")]
    [SerializeField] private Tilemap tillableTilemap;
    [SerializeField] private Tilemap soilTilemap;

    [Header("Single Soil Tile")]
    [SerializeField] private TileBase soilSingleTile;

    [Header("Top Row Soil Tiles")]
    [SerializeField] private TileBase soilTopLeftTile;
    [SerializeField] private TileBase[] soilTopMiddleTiles;
    [SerializeField] private TileBase soilTopRightTile;

    [Header("Middle Row Soil Tiles")]
    [SerializeField] private TileBase soilMiddleLeftTile;
    [SerializeField] private TileBase[] soilMiddleMiddleTiles;
    [SerializeField] private TileBase soilMiddleRightTile;

    [Header("Bottom Row Soil Tiles")]
    [SerializeField] private TileBase soilBottomLeftTile;
    [SerializeField] private TileBase[] soilBottomMiddleTiles;
    [SerializeField] private TileBase soilBottomRightTile;

    [Header("Horizontal Row Soil Tiles")]
    [SerializeField] private TileBase soilHorizontalLeftTile;
    [SerializeField] private TileBase soilHorizontalMiddleTile;
    [SerializeField] private TileBase soilHorizontalRightTile;

    [Header("Vertical Row Soil Tiles")]
    [SerializeField] private TileBase soilVerticalTopTile;
    [SerializeField] private TileBase soilVerticalMiddleTile;
    [SerializeField] private TileBase soilVerticalBottomTile;

    [Header("Hoe Refs")]
    [SerializeField] private HoeController hoe;
    [SerializeField] private bool isHoeing;
    public bool IsHoeing => isHoeing;

    [Header("Equip Status")]
    [Tooltip("Temporary bool to indicate hoe has been equipped, will remove when I build equip system")]
    [SerializeField] private bool isEquipped = false;

    private void Awake()
    {
        if (!playerController) playerController = GetComponent<PlayerController>();
        if (!animator) animator = GetComponent<Animator>();
        if (!playerActionState) playerActionState = GetComponent<PlayerActionState>();
    }

    private void Update()
    {
        faceDir = playerController.Facing;
    }

    public void OnInteract()
    {
        if (!isEquipped) return;
        BeginHoeing();
    }

    public void BeginHoeing()
    {
        if (playerActionState.IsBusy) return;
        if (isHoeing) return;

        animator.SetBool("IsHoeing", true);
        hoe.PlayHoe();

        isHoeing = true;
        playerActionState.SetActionState(PlayerState.Hoeing);
    }

    // ---- Animation Events ---- //

    public void OnHoeImpact()
    {
        SwapToTilledTile();
    }

    public void EndHoeing()
    {
        animator.SetBool("IsHoeing", false);
        isHoeing = false;
        playerActionState.ClearActionState();
    }

    // ---- Tile Swap ---- //

    private void SwapToTilledTile()
    {
        Vector3Int targetCell = GetTargetCell();

        TileBase baseTile = tillableTilemap.GetTile(targetCell);

        if (baseTile is not TillableTile)
            return;

        // Place a temporary soil tile so this cell now counts as soil
        soilTilemap.SetTile(targetCell, soilSingleTile);

        // Refresh this tile and all neighboring tiles that may need to reconnect
        RefreshSoilArea(targetCell);
    }

    private void RefreshSoilArea(Vector3Int centerCell)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int cell = centerCell + new Vector3Int(x, y, 0);
                RefreshSoilTile(cell);
            }
        }
    }

    private void RefreshSoilTile(Vector3Int cell)
    {
        if (!HasSoil(cell))
            return;

        bool hasLeft = HasSoil(cell + Vector3Int.left);
        bool hasRight = HasSoil(cell + Vector3Int.right);
        bool hasUp = HasSoil(cell + Vector3Int.up);
        bool hasDown = HasSoil(cell + Vector3Int.down);

        TileBase chosenTile = GetSoilTile(cell, hasLeft, hasRight, hasUp, hasDown);

        if (chosenTile != null)
            soilTilemap.SetTile(cell, chosenTile);
    }

    private TileBase GetSoilTile(
        Vector3Int cell,
        bool hasLeft,
        bool hasRight,
        bool hasUp,
        bool hasDown
    )
    {
        // No neighbors
        if (!hasLeft && !hasRight && !hasUp && !hasDown)
            return soilSingleTile;

        // Horizontal-only row
        if ((hasLeft || hasRight) && !hasUp && !hasDown)
        {
            if (!hasLeft && hasRight) return soilHorizontalLeftTile;
            if (hasLeft && hasRight) return soilHorizontalMiddleTile;
            if (hasLeft && !hasRight) return soilHorizontalRightTile;
        }

        // Vertical-only column
        if ((hasUp || hasDown) && !hasLeft && !hasRight)
        {
            if (!hasUp && hasDown) return soilVerticalTopTile;
            if (hasUp && hasDown) return soilVerticalMiddleTile;
            if (hasUp && !hasDown) return soilVerticalBottomTile;
        }

        // Larger patch: top row
        if (!hasUp && hasDown)
        {
            if (!hasLeft && hasRight) return soilTopLeftTile;
            if (hasLeft && hasRight) return PickVariant(soilTopMiddleTiles, cell);
            if (hasLeft && !hasRight) return soilTopRightTile;
        }

        // Larger patch: middle row
        if (hasUp && hasDown)
        {
            if (!hasLeft && hasRight) return soilMiddleLeftTile;
            if (hasLeft && hasRight) return PickVariant(soilMiddleMiddleTiles, cell);
            if (hasLeft && !hasRight) return soilMiddleRightTile;
        }

        // Larger patch: bottom row
        if (hasUp && !hasDown)
        {
            if (!hasLeft && hasRight) return soilBottomLeftTile;
            if (hasLeft && hasRight) return PickVariant(soilBottomMiddleTiles, cell);
            if (hasLeft && !hasRight) return soilBottomRightTile;
        }

        return soilSingleTile;
    }

    private TileBase PickVariant(TileBase[] variants, Vector3Int cell)
    {
        if (variants == null || variants.Length == 0)
            return soilSingleTile;

        int index = Mathf.Abs(cell.x) % variants.Length;
        return variants[index];
    }

    private bool HasSoil(Vector3Int cell)
    {
        return soilTilemap.GetTile(cell) != null;
    }

    private Vector3Int GetTargetCell()
    {
        Transform impactTransform = faceDir switch
        {
            FacingDirection.North => impactN,
            FacingDirection.South => impactS,
            FacingDirection.East => impactE,
            FacingDirection.West => impactW,
            _ => impactS
        };

        Vector3 worldPos = impactTransform.position;
        return tillableTilemap.WorldToCell(worldPos);
    }
}