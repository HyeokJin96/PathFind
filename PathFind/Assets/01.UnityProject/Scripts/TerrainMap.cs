using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainMap : TileMapController
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTilemap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;

    private List<TerrainController> allTerrains = default;

    //! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.
    public override void InitAwake(MapBoard mapConroller_)
    {
        this.tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapConroller_);

        allTerrains = new List<TerrainController>();

        mapCellSize = Vector2Int.zero;

        float temTileY = allTileObjs[0].transform.localPosition.y;

        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (temTileY.IsEquals(allTileObjs[i].transform.localPosition.y) == false)
            {
                mapCellSize.x = i;
            break;
            }
        }

        mapCellSize.y = Mathf.FloorToInt(allTileObjs.Count / mapCellSize.x);

        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x - allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y - allTileObjs[0].transform.localPosition.y;
    }

    private void Start()
    {
        //  Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����
        GameObject changeTilePrefab = ResManager.Instance.terrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];
        // Ÿ�ϸ� �߿� ��� ������ �ٴٷ� ��ü�� ������ �����Ѵ�.
        const float CHANGE_PERCENT = 15.0f;
        float correctChangePercentage = allTileObjs.Count * (CHANGE_PERCENT / 100.0f);
        //  �ٴٷ� ��ü�� Ÿ���� ������ ����Ʈ ���·� �����ؼ� ���´�.
        List<int> changedTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        changedTileResult.Shuffle();

        GameObject tempchangeTile = default;
        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (correctChangePercentage <= changedTileResult[i])
            {
                continue;
            }

            tempchangeTile = Instantiate(changeTilePrefab, tileMap.transform);
            tempchangeTile.name = changeTilePrefab.name;
            tempchangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempchangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempchangeTile, i);
            tempchangeTile.DestroyObj();
        }   //  Loop : ������ ������ ������ ���� Ÿ�ϸʿ� �ٴٸ� �����ϴ� ����

        //  ������ �����ϴ� Ÿ���� ������ �����ϰ�, ��Ʈ�ѷ��� ĳ���ϴ� ����
    }   //  Start()

    //! �ʱ�ȭ�� Ÿ���� ������ ������ ���� ����, ���� ũ�⸦ �����Ѵ�.
    public Vector2Int GetCellSize()
    {
        return mapCellSize;
    }   //  GetCellSize()

    //! �ʱ�ȭ�� Ÿ���� ������ ������ Ÿ�� ������ ���� �����Ѵ�.
    public Vector2 GetCellGap()
    {
        return mapCellGap;
    }   //  GetCellGap()

    //! �ε����� �ش��ϴ� Ÿ���� �����Ѵ�.
    public TerrainController GetTile(int tileIdx1D)
    {
        if (allTerrains.IsValid(tileIdx1D))
        {
            return allTerrains[tileIdx1D];
        }
        return default;
    }   //  GetTile()
}
