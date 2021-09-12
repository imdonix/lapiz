using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviourPun, IPunObservable
{
    [Header("Settings")]
    [SerializeField] private float idlePhaseTime;
    [SerializeField] private float attackPhaseTime;
    [SerializeField] private Wave[] Waves;

    [SerializeField] private bool attacking = false;
    [SerializeField] private float countDown = 0;
    [SerializeField] private int remaining = 0;

    private int readyCounter = 0;

    private int index = 0;
    private bool ready = false;

    public static Story Loaded;

    #region UNITY

    private void Awake()
    {
        Loaded = this;

        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            remaining = attacking ? LivingEntity.GetEnemies().Count : readyCounter;
            countDown -= Time.deltaTime;

            if (countDown < 0 || ((remaining == 0) && attacking))
            {
                if (attacking)
                    IdlePhase();
                else
                    AttackPhase();
            }

            HUD.Instance.UpdateStory(attacking, countDown, remaining, ready);
        }
    }

    #endregion

    private void SpawnNextWave() 
    {
        index++;
        int current = index % Waves.Length;
        int level = index / Waves.Length;

        Wave wave = Waves[current];
        foreach (LivingEntity entity in wave.Entities)
        {
            LivingEntity ent = PhotonNetwork.InstantiateRoomObject(entity.name,
                World.Loaded.GetAEnemySpawnPosition(),
                Quaternion.identity).GetComponent<LivingEntity>();
            ent.LevelUp(level);
        }
    }

    private void IdlePhase()
    {
        countDown = idlePhaseTime;
        attacking = false;
    }

    private void AttackPhase()
    {
        readyCounter = 0;
        ready = false;
        countDown = attackPhaseTime;
        attacking = true;
        SpawnNextWave();
    }

    private void StartGame()
    {
        IdlePhase();
    }

    #region SERIALIZATION

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.countDown = (float)stream.ReceiveNext();
            this.attacking = (bool)stream.ReceiveNext();
            this.remaining = (int)stream.ReceiveNext();
            this.ready = attacking ? false : this.ready;
            HUD.Instance.UpdateStory(attacking, countDown, remaining, ready);
        }
        else
        {
            stream.SendNext(countDown);
            stream.SendNext(attacking);
            stream.SendNext(remaining);
        }
    }

    #endregion

    #region PUN

    [PunRPC]
    public void OnReady(bool ready)
    {
        if (attacking) return;

        readyCounter += ready ? 1 : -1;
        if (readyCounter >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            AttackPhase();
        }
    }

    public void SendReady()
    {
        ready = !ready;
        photonView.RPC("OnReady", RpcTarget.MasterClient, ready);
    }

    #endregion
}