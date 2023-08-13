using Enemies;
using Game.Scripts.GameBoardLogic.Board;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsDistancer : MonoBehaviour
{
    public Action onBuild;

    [SerializeField] private GameObject _topMainBuilding;
    [SerializeField] private GameObject _bottomMainBuilding;

    private List<GameObject> _topBuildings = new();
    private List<GameObject> _bottomBuildings = new();

    private void Awake() => Initialize(); //delete this after subscribing on game start event

    private void Initialize() //subscrive on game start event
    {
        Validate();

        Board.OnBoardChanged += ( list1, list2 ) =>
        {
            if ( this == null || !this )
                return;

            AddBuilding( list1, list2 );
        };

        _topBuildings.Add(_topMainBuilding);
        _bottomBuildings.Add(_bottomMainBuilding);
    }

    private void Validate()
    {
        _topBuildings.Clear();
        _bottomBuildings.Clear();
    }

    private void AddBuilding(List<GameObject> topBuildings, List<GameObject> lowBuildings)
    {
        Validate();

        _topBuildings.Add(_topMainBuilding);
        _bottomBuildings.Add(_bottomMainBuilding);

        _topBuildings.AddRange(topBuildings);
        _bottomBuildings.AddRange(lowBuildings);

        onBuild?.Invoke();
    }

    public GameObject TryGetClosestBuilding(BaseEnemy enemy, Dimension enemyDimension)
    {
        List<GameObject> buildings = GetDimensionBuildings(enemyDimension);

        if (buildings.Count < 1)
            return null;

        GameObject closestBuilding = buildings[0];
        float closestBuildingDistance = Vector2.Distance(enemy.gameObject.transform.position, closestBuilding.transform.position);

        for (int i = 1; i < buildings.Count; i++)
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
