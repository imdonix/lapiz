using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    public const int BOTTOM = -5;
    public const float MACHINE_UPDATE = 3F;

    [Header("World")]
    [SerializeField] private Vector3 PlayerStartPosition;
    [SerializeField] private List<Vector3> IronOreVainPositions;


    [Header("Resources")]
    [SerializeField] public IronOreVain IronOreVainPref;

    [Header("Machines")]
    [SerializeField] public Thrower ThrowerPref;
    [SerializeField] public Furnace FurcanePref;


    public static World Loaded;

    private PathFinder pathFinder;
    private List<Machine> machines;

    private float machineUpdateTimer = 0;

    #region UNITY

    private void Awake()
    {
        this.pathFinder = GetComponent<PathFinder>();
        this.machines = new List<Machine>();

        Loaded = this;
    }

    private void Start()
    {
        if (AssertEditor()) return;
        InitPlayer();


        if (PhotonNetwork.IsMasterClient)
        {
            CreateWorld();
        }

        pathFinder.ReloadTerrain();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateMachines();
        }    
    }

    #endregion

    public void RegisterMachine(Machine machine)
    {
        machines.Add(machine);
    }

    public void DeRegisterMachine(Machine machine)
    {
        machines.Remove(machine);
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return PlayerStartPosition;
    }

    public PathFinder GetPathFinder()
    {
        return pathFinder;
    }

    public void InitPlayer()
    {
        NPlayer myself = PhotonNetwork.Instantiate(Manager.Instance.PlayerPref.name, PlayerStartPosition, Quaternion.identity).GetComponent<NPlayer>();
        myself.TakeControll();
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < 1; i++)
        {
            yield return new WaitForSeconds(2);
            PhotonNetwork.InstantiateRoomObject(Manager.Instance.ChuninPref.name, PlayerStartPosition + Vector3.up * 3, Quaternion.identity);

        }
    }

    private void UpdateMachines()
    {
        if (machineUpdateTimer > MACHINE_UPDATE)
        {
            machineUpdateTimer = 0;
            foreach (Machine machine in machines)
                machine.ReadInput();
        }

        machineUpdateTimer += Time.deltaTime;
    }

    private void CreateWorld()
    {
        CreateHarvestables();

        //TEST

        PhotonNetwork.InstantiateRoomObject(ThrowerPref.name, new Vector3(43, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(FurcanePref.name, new Vector3(53, 1, 43), Quaternion.identity);

        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.SwordPref.name, new Vector3(53, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.SwordPref.name, new Vector3(53, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.SwordPref.name, new Vector3(53, 1, 43), Quaternion.identity);


        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.AxePref.name, new Vector3(53, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.PickaxePref.name, new Vector3(53, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.PickaxePref.name, new Vector3(53, 1, 43), Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.PickaxePref.name, new Vector3(53, 1, 43), Quaternion.identity);

        
        for (int i = 0; i < 6; i++)
            PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.LapizPref.name, new Vector3(53, 3+i, 43), Quaternion.identity);
        for (int i = 0; i < 45; i++)
            PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.IronOrePref.name, new Vector3(53, 10+i, 43), Quaternion.identity);
        

        StartCoroutine(SpawnEnemy());
    }

    private void CreateHarvestables()
    {
        foreach (Vector3 pos in IronOreVainPositions)
            PhotonNetwork.InstantiateRoomObject(IronOreVainPref.name, pos, Quaternion.identity);
    }

    private bool AssertEditor()
    {
        if (ReferenceEquals(Manager.Instance, null))
        {
            SceneManager.LoadScene("Start");
            return true;
        }
        return false;            
    }

}
