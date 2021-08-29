using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;

public class Manager : MonoBehaviourPunCallbacks
{
    public const string ROOM = "lapiz";

    public static Manager Instance;
   
    [Header("Components")]
    [SerializeField] public FreeCam FreeCamera;

    [Header("Prefhabs")]
    [SerializeField] public NPlayer PlayerPref;
    [SerializeField] public Chunin ChuninPref;

    [Header("Settings")]
    [SerializeField] public Vector3 StartPosition;

    private NPlayer CurrentPlayerObject;
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
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    public ILanguage GetLanguage()
    {
        return language;
    }

    private void SetupLanguage()
    {
        language = English.I;
    }

    private IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < 50; i++)
        {
            PhotonNetwork.InstantiateRoomObject(ChuninPref.name, StartPosition + Vector3.up * 3, Quaternion.identity);
            yield return new WaitForSeconds(10);
        }
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

        NPlayer myself = PhotonNetwork.Instantiate(PlayerPref.name, StartPosition, Quaternion.identity).GetComponent<NPlayer>();
        myself.TakeControll();
        CurrentPlayerObject = myself;

        StartCoroutine(SpawnEnemy());
    }


    private static RoomOptions GetDefault()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 5;
        return opt;
    }

    #endregion
}
