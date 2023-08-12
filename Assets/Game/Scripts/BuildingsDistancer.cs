using Enemies;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsDistancer : MonoBehaviour
{
    public Action<GameObject, Dimension> onBuild;
    private List<GameObject> _topBuildings = new();
    private List<GameObject> _bottomBuildings = new();

    public void AddBuilding(GameObject building, Dimension buildingDimension)
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

    public void TryGetClosestBuilding(BaseEnemy enemy, Dimension enemyDimension)
    {
        //if (_buildings.Count < 1)
        //    return null;

        //GameObject closestBuilding = _buildings[0];
        //float closestBuildingDistance = Vector2.Distance(enemy.gameObject.transform.position, _buildings[0].transform.position);

        //for (int i = 0; i < _buildings.Count; i++)
        //{
        //    float nextBuildingDistance = Vector2.Distance(enemy.gameObject.transform.position, _buildings[i].transform.position);

        //    if (nextBuildingDistance < closestBuildingDistance)
        //    {
        //        closestBuilding = _buildings[i];
        //        closestBuildingDistance = nextBuildingDistance;
        //    }
        //}

        //return closestBuilding;
    }
}
