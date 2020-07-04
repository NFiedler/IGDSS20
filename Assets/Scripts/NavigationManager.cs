using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public GameManager _gameManager;
    private int[,] _baseCostMap;

    int _n = 500000;
    int _k = 1000000;
    bool once = true;

    // Start is called before the first frame update
    void Start()
    {

    }
    public int[,] GetCostMap(int x, int y)
    {
        Debug.Log("We are starting the cost map");
        int a, b;
        int[,] costMap = new int[_baseCostMap.GetLength(0), _baseCostMap.GetLength(0)];
        for (a = 0; a < _baseCostMap.GetLength(0); a++)
        {
            for (b = 0; b < _baseCostMap.GetLength(1); b++)
            {
                costMap[a, b] = _n;
            }
        }
        costMap[x, y] = 0;
        int currentX = x, currentY = y, currentMin = 0;
        while(currentMin < _k)
        {
            HandleNeigbors(currentX, currentY, costMap);
            costMap[currentX, currentY] += _k;
            Tuple<int, int, int> min = FindMin(costMap);
            currentX = min.Item1;
            currentY = min.Item2;
            currentMin = min.Item3;
        }


        for (a = 0; a < _baseCostMap.GetLength(0); a++)
        {
            for (b = 0; b < _baseCostMap.GetLength(1); b++)
            {
                costMap[a, b] -= _k;
            }
        }
        return costMap;
    }

    Tuple<int, int, int> FindMin(int[,] inArray)
    {
        int minA=-1, minB=-1, min = 1000000000;
        for (int a = 0; a < inArray.GetLength(0); a++)
        {
            for (int b = 0; b < inArray.GetLength(1); b++)
            {
                if(inArray[a, b] < min)
                {
                    minA = a;
                    minB = b;
                    min = inArray[a, b];
                }
            }
        }
        return Tuple.Create(minA, minB, min);
    }

    void HandleNeigbors(int x, int y, int[,] inArray)
    {
        foreach(Tuple<int, int> c in GetNeigborCoordinates(x, y))
        {
            if(inArray[c.Item1, c.Item2] == _n)
            {
                inArray[c.Item1, c.Item2] = inArray[x, y] + _baseCostMap[c.Item1, c.Item2];
            }
        }
    }

    List<Tuple<int, int>> GetNeigborCoordinates(int x, int y)
    {
        List<Tuple<int, int>> neighborCoordinates = new List<Tuple<int, int>>();

        List<Tile> neigborList = _gameManager._tileMap[x, y]._neighborTiles;
        foreach(Tile t in neigborList)
        {
            neighborCoordinates.Add(Tuple.Create<int, int>(t._coordinateWidth, t._coordinateHeight));
        }
        return neighborCoordinates;
    }

    // Update is called once per frame
    void Update()
    {
        if (once)
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            Debug.Log(_gameManager);
            Debug.Log(_gameManager._tileMap);
            _baseCostMap = new int[_gameManager._tileMap.GetLength(0), _gameManager._tileMap.GetLength(1)];
            for (int x = 0; x < _gameManager._tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < _gameManager._tileMap.GetLength(1); y++)
                {
                    _baseCostMap[x, y] = _gameManager._tileMap[x, y]._pathfindingCost;
                }
            }
        }
        once = false;
    }
}
