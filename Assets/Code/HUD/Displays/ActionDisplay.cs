using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ActionDisplay : MonoBehaviour
{
    [SerializeField] private Text Action;


    public void Show(IInteractable interactable, Item hand)
    {
        StringBuilder builder = new StringBuilder();
        if (!ReferenceEquals(interactable, null))
            if (interactable.CanInteract())
                builder.Append(string.Format("[{0}] {1}\n",
                    Settings.Instance.Interact.ToString(),
                    interactable.GetDescription()));

        if (!ReferenceEquals(hand, null))
        {
            builder.Append(string.Format("[{0}] {1} {2}\n",
                Settings.Instance.Throw.ToString(),
                Manager.Instance.GetLanguage().ThrowAway,
                hand.GetName()));

            if (hand is IConsumable)
                builder.Append(string.Format("[{0}] {1} (for {2})\n",
                    Settings.Instance.Consume.ToString(),
                    Manager.Instance.GetLanguage().Use,
                    (hand as IConsumable).GetReward()));

        }

        Action.text = builder.ToString();
    }
}

