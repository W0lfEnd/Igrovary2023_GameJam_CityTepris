using Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsDistancer : MonoBehaviour
{
    public Action<Dimension> onBuild;

    private List<GameObject> _topBuildings = new();
    private List<GameObject> _bottomBuildings = new();

    private void AddBuilding(GameObject building, Dimension buildingDimension) //subscribe onBuild action
    {
        switch (buildingDimension)
        {
            case Dimension.TopDimesion:
                {
                    _topBuildings.Add(building);
                    break;
                }
            case Dimension.BottomDimension:
                {
                    _bottomBuildings.Add(building);
                    break;
                }
            default:
                {
                    _topBuildings.Add(building);
                    break;
                }
        }
    }

    public GameObject TryGetClosestBuilding(BaseEnemy enemy, Dimension enemyDimension)
    {
        List<GameObject> buildings = GetDimensionBuildings(enemyDimension);

        if (buildings.Count < 1)
            return null;

        GameObject closestBuilding = buildings[0];
        float closestBuildingDistance = Vector2.Distance(enemy.gameObject.transform.position, buildings[0].transform.position);

        for (int i = 0; i < buildings.Count; i++)
        {
            float nextBuildingDistance = Vector2.Distance(enemy.gameObject.transform.position, buildings[i].transform.position);

            if (nextBuildingDistance < closestBuildingDistance)
            {
                closestBuilding = buildings[i];
                closestBuildingDistance = nextBuildingDistance;
            }
        }

        return closestBuilding;
    }

    private List<GameObject> GetDimensionBuildings(Dimension dimension)
    {
        switch (dimension)
        {
            case Dimension.TopDimesion:
                {
                    return _topBuildings;
                }
            case Dimension.BottomDimension:
                {
                    return _bottomBuildings;
                }
            default:
                {
                    return _topBuildings;
                }
        }
    }
}
