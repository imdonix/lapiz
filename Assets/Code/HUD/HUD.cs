using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    private static List<Display> cache;

    [Header("Components")]
    [SerializeField] public LiveStateDisplay LiveState;
    [SerializeField] public ActionDisplay Action;
    [SerializeField] public StoryDisplay Story;
    [SerializeField] public GameOverDisplay GameOver;
    [SerializeField] public ItemLibraryDisplay ItemLibrary;

    #region UNITY

    private void Awake()
    {
        CacheDisplays(this);
        Instance = this;
    }

    #endregion

    public void SwitchPlayerOverlay()
    {
        OpenAll(LiveState, Action, Story, ItemLibrary);
    }

    public void SwitchFreecamOverlay()
    {
        OpenAll(Story);
    }

    public void SwitchGameOverOverlay(int level)
    {
        OpenAll(GameOver);
    }


    private void CloseAll()
    {
        foreach (Display dp in HUD.cache)
            dp.Close();
    }

    private void OpenAll(params Display[] displays)
    {
        CloseAll();
        foreach (Display dp in displays)
            dp.Open();
    }


    private static void CacheDisplays(HUD instance)
    {
        HUD.cache = new List<Display>();
        foreach (var field in typeof(HUD).GetFields())
        {
            object obj = field.GetValue(instance);
            if(obj is Display)
                HUD.cache.Add((Display) obj);
        }
            
    }
}
