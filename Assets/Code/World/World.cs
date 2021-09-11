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

    public static World Loaded;

    [Header("World")]
    [SerializeField] private Vector3 PlayerStartPosition;
    [SerializeField] private List<Vector3> IronOreVainPositions;

    [Header("Resources")]
    [SerializeField] public IronOreVain IronOreVainPref;

    [Header("Machines")]
    [SerializeField] public Thrower ThrowerPref;
    [SerializeField] public Furnace FurcanePref;


    private PathFinder pathFinder;
    private List<Machine> machines;

    private FreeCam freeCam;

    private float machineUpdateTimer = 0;

    #region UNITY

    private void Awake()
    {
        this.pathFinder = GetComponent<PathFinder>();
        this.machines = new List<Machine>();
        this.freeCam = GetComponentInChildren<FreeCam>();

        Loaded = this;
    }

    private void Start()
    {
        if (AssertEditor()) return;
        if (PhotonNetwork.IsMasterClient)
        {
            CreateWorld();
        }

        pathFinder.ReloadTerrain();

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

        //DEBUG

        PhotonNetwork.InstantiateRoomObject(Manager.Instance.ChuninPref.name, PlayerStartPosition, Quaternion.identity);
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
