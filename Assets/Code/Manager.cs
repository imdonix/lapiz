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

    [Header("Core")]
    [SerializeField] public NPlayer PlayerPref;
    [SerializeField] public Story Story;

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

        DisableCursor();
        PhotonNetwork.LoadLevel("Main");
    }

    private void DisableCursor()
    {
#if UNITY_EDITOR
#else
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }

    private static RoomOptions GetDefault()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 5;
        return opt;
    }

    #endregion
}
