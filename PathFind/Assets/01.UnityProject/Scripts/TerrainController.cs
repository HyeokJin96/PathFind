using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private const string TILE_FRONT_RENDERER_OBJ_NAME = "FrontRenderer";

    private TerrainType terrainType = TerrainType.NONE;

    public bool isPassable { get; private set; } = false;
    public int TileIdx1D { get; private set; } = -1;
    public Vector2Int TileIdx2D { get; private set; } = default;

    #region 길찾기 알고리즘을 위한 변수
    private SpriteRenderer frontRenderer = default;

    private Color defaultColor = default;
    private Color selectColor = default;
    private Color searchColor = default;
    private Color inactivateColor = default;
    #endregion

    private void Awake()
    {
    }   //  Awake()
}
