using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [Header("Hand")]
    // We to only the end of the roatation coz it can be used with items in hand
    [SerializeField] private Vector3[] SealsRotation;
    [SerializeField] private Vector3 ItemRotation;

    [SerializeField] private Transform ItemHolder;

    private Vector3 start;
    private Vector3 target;
    private float time;
    private float state;

    private bool isMine = false;


    private void Update()
    {
        if (!isMine) return;

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(start), Quaternion.Euler(target), state);
        state += Time.deltaTime / time;
    }

    public void MoveTo(Vector3 from, Vector3 target, float time)
    {
        this.start = from;
        this.target = target;
        this.time = time;
        this.state = 0;
    }

    public void MoveTo(Vector3 target, float time)
    {
        this.start = transform.localRotation.eulerAngles;
        this.target = target;
        this.time = time;
        this.state = 0;
    }

    public void Set(Vector3 target)
    {
        this.start = target;
        this.target = target;
        this.time = this.state = 1F;
    }

    public void Claim() 
    {
        isMine = true;
    }

    public Vector3 GetSealRotation(HandSeal seal)
    {
        return SealsRotation[(int) seal];
    }

    public Vector3 GetHandItemRotation()
    {
        return ItemRotation;
    }

    public Transform GetItemHolder()
    {
        return ItemHolder;
    }

}
