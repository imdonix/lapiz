    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Manager : MonoBehaviourPunCallbacks
{
    public const string ROOM = "lapiz";

    public static Manager Instance;
   

    [Header("Components")]
    [SerializeField] public FreeCam FreeCamera;

    [Header("Prefhabs")]
    [SerializeField] public Player PlayerPref;
    [SerializeField] public Shuriken ShurikenPref;

    [Header("Settings")]
    [SerializeField] public Vector3 StartPosition;

    private Player CurrentPlayerObject;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
    }


    #region PHOTON

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PUN] Connected to master");
        PhotonNetwork.JoinOrCreateRoom(ROOM, GetDefault(), TypedLobby.Default);
        Manager.Instance.FreeCamera.EnableFreeCam();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(string.Format("[PUN] Connected to room ({0})", PhotonNetwork.CurrentRoom.PlayerCount));

        Player myself = PhotonNetwork.Instantiate(PlayerPref.name, StartPosition, Quaternion.identity).GetComponent<Player>();
        myself.TakeControll();
        CurrentPlayerObject = myself;
    }


    private static RoomOptions GetDefault()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 5;
        return opt;
    }

    #endregion
}
