using Photon.Pun;
using UnityEngine;


public abstract class WorldObject : Entity, IJobProvider
{
    private World world;
    private JobProvider provider;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        this.world = FindObjectOfType<World>();
        this.transform.SetParent(world.transform);

        if (PhotonNetwork.IsMasterClient)
        {
            this.provider = new JobProvider(this, GetPriority());
        }
    }

    #endregion

    public World GetWorld()
    {
        return world;
    }

    public JobProvider GetProvider()
    {
        return provider;
    }

    public abstract float GetSize();

    protected abstract int GetPriority();

    public abstract Job GetJob(NPC npc);

    public  abstract bool IsWorkAvailable();
}
