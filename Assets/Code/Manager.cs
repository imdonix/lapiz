using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviourPunCallbacks
{
    public const string ROOM = "lapiz";

    public static Manager Instance;

    [Header("Prefhabs")]
    [SerializeField] public NPlayer PlayerPref;
    [SerializeField] public Chunin ChuninPref;

    [Header("Settings")]
    [SerializeField] public Vector3 StartPosition;

    private World world;
    private ILanguage language;

    #region UNITY
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
        SetupLanguage();
    }

    private void Start()
    {
        StartMatchmaking();
    }

    #endregion

    public World GetWorld()
    {
        return world;
    }    

    public void StartMatchmaking()
    {
        //TODO
        PhotonNetwork.ConnectUsingSettings();
    }

    public ILanguage GetLanguage()
    {
        return language;
    }

    private void SetupLanguage()
    {
        language = English.I;
    }

    #region PHOTON

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PUN] Connected to master");
        PhotonNetwork.JoinOrCreateRoom(ROOM, GetDefault(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(string.Format("[PUN] Connected to room ({0})", PhotonNetwork.CurrentRoom.PlayerCount));

        //TODO
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }


    private static RoomOptions GetDefault()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 5;
        return opt;
    }

    #endregion
}
