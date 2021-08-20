using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    private const float ANGLE = 40F;
    private const float SPRINT = 55F;

    [SerializeField] public Transform Left;
    [SerializeField] public Transform Right;

    private float state = 0;
    private float target = 0;
    private float idle = 1F;
    private bool sprint = false;

    private bool isMine = false;

    private void Update()
    {
        if (!isMine) return;

        Left.localRotation = Quaternion.Euler(Mathf.Lerp(90 - (sprint ? SPRINT : ANGLE) , 90 + (sprint ? SPRINT : ANGLE), ( target+1)/2), 0, 0);
        Right.localRotation = Quaternion.Euler(Mathf.Lerp(90 - (sprint ? SPRINT : ANGLE), 90 + (sprint ? SPRINT : ANGLE), (-target+1)/2), 0, 0);

        idle += Time.deltaTime;
        if (idle > 0.15F) target = state = 0;
    }

    public void Forward(float amount, bool sprint, bool grounded)
    {
        if (Mathf.Abs(amount) < float.Epsilon) return;

        state += amount * (sprint ? 1.75F : 1.35F);
        float s = Mathf.Sin(state);
        this.target = grounded ? s : target;
        this.idle = 0;
        this.sprint = sprint;
    }

    public void Claim()
    {
        isMine = true;
    }

}
