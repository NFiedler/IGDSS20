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



        if (timer >= spawnTimer  + (1 - CalcEfficiency()) * 3 * spawnTimer)
            {
                createWorker();
            }

        }

        private GameObject createWorker()
    {
        if (workers.Count < 10)
        {
            //GameObject worker = GameObject.Instantiate(possibleWorkers[Random.Range(0, 2)], _tile.transform.position, Quaternion.identity);
            GameObject worker = GameObject.Instantiate(worker_female, _tile.transform.position, Quaternion.identity);

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
            double x = workers[i].transform.position.x - 4 + 0.6 * i;
            workers[i].transform.position = new Vector3((float) x, workers[i].transform.position.y, workers[i].transform.position.z);
        }
    }

    private double CalcEfficiency()
    {
        double efficiency = 0;
        foreach(GameObject w in workers)
        {
            efficiency += (double) w.GetComponent<Worker>()._happiness;
        }
        // calc average efficiency, divide by 10, to have values between 0 and 1
        return (efficiency / workers.Count) / 10;
    }
}
