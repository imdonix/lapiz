using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    public const int BOTTOM = -5;
    public const float MACHINE_UPDATE = 3F;

    public static World Loaded;

    [Header("World")]
    [SerializeField] private Vector3 PlayerStartPosition;
    [SerializeField] private Vector3 VillagerStartPosition;
    [SerializeField] private Vector3[] EnemyStartPositions;
    [SerializeField] private List<Vector3> IronOreVainPositions;
    [SerializeField] private List<Vector3> FurnacePositions;
    [SerializeField] private Vector3 StoragePositions;

    [Header("Resources")]
    [SerializeField] public IronOreVain IronOreVainPref;

    [Header("Machines")]
    [SerializeField] public Thrower ThrowerPref;
    [SerializeField] public Furnace FurcanePref;
    [SerializeField] public Storage StoragePref;


    private PathFinder pathFinder;
    private List<Machine> machines;
    private List<IJobProvider> jobs;

    private FreeCam freeCam;

    private float machineUpdateTimer = 0;

    #region UNITY

    private void Awake()
    {
        this.pathFinder = GetComponent<PathFinder>();
        this.machines = new List<Machine>();
        this.jobs = new List<IJobProvider>();
        this.freeCam = GetComponentInChildren<FreeCam>();

        Loaded = this;
    }

    private void Start()
    {
        if (AssertEditor()) return;
        if (PhotonNetwork.IsMasterClient)
        {
            CreateWorld();
            CreateStory();
            pathFinder.ReloadTerrain();
        }

        InitPlayer();
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

    public Vector3 GetAEnemySpawnPosition()
    {
        return EnemyStartPositions[UnityEngine.Random.Range(0, EnemyStartPositions.Length)];
    }

    public Vector3 GetVillagerSpawnPoint()
    {
        return VillagerStartPosition;
    }

    public JobProvider[] GetJobs()
    {
        return jobs.Select(j => j.GetProvider()).ToArray();
    }


    public void TakeControll(NPlayer player)
    {
        HUD.Instance.SwitchPlayerOverlay();
        player.TakeControll();
    }

    public void TakeFreeCam()
    {
        HUD.Instance.SwitchFreecamOverlay();
        freeCam.EnableFreeCam();
    }

    private void CreateStory()
    {
        PhotonNetwork.InstantiateRoomObject(Manager.Instance.Story.name, Vector3.zero, Quaternion.identity);
    }


    private void InitPlayer()
    {
        NPlayer myself = PhotonNetwork.Instantiate(Manager.Instance.PlayerPref.name, PlayerStartPosition, Quaternion.identity).GetComponent<NPlayer>();
        myself.TakeControll();
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
        CreateMachines();

        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.SwordPref.name, PlayerStartPosition, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.IronIngotPref.name, PlayerStartPosition, Quaternion.identity);
        PhotonNetwork.InstantiateRoomObject(ItemLibrary.Instance.IronOrePref.name, PlayerStartPosition, Quaternion.identity);
    }

    private void CreateHarvestables()
    {
        foreach (Vector3 pos in IronOreVainPositions)
        {
            Harvestable harvestable = PhotonNetwork.InstantiateRoomObject(
                IronOreVainPref.name, 
                pos, 
                Quaternion.identity)
                .GetComponent<Harvestable>();
            jobs.Add(harvestable);
        } 
    }

    private void CreateMachines()
    {
        CreateSotrages();
        CreateCrafters();
    }

    private void CreateSotrages()
    {
        Item[] storedItems = new Item[] {
            ItemLibrary.Instance.LapizPref,
            ItemLibrary.Instance.IronOrePref,
            ItemLibrary.Instance.IronIngotPref
        };

        for (int i = 0; i < storedItems.Length; i++)
        {
            Storage storage = PhotonNetwork.InstantiateRoomObject(
                StoragePref.name,
                StoragePositions + (Vector3.left * 2 * i),
                Quaternion.identity).GetComponent<Storage>();
            storage.InitItem(storedItems[i]);
        }
    }

    private void CreateCrafters()
    {
        foreach (Vector3 pos in FurnacePositions)
        {
            Furnace furnace = PhotonNetwork.InstantiateRoomObject(FurcanePref.name, pos, Quaternion.identity)
                .GetComponent<Furnace>();

        }

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
