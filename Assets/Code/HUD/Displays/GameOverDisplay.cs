using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDisplay : Display
{

    [SerializeField] public Text GameOver;
    [SerializeField] public Text BackToMenu;

    protected override void OnInit()
    {
        ILanguage lang = Manager.Instance.GetLanguage();
        GameOver.text = lang.GameOver;
        BackToMenu.text = lang.BackToMenu;
    }

    #region UI

    public void OnMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    #endregion
}
