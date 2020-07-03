using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.WSA;

public class fieldcreator
{
    Dictionary<Tile, int> tileweights;

    public fieldcreator(Building b)
    {
        tileweights = new Dictionary<Tile, int>();
        calcweight(b._tile, 0);
    }

    private int calcweight(Tile t, int prevWeight)
    {
        int weight = prevWeight
        while (true)
        {
            foreach (Tile tile in t._neighborTiles)
            {
                
                if (!tileweights.ContainsKey(tile))
                {
                    calcweight(tile, weight);
                }
            }


        }
    }
}
