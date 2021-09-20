using UnityEditor;
using UnityEngine;


public class Display : MonoBehaviour
{
    private bool first = true;

    public void Open()
    {
        gameObject.SetActive(true);

        if (first)
        {
            OnInit();
            first = false;
        }

        OnOpen();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnClose(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
    }

    protected virtual void OnOpen() {}

    protected virtual void OnClose(){ }

    protected virtual void OnInit() { }

    protected virtual void OnBack() { HUD.Instance.SwitchPlayerOverlay(); }
}
