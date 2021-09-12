using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    [Header("Components")]
    [SerializeField] private LiveStateDisplay LiveState;
    [SerializeField] private ActionDisplay Action;
    [SerializeField] private StoryDisplay Story;

    #region UNITY

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void UpdateStatus(float hp, float hpMax, float chakra, float chakraMax)
    {
        LiveState.UpdateStatus(hp, hpMax, chakra, chakraMax);
    }

    public void Show(IInteractable interactable, Item hand)
    {
        Action.Show(interactable, hand);
    }

    public void UpdateStory(bool attacking, float countDown, int remaining, bool ready)
    {
        Story.Show(attacking, countDown, remaining, ready);
    }

    public void SwitchPlayerOverlay()
    {

    }

    public void SwitchFreecamOverlay()
    {

    }
}
