using Photon.Pun;
using System.Collections;
using UnityEngine;


public class World : MonoBehaviour
{
    [Header("World")]
    [SerializeField] private Vector3 PlayerStartPosition;

    public static World Loaded;

    private PathFinder pathFinder;

    #region UNITY

    private void Awake()
    {
        this.pathFinder = GetComponent<PathFinder>();

        Loaded = this;
    }

    private void Start()
    {
        InitPlayer();
        StartCoroutine(SpawnEnemy());
    }

    #endregion

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
        for (int i = 0; i < 50; i++)
        {
            PhotonNetwork.InstantiateRoomObject(Manager.Instance.ChuninPref.name, PlayerStartPosition + Vector3.up * 3, Quaternion.identity);
            yield return new WaitForSeconds(10);
        }
    }

}
