using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour
{
    public const int BOTTOM = -5;

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
        if (AssertEditor()) return;
        InitPlayer();
        
        
        StartCoroutine(SpawnEnemy());
    }

    #endregion

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
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(2);
            PhotonNetwork.InstantiateRoomObject(Manager.Instance.ChuninPref.name, PlayerStartPosition + Vector3.up * 3, Quaternion.identity);

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
