using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float FOV;


    public static Settings Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(transform.parent.gameObject);
    }

}

