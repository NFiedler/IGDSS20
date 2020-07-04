using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingBuilding : Building
{
    public List<GameObject> workers;

    List<GameObject> possibleWorkers;
    public GameObject worker_female;
    public GameObject worker_male;

    public int start_position;

    private float timer = 0.0f;
    public int spawnTimer = 30;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        housingBuilding = true;
        //possibleWorkers.Add(worker_female);
        //possibleWorkers.Add(worker_male);
        // spawns 2 grown workers when built
        GameObject initialWorker = createWorker();
        initialWorker.GetComponent<Worker>().SetAge(20);

        GameObject initialWorker2 = createWorker();
        initialWorker2.GetComponent<Worker>().SetAge(20);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTimer  + (1 - CalcEfficiency()) * 2 * spawnTimer)
            {
                createWorker();
                timer = 0f;
            }

        }

        private GameObject createWorker()
    {
        foreach (GameObject w in workers)
        {
            if (w == null)
            {
                workers.Remove(w);
            }
        }
        if (workers.Count < 10)
        {
            GameObject worker;
            if(Random.Range(0,2) == 1)
            {
                worker = GameObject.Instantiate(worker_female, _tile.transform.position, Quaternion.identity);
            }
            else
            {
                worker = GameObject.Instantiate(worker_male, _tile.transform.position, Quaternion.identity);
            }
            //GameObject worker = GameObject.Instantiate(possibleWorkers[Random.Range(0, 2)], _tile.transform.position, Quaternion.identity);
            worker.GetComponent<Worker>()._homeTile = this._tile;
            worker.GetComponent<Worker>()._workerObject = worker;
            workers.Add(worker);
            rearrangeWorkers();
            return worker;
        }

        return null;
    }

    // arranges the workers so they don't stick in each other in front of the building
    private void rearrangeWorkers()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            float x = _tile.transform.position.x - 4 + 0.8f * i;
            float z = _tile.transform.position.z - 2;
            workers[i].transform.position = new Vector3((float) x, workers[i].transform.position.y, z);
        }
    }

    private double CalcEfficiency()
    {
        double efficiency = 0;
        foreach(GameObject w in workers)
        {
            if (w == null)
            {
                workers.Remove(w);
                return CalcEfficiency();
            }
            efficiency += (double) w.GetComponent<Worker>()._happiness;
        }
        // calc average efficiency, divide by 10, to have values between 0 and 1
        return (efficiency / workers.Count) / 10;
    }
}
