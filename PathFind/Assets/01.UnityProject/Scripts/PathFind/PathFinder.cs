using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : GSingleton<PathFinder>
{
    #region ���� Ž���� ���� ����
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion

    #region A Star �˰������� �ִܰŸ��� ã�� ���� ����
    private List<AstarNode> aStarResultPath = default;
    private List<AstarNode> aStarOpenPath = default;
    private List<AstarNode> aStarClosePath = default;
    #endregion

    //! ������� ������ ������ ���� ã�� �Լ�
    public void FindPath_Astar()
    {
        StartCoroutine(DelayFindPath_Astar(1.0f));
    }   //  FindPath_Astar()

    //! Ž�� �˰��� �����̸� �Ǵ�.
    private IEnumerator DelayFindPath_Astar(float delay_)
    {
        //  A star �˰����� ����ϱ� ���ؼ� �н� ����Ʈ�� �ʱ�ȭ�Ѵ�.
        aStarOpenPath = new List<AstarNode>();
        aStarClosePath = new List<AstarNode>();
        aStarResultPath = new List<AstarNode>();

        TerrainController targetTerrain = default;

        //  ������� �ε����� ���ؼ�, ����� ��带 ã�ƿ´�.
        string[] sourceObjNameParts = sourceObj.name.Split('_');
        int sourceIdx1D = -1;
        int.TryParse(sourceObjNameParts[sourceObjNameParts.Length - 1], out sourceIdx1D);

        targetTerrain = mapBoard.GetTerrain(sourceIdx1D);

        //  ã�ƿ� ����� ��带 open ����Ʈ�� �߰��Ѵ�.
        AstarNode targetNode = new AstarNode(targetTerrain, destinationObj);
        Add_AstarOpenList(targetNode);

        int loopIdx = 0;
        bool isFoundDestination = false;
        bool isNowayToGo = false;

        while (isFoundDestination == false && isNowayToGo == false)
        {
            //  open list�� ��ȸ�ؼ� ���� �ڽ�Ʈ�� ���� ��带 �����Ѵ�.
            AstarNode MinCostNode = default;
            foreach (var terrainNode in aStarOpenPath)
            {
                if (MinCostNode == default)
                {
                    MinCostNode = terrainNode;
                }   //  if : ���� ���� �ڽ�Ʈ�� ��尡 ��� �ִ� ���
                else
                {
                    //  terrainNode�� �� ���� �ڽ�Ʈ�� ������ ��� minCostNode�� ������Ʈ�Ѵ�.
                    if (terrainNode.AstarF < MinCostNode.AstarF)
                    {
                        MinCostNode = terrainNode;
                    }
                    else
                    {
                        continue;
                    }
                }   //  else : ���� ���� �ڽ�Ʈ�� ��尡 ĳ�̵Ǿ� �ִ� ���
            }   //  loop : ���� �ڽ�Ʈ�� ���� ��带 ã�� ����

            MinCostNode.ShowCost_Astar();
            MinCostNode.Terrain.SetTileActiveColor(RDefine.TileStatusColor.SEARCH);

            //  ������ ��尡 �������� �����ߴ��� Ȯ���Ѵ�.
            bool isArriveDest = mapBoard.GetDistance2D(MinCostNode.Terrain.gameObject, destinationObj).Equals(Vector2.zero);

            GFunc.Log($"minCostNode : {MinCostNode}, {isArriveDest}");
            if (isArriveDest)
            {
                //  �������� ���� �ߴٸ� aStarResultPath ����Ʈ�� �����Ѵ�.
                AstarNode resultNode = MinCostNode;
                bool isSet_aStarResultPathOk = false;
                while (isSet_aStarResultPathOk == false)
                {
                    aStarResultPath.Add(resultNode);
                    if (resultNode.AstarPrevNode == default || resultNode.AstarPrevNode == null)
                    {
                        isSet_aStarResultPathOk = true;
                        break;
                    }
                    else
                    {

                    }

                    resultNode = resultNode.AstarPrevNode;
                }   //  loop : ���� ��带 ã�� ���� ������ ��ȸ�ϴ� ����

                //  open list�� close list�� �����Ѵ�.
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }   //  if : ������ ��尡 �������� ������ ���
            else
            {
                //  �������� �ʾҴٸ� ���� Ÿ���� �������� 4���� ��带 ã�ƿ´�.
                List<int> nextSearchIdx1Ds = mapBoard.GetTileIdx2D_Around4ways(MinCostNode.Terrain.TileIdx2D);

                GFunc.Log($"nextSearchIdx1Ds : {nextSearchIdx1Ds.Count}");
                GFunc.Log($"MinCostNode.Terrain.TileIdx2D : {MinCostNode.Terrain.TileIdx2D}");

                //  ã�ƿ� ��� �߿��� �̵� ������ ���� open list�� �߰��Ѵ�.
                AstarNode nextNode = default;
                foreach (var nextIdx1D in nextSearchIdx1Ds)
                {
                    nextNode = new AstarNode(mapBoard.GetTerrain(nextIdx1D), destinationObj);

                    if (nextNode.Terrain.isPassable == false)
                    {
                        continue;
                    }

                    GFunc.Log($"nextIdx1D : {nextIdx1D}");

                    Add_AstarOpenList(nextNode, MinCostNode);
                }   //  loop : �̵� ������ ��带 open list�� �߰��ϴ� ����

                //  Ž���� ���� ���� close list�� �߰��ϰ�, open list���� �����Ѵ�.
                //  �� ��, open list�� ��� �ִٸ� �� �̻� Ž���� �� �ִ� ���� �������� �ʴ� ���̴�.
                aStarClosePath.Add(MinCostNode);
                aStarOpenPath.Remove(MinCostNode);
                if (aStarOpenPath.IsValid() == false)
                {
                    GFunc.LogWarning("[Warning] There are no more tiles to explore");
                    isNowayToGo = true;
                }   //  if : �������� �������� ���ߴµ�, �� �̻� Ž���� �� �ִ� ���� ���� ���

                foreach (var tempNode in aStarOpenPath)
                {
                    GFunc.Log($"Idx : {tempNode.Terrain.TileIdx1D}, Cost : {tempNode.AstarF}");
                }
            }   //  else : ������ ��尡 �������� �������� ���� ���

            loopIdx++;
            yield return new WaitForSeconds(delay_);
        }   //  loop : A star �˰������� ���� ã�� ���� ����
    }   //  DelayFindPath_Astar()

    //! ����� ������ ��带 Open ����Ʈ�� �߰��Ѵ�.
    private void Add_AstarOpenList(AstarNode targetTerrain_, AstarNode prevNode = default)
    {
        //  Open ����Ʈ�� �߰��ϱ� ���� �˰��� ����� �����Ѵ�.
        Update_AstarCostToTerrain(targetTerrain_, prevNode);

        AstarNode closeNode = aStarClosePath.FindNode(targetTerrain_);

        if (closeNode != default && closeNode != null)
        {
            //  �̹� Ž���� ���� ��ǥ�� ��尡 �����ϴ� ��쿡�� Open list�� �߰����� �ʴ´�.
            /* Do Nothing */
        }   //  if : close list�� �̹� Ž���� ���� ��ǥ�� ��尡 �����ϴ� ���
        else
        {
            AstarNode opendNode = aStarOpenPath.FindNode(targetTerrain_);

            if (opendNode != default && opendNode != null)
            {
                //  Ÿ�� ����� �ڽ�Ʈ�� �� ���� ��쿡�� open list���� ��带 ��ü���� �ʰ�, �� ū��쿡�� open list�� �߰����� �ʴ´�.
                if (targetTerrain_.AstarF < opendNode.AstarF)
                {
                    aStarOpenPath.Remove(opendNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                else
                {
                    /* Do Nothing */
                }
            }   //  if : open list�� ���� �߰��� ���� ���� ��ǥ�� ��尡 �����ϴ� ���
            else
            {
                aStarOpenPath.Add(targetTerrain_);
            }   //  else : open list�� ���� �߰��� ���� ���� ��ǥ�� ��尡 ���� ���
        }   //  else : ���� Ž���� ������ ���� ����� ���
    }   //  Add_AstarOpenList()

    //! Target ���� ������ Destination ���� ������ Distance�� Heuristic�� �����ϴ� �Լ�
    private void Update_AstarCostToTerrain(AstarNode targetNode, AstarNode prevNode)
    {
        //  Target �������� Destination ������ 2D Ÿ�� �Ÿ��� ����ϴ� ����
        Vector2Int distance2D = mapBoard.GetDistance2D(targetNode.Terrain.gameObject, destinationObj);
        int totalDistance2D = distance2D.x + distance2D.y;

        //  Heuristic�� �����Ÿ��� �����Ѵ�.
        Vector2 localDistance = destinationObj.transform.localPosition - targetNode.Terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);

        // ���� ��尡 �����ϴ� ��� ���� ����� �ڽ�Ʈ�� �߰��ؼ� �����Ѵ�.
        if (prevNode == default || prevNode == null)
        {
            /* Do Nothing */
        }
        else
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AstarG + 1.0f);
        }
        targetNode.UpdateCost_Astar(totalDistance2D, heuristic, prevNode);
    }   //  Update_AstarCostToTerrain()
}
