using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float FOV;


    public static Settings Instance;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

}

