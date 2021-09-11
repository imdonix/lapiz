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

    private int index = 0;

    #region UNITY

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            remaining = LivingEntity.GetEnemies().Count;
            countDown -= Time.deltaTime;

            if (countDown < 0 || ((remaining == 0) && attacking))
            {
                if (attacking)
                    IdlePhase();
                else
                    AttackPhase();
            }
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
        countDown = attackPhaseTime;
        attacking = true;
        SpawnNextWave();
    }

    private void StartGame()
    {
        IdlePhase();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.countDown = (float)stream.ReceiveNext();
            this.attacking = (bool) stream.ReceiveNext();
            this.remaining = (int)stream.ReceiveNext();
        }
        else
        {
            stream.SendNext(countDown);
            stream.SendNext(attacking);
            stream.SendNext(remaining);
        }

        HUD.Instance.UpdateStory(attacking, countDown, remaining);
    }
}