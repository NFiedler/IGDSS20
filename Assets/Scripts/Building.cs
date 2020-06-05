using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Tile _tile; // tile the building is built on
    private float _efficiency; // Calculated based on the surrounding tile types
    public int _upkeep; // money cost per minute
    public int _build_cost_money; // placement money cost
    public int _build_cost_planks; // placement planks cost
    public int _resource_generation_interval; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public int _output_count; // The number of output resources per generation cycle (for example the Sawmill produces 2 planks at a time)
    public Boolean input_is_wood;
    public Boolean input_is_wool;
    public Boolean input_is_potato;
    public Boolean water_can_be_built_on;
    public Boolean sand_can_be_built_on;
    public Boolean grass_can_be_built_on;
    public Boolean forest_can_be_built_on;
    public Boolean stone_can_be_built_on;
    public Boolean mountain_can_be_built_on;
    public Boolean fish_is_output;
    public Boolean wood_is_output;
    public Boolean planks_is_output;
    public Boolean wool_is_output;
    public Boolean cloth_is_output;
    public Boolean potato_is_output;
    public Boolean schnapps_is_output;
    public Boolean water_must_be_neighbor;
    public Boolean forest_must_be_neighbor;
    public Boolean grass_must_be_neighbor;
    public Tile _efficiency_scale_with_neighbor_tile; // A choice if its efficiency scales with a specific type of surrounding tile
    public int _min_neighbors; // The minimum and maximum number of surrounding tiles its efficiency scales with (0-6)
    public int _max_neighbors; // The minimum and maximum number of surrounding tiles its efficiency scales with (0-6)

    // TODO these 2 probably shouldn't be a string
    private string input_ressource; // A choice for input resource types (0, 1 or 2 types)
    private string output_ressource; // A choice for output resource type

    Building(Tile tile) 
    {
        _tile = tile;
        if(_min_neighbors > 0)
        {
            if(water_must_be_neighbor)
            {
                int neighbors = tile.GetNeigborTileCount(Tile.TileTypes.Water);
                if(neighbors == 0)
                {
                    // prevent division by zero
                    _efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    _efficiency = Math.Min(1, neighbors / _max_neighbors);
                }
            }
            else if(forest_must_be_neighbor)
            {
                int neighbors = tile.GetNeigborTileCount(Tile.TileTypes.Forest);
                if (neighbors == 0)
                {
                    // prevent division by zero
                    _efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    _efficiency = Math.Min(1, neighbors / _max_neighbors);
                }
            }
            else if(grass_must_be_neighbor)
            {
                int neighbors = tile.GetNeigborTileCount(Tile.TileTypes.Grass);
                if (neighbors == 0)
                {
                    // prevent division by zero
                    _efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    _efficiency = Math.Min(1, neighbors / _max_neighbors);
                }
            }
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}