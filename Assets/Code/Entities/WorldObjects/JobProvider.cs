using System.Collections;
using UnityEngine;


/// <summary>
/// The priority of jobs are based on:
/// 100 > important
/// 50 > requested
/// 20 > base
/// </summary>


public class JobProvider
{
    public const int IMPORTANT = 100;
    public const int REQUESTED = 50;
    public const int BASE = 20;

    public const int HARVESTABLE = BASE + 3;
    public const int CRAFTER = BASE + 2;
    public const int STORAGE = BASE + 1;
    public const int THROWER = BASE;

    private readonly WorldObject station;
    private readonly int priority;

    private NPC worker;

    public JobProvider(WorldObject station, int priority)
    {
        this.station = station;
        this.priority = priority;
    }

    public Job ApplyJob(NPC worker)
    {
        this.worker = worker;
        return station.GetJob(worker);
    }

    public bool IsAvailable()
    {
        if (!station.IsWorkAvailable())
            return false;
        if (ReferenceEquals(this.worker, null))
            return true;
        if (ReferenceEquals(this.worker.GetCurrentProvider(), this))
            return false;
        return true;

    }

    public void DiscardJob()
    {
        this.worker = null;
    }

    public int GetPriority()
    {
        return priority;
    }
}
