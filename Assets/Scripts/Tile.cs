﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //public Tile(TileTypes type, int coordinateWidth, int coordinateHeight)
    //{
    //    _type = type;
    //    _coordinateHeight = coordinateHeight;
    //    _coordinateWidth = coordinateWidth;
    //}

    public int GetNeigborTileCount(TileTypes type)
    {
        int count = 0;
        foreach(Tile t in _neighborTiles)
        {
            if(t._type == type)
            {
                count++;
            }
        }
        return count;
    }
    
    public enum Edges {Top, Topright, Bottomright, Bottom, Bottomleft, Topleft};

    public void SetEdge(Edges edge, bool set)
    {
        string edgename;
        switch(edge){
            case Edges.Top: 
                edgename = "TopEdge";
                break;
            case Edges.Topright:
                edgename = "TopRightEdge";
                break;
            case Edges.Bottomright:
                edgename = "BottomRightEdge";
                break;
            case Edges.Bottom:
                edgename = "BottomEdge";
                break;
            case Edges.Bottomleft:
                edgename = "BottomLeftEdge";
                break;
            case Edges.Topleft:
                edgename = "TopLeftEdge";
                break;
            default:
                Debug.LogError("This should have never ever happened.");
                edgename = "";
                break;
        }
        GameObject currentEdge = _tileObject.transform.Find(edgename).gameObject;
        currentEdge.SetActive(set);
    }

    #region Attributes
    public GameObject _tileObject;
    public int _pathfindingCost;
    public TileTypes _type; //The type of the tile
    public Building _building; //The building on this tile
    public List<Tile> _neighborTiles; //List of all surrounding tiles. Generated by GameManager
    public int _coordinateHeight; //The coordinate on the y-axis on the tile grid (not world coordinates)
    public int _coordinateWidth; //The coordinate on the x-axis on the tile grid (not world coordinates)
    public int[,] _costMap;
    #endregion

    #region Enumerations
    public enum TileTypes { Empty, Water, Sand, Grass, Forest, Stone, Mountain }; //Enumeration of all available tile types. Can be addressed from other scripts by calling Tile.Tiletypes
    #endregion

    //This class acts as a data container and has no functionality
}
