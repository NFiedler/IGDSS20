﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    #region Manager References
    JobManager _jobManager; //Reference to the JobManager
    GameManager _gameManager;//Reference to the GameManager
    #endregion

    public float _age; // The age of this worker
    public float _happiness; // The happiness of this worker

    private float timer = 0.0f;
    public int ageWaitTime = 15;

    private bool ofAge = false;
    private bool retired = false;

    GameManager gameManager;

    float happiness = 10;

    public bool hasAJob = false;

    public Job job;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _jobManager = GameObject.Find("GameManager").GetComponent<JobManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Age();
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
