using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static Camera selected;
    public static HUD Instance;
    private static List<Display> cache;

    [Header("Tags")]
    [SerializeField] public Canvas Tags;
    [SerializeField] public EntityTag TagPref;

    [Header("Components")]
    [SerializeField] public LiveStateDisplay LiveState;
    [SerializeField] public ActionDisplay Action;
    [SerializeField] public StoryDisplay Story;
    [SerializeField] public GameOverDisplay GameOver;
    [SerializeField] public ItemLibraryDisplay ItemLibrary;
    [SerializeField] public CraftingDisplay Craft;


    #region UNITY

    private void Awake()
    {
        CacheDisplays(this);
        Instance = this;
    }

    #endregion

    public void SwitchPlayerOverlay()
    {
        SetCursor(false);
        OpenAll(LiveState, Action, Story);
    }

    public void SwitchFreecamOverlay()
    {
        SetCursor(false);
        OpenAll(Story);
    }

    public void SwitchGameOverOverlay(int level)
    {
        SetCursor(true);
        OpenAll(GameOver);
    }

    public void ToggleItemLibrary()
    {
        if (!ItemLibrary.isActiveAndEnabled)
        {
            SetCursor(true);
            OpenAll(ItemLibrary);
        }
        else
            SwitchPlayerOverlay();
    }

    private void SetCursor(bool enabled)
    {
#if UNITY_EDITOR
#else
        Cursor.visible = enabled;
        Cursor.lockState = enabled ? CursorLockMode.Confined : CursorLockMode.Locked;
        
#endif
    }

    public void ShowCraftRecipe(ICraftable craftable)
    {
        Craft.Render(craftable);
        OpenAll(Craft);
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
