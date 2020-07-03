using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour
{

    private List<Job> _availableJobs = new List<Job>();
    public List<Worker> _unoccupiedWorkers = new List<Worker>();



    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Worker w in _unoccupiedWorkers)
        {
            if (w == null)
            {
                _unoccupiedWorkers.Remove(w);
            }
        }
        HandleUnoccupiedWorkers();

    }
    #endregion


    #region Methods

    private void HandleUnoccupiedWorkers()
    {
        if (_unoccupiedWorkers.Count > 0 && _availableJobs.Count > 0)
        {
            // This could be done in a loop. 
            // We liked the idea better to call it once every update, because now it becomes an animation instead
            // Instead of 50 people showing up in one instant at the Schnapps Distillery, they show up one after another
            assignWorker(_unoccupiedWorkers[0]);
        }
    }

    public void RegisterWorker(Worker w)
    {
        _unoccupiedWorkers.Add(w);
    }



    public void RemoveWorker(Worker w)
    {
        _unoccupiedWorkers.Remove(w);
        _availableJobs.Add(w.job);
    }

    public void addJob(Job j)
    {
        _availableJobs.Add(j);
    }

    private void assignWorker(Worker w)
    {
        Job job = _availableJobs[Random.Range(0, _availableJobs.Count)];
        if(job == null)
        {
            _availableJobs.Remove(job);
            return;
        }
        w.job = job;
        w.hasAJob = true;
        job._worker = w;
        _unoccupiedWorkers.Remove(w);
        _availableJobs.Remove(job);
    }
    #endregion
}
