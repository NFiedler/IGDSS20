using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ProductionBuilding : Building
{
    private float timer = 0.0f;
    private float _efficiency; // Calculated based on the surrounding tile types
    public int _resource_generation_interval; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public int _output_count; // The number of output resources per generation cycle (for example the Sawmill produces 2 planks at a time)
    public Boolean input_is_wood;
    public Boolean input_is_wool;
    public Boolean input_is_potato;
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

    public GameManager.ResourceTypes input_ressource; // A choice for input resource types (0, 1 or 2 types)
    public GameManager.ResourceTypes output_ressource; // A choice for output resource type

    public int availableJobs;
    private List<Job> jobList = new List<Job>();
    public int workerCount;


    public void calc_efficiency()
    {
        _efficiency = (calc_tile_efficiency() + calc_worker_efficiency()) / 2;

    }

    private float calc_tile_efficiency()
    {
        float efficiency;
        if (_min_neighbors > 0)
        {
            if (water_must_be_neighbor)
            {
                int neighbors = _tile.GetNeigborTileCount(Tile.TileTypes.Water);
                if (neighbors == 0)
                {
                    // prevent division by zero
                    efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    efficiency = Math.Min(1, (float)neighbors / (float)_max_neighbors);
                }
                return efficiency;
            }
            else if (forest_must_be_neighbor)
            {
                int neighbors = _tile.GetNeigborTileCount(Tile.TileTypes.Forest);
                if (neighbors == 0)
                {
                    // prevent division by zero
                    efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    efficiency = Math.Min(1, (float)neighbors / (float)_max_neighbors);
                }
                return efficiency;
            }
            else if (grass_must_be_neighbor)
            {
                int neighbors = _tile.GetNeigborTileCount(Tile.TileTypes.Grass);
                if (neighbors == 0)
                {
                    // prevent division by zero
                    efficiency = 0;
                }
                else
                {
                    // prevent efficiency > 1
                    efficiency = Math.Min(1, (float)neighbors / (float)_max_neighbors);
                }
                return efficiency;
            }
            else
            {
                Debug.LogError("Something went wrong in the efficiency calculation.");
                return 0;
            }
        }
        else
        {
            efficiency = 1;
            return efficiency;
        }
    }
    private float calc_worker_efficiency()
    {
        int jobsFilled = 0;
        float happinessSum = 0;
        foreach(Job j in jobList)
        {
            if (j._worker != null)
            {
                jobsFilled += 1;
                happinessSum += j._worker._happiness;
            }
        }
        float avgHappiness = happinessSum / jobsFilled;
        workerCount = jobsFilled;

        // we want a value between 0 and 1 so we divide the happiness by its maximum value
        // and then we divide by 2, because we have 2 parameters going up to 1 each
        return ((avgHappiness / 10) + (jobsFilled / availableJobs)) / 2;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        productionBuilding = true;
        JobManager jobman = gameManager.GetComponent<JobManager>();
        for(int i = 0; i < availableJobs; i++)
        {
            Job job = new Job(this);
            jobList.Add(job);
            jobman.addJob(job);
        }
    }

    // Update is called once per frame
    void Update()
    {
        calc_efficiency();
        if (_efficiency > 0)
        {
            float interval = _resource_generation_interval + (1 - _efficiency) * 3 * _resource_generation_interval;
                    timer += Time.deltaTime;

                    if (timer >  interval)
                    {
                        GameManager gm = gameManager.GetComponent<GameManager>();
                        if (input_ressource == GameManager.ResourceTypes.None || gm._resourcesInWarehouse[input_ressource] >= 1)
                        {
                            if (input_ressource != GameManager.ResourceTypes.None)
                            {
                                gm._resourcesInWarehouse[input_ressource] -= 1;
                            }
                            gm._resourcesInWarehouse[output_ressource] += _output_count;
                        }
                        timer = timer - interval;
                    }
        }
        
    }
}