using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float FOV;
    [SerializeField] public KeyCode Jump;
    [SerializeField] public KeyCode Sprint;
    [SerializeField] public KeyCode Cast;
    [SerializeField] public KeyCode Interact;
    [SerializeField] public KeyCode Throw;
    [SerializeField] public KeyCode Consume;
    [SerializeField] public KeyCode Ready;

    public static Settings Instance;


    private void Awake()
    {
        Instance = this;
    }

}

