using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class WorldObject : Entity
{

    private World world;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        this.world = FindObjectOfType<World>();
        transform.SetParent(world.transform);
    }

    #endregion

    public World GetWorld()
    {
        return world;
    }

}
