using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoryDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    [SerializeField] private Sprite AttackPhase;
    [SerializeField] private Sprite IdlePhase;

    public void Show(bool attacking, float countDown, int remaining, bool ready)
    {
        image.sprite = attacking ? AttackPhase : IdlePhase;
        text.text = BuildText(attacking, countDown, remaining, ready);
    }

    private string BuildText(bool attacking, float countDown, int remaining, bool ready)
    {
        StringBuilder stringBuilder = new StringBuilder();
        TimeSpan result = TimeSpan.FromSeconds(Mathf.RoundToInt(countDown));
        stringBuilder.AppendLine(result.ToString("mm':'ss"));
        if (attacking)
            stringBuilder.AppendLine(string.Format("{0} : {1}", Manager.Instance.GetLanguage().Attackers, remaining));
        else
        {
            stringBuilder.AppendLine(string.Format("{0} : {1}", Manager.Instance.GetLanguage().Ready, remaining));
            if (!ready)
                stringBuilder.AppendLine(string.Format("[{0}] {1}", Settings.Instance.Ready.ToString(), Manager.Instance.GetLanguage().Ready, remaining));
        }



        return stringBuilder.ToString();
    }
}

