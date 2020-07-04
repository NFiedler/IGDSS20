using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Manager References
    JobManager _jobManager; //Reference to the JobManager
    GameManager _gameManager;//Reference to the GameManager
    NavigationManager _navigartionManager;
    #endregion

    public float _age; // The age of this worker
    public float _happiness; // The happiness of this worker

    private float timer = 0.0f;
    public int ageWaitTime = 15;

    private bool ofAge = false;
    private bool retired = false;
    public GameObject _workerObject;

    GameManager gameManager;

    float happiness = 10;

    bool _atWork = false;
    bool _arrived = true;
    int _walkIteration;
    public int _walkIterationCount = 100;

    public Tile _homeTile;
    Tile _currentGoalTile;
    Tile _currentTile;
    Tile _nextGoalTile;

    public bool hasAJob = false;

    public Job job;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _navigartionManager = GameObject.Find("GameManager").GetComponent<NavigationManager>();
        _jobManager = GameObject.Find("GameManager").GetComponent<JobManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Age();
        MovementIteration();
    }

    public void Commute()
    {
        if (_atWork)
        {
            CommuteHome();
        }
        else
        {
            CommuteToWork();
        }
    }

    private void CommuteToWork()
    {
        Debug.Log("Walking to work");
        if(job == null)
        {
            return;
        }
        _arrived = false;
        _currentGoalTile = job._building._tile;
        _currentTile = _homeTile;
        _nextGoalTile = FindNextGoalTile(_currentTile, _currentGoalTile);
        _atWork = true;
    }

    private void CommuteHome()
    {
        Debug.Log("Walking to home");
        _arrived = false;
        _currentGoalTile = _homeTile;
        _currentTile = job._building._tile;
        _nextGoalTile = FindNextGoalTile(_currentTile, _currentGoalTile);
        _atWork = false;
    }

    private void MovementIteration()
    {
        if(_arrived)
        {
            return;
        }
        if(_walkIteration <= _walkIterationCount)
        {
            Transform from = _currentTile._tileObject.GetComponent<Transform>();
            Transform to = _nextGoalTile._tileObject.GetComponent<Transform>();
            float newx = from.position.x + ((to.position.x - from.position.x) / _walkIterationCount) * _walkIteration;
            float newz = from.position.z + ((to.position.z - from.position.z) / _walkIterationCount) * _walkIteration;
            float newy = from.position.y;
            if(_walkIteration > _walkIterationCount/2)
            {
                newy = to.position.y;
            }
            _walkIteration += 1; 
            _workerObject.GetComponent<Transform>().position = new Vector3(newx, newy, newz);
            _workerObject.GetComponent<Transform>().rotation = Quaternion.Euler(0, Mathf.Atan2((to.position.x - from.position.x), (to.position.z - from.position.z)) * Mathf.Rad2Deg, 0);
        }
        else
        {
            _currentTile = _nextGoalTile;
            _nextGoalTile = FindNextGoalTile(_currentTile, _currentGoalTile);
            _walkIteration = 0;
        }
    }

    private Tile FindNextGoalTile(Tile currentTile, Tile goalTile)
    {
        if(goalTile._costMap[currentTile._coordinateWidth, currentTile._coordinateHeight] == 0) // if we are at the goal
        {
            _arrived = true;
            return null;
        }
        int minWeight = 500000;
        Tile minWeightTile = null;
        foreach(Tile t in currentTile._neighborTiles)
        {
            if(goalTile._costMap[t._coordinateWidth, t._coordinateHeight] < minWeight)
            {
                minWeightTile = t;
                minWeight = goalTile._costMap[t._coordinateWidth, t._coordinateHeight];
            }
        }
        return minWeightTile;
    }

    private void Age()
    {
        //TODO: Implement a life cycle, where a Worker ages by 1 year every 15 real seconds.
        //When becoming of age, the worker enters the job market, and leaves it when retiring.
        //Eventually, the worker dies and leaves an empty space in his home. His Job occupation is also freed up.

        timer += Time.deltaTime;

        if (timer >= ageWaitTime)
        {
            timer = timer - ageWaitTime;
            _age += 1;

            consume();

            if (_age > 14 && !ofAge)
            {
                BecomeOfAge();
                ofAge = true;
            }

            if (_age > 64 && !retired)
            {
                Retire();
                retired = true;
            }

            if (_age > 100)
            {
                Die();
            }
        }


    }

    private void consume()
    {
        // for some reason consume is called before start
        // WTF?!?
        // thus we have to find the game manager here too?
        // tested by printing in the start function. that print (even with logerror) is shown after the null pointer exception here
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Fish] >= 0.001f)
        {
            gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Fish] -= 0.001f;
            increaseHappiness();
        }
        else
        {
            reduceHappiness();
        }


        if (gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Clothes] >= 0.001f)
        {
            gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Clothes] -= 0.001f;
            increaseHappiness();

        }
        else
        {
            reduceHappiness();
        }


        if (gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Schnapps] >= 0.001f)
        {
            gameManager._resourcesInWarehouse[GameManager.ResourceTypes.Schnapps] -= 0.001f;
            increaseHappiness();
        }
        else
        {
            reduceHappiness();
        }


        if(hasAJob)
        {
            increaseHappiness();
        }
        else
        {
            reduceHappiness();
        }
    }

    private void reduceHappiness()
    {

        // catch case where happiness would dip below zero
        happiness = Math.Max(happiness - 0.25f, 0);
    }

    private void increaseHappiness()
    {

         happiness = Math.Min(happiness + 0.25f, 10f);
    }

    public void BecomeOfAge()
    {
        _jobManager = GameObject.Find("GameManager").GetComponent<JobManager>();
        _jobManager.RegisterWorker(this);
    }

    private void Retire()
    {
        _jobManager.RemoveWorker(this);
        if (job != null)
        {
            job.RemoveWorker(this);
            _jobManager._occupiedWorkers.Remove(this);
            job = null;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject, 1f);
        // in case at some point of the future workers can die before they have brought glory to the supreme leader by working until retirement:
        if(job != null)
        {
            job.RemoveWorker(this);
            job = null;
        }
    }

    public void SetAge(int age)
    {
        // we set the age to age - 1 and immediately let age put it to +1
        // this means that the worker can immediately take up a job
        _age = age - 1;
        timer = 15f;
        Age();
    }
}
