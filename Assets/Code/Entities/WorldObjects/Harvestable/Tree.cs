using Photon.Pun;
using System.Collections;
using UnityEngine;


public class Tree : Harvestable
{
    private const float MAX_TIME = 30;
    private const float MAX = 0.65F;
    private const float MIN = 0.15F;

    private float lifeTime = MAX_TIME;

    #region UNITY

    protected override void Update()
    {
        base.Update();

        if (PhotonNetwork.IsMasterClient)
        {
            if (!IsGrown())
            {
                lifeTime += Time.deltaTime;
                RenderState(lifeTime);
            }
        }
    }

    #endregion

    public override Tool GetCorrectTool()
    {
        return ItemLibrary.Instance.AxePref;
    }

    public override float GetSize()
    {
        return 1F;
    }

    protected override int GetRate()
    {
        return 10;
    }

    protected override Item GetReward()
    {
        return ItemLibrary.Instance.StickPref;
    }

    public override bool IsWorkAvailable()
    {
        return IsGrown();
    }

    private bool IsGrown()
    {
        return lifeTime > MAX_TIME;
    }

    protected override void OnHarvested()
    {
        lifeTime = 0;
    }

    private void RenderState(float state)
    {
        transform.localScale = Vector3.one * Mathf.Lerp(MIN, MAX, state / MAX_TIME);
    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            RenderState((float)stream.ReceiveNext());
        }
        else
        {
            stream.SendNext(lifeTime);
        }
    }

    #endregion
}


