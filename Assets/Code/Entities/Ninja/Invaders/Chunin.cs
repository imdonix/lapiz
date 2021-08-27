using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Chunin : Invader 
{

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        head.SetBadge(Village.Kerth);
    }

    #endregion

}

