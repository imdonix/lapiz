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
    [SerializeField] private Text ActionText;

    private IInteractable interactable;
    private Item hand;

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
        this.interactable = interactable;
        this.hand = hand;

        ShowActionText();
    }

    private void ShowActionText()
    {


        StringBuilder builder = new StringBuilder();
        if(!ReferenceEquals(interactable, null)) 
            if(interactable.CanInteract())
                builder.Append(string.Format("[{0}] {1}\n", 
                    Settings.Instance.Interact.ToString(), 
                    interactable.GetDescription()));

        if (!ReferenceEquals(hand, null))
        {
            builder.Append(string.Format("[{0}] {1} {2}\n",
                Settings.Instance.Throw.ToString(),
                Manager.Instance.GetLanguage().ThrowAway,
                hand.GetName()));

            if(hand is IConsumable)
                builder.Append(string.Format("[{0}] {1} (for {2})\n",
                    Settings.Instance.Consume.ToString(),
                    Manager.Instance.GetLanguage().Use,
                    (hand as IConsumable).GetReward()));

        }

        ActionText.text = builder.ToString();
    }
}
