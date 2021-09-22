using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Backstone : Item, ICraftable
{
    private const string ID = "backstone";
    private const float CHANGE = 8;


    private Gradient gradient;
    private MeshRenderer render;
    private float state;

    #region UNITY

    protected override void Awake()
    {
        base.Awake();
        this.render = GetComponent<MeshRenderer>();
        InitGradient();
    }

    protected override void Update()
    {
        base.Update();
        if (photonView.IsMine)
        {
            state += Time.deltaTime / 8;
            Render();
        }
    }

    #endregion

    private void InitGradient()
    {
        Material mat = render.material;
        gradient = new Gradient();

        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = mat.color;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.cyan;
        colorKey[1].time = 1.0f;

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.75f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }

    private void Render()
    {
        render.material.color = gradient.Evaluate(state % 1);
    }

    public override string GetID()
    {
        return ID;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Backstone;
    }

    public override Item GetItemPref()
    {
        return ItemLibrary.Instance.BackstonePref;
    }

    public Recipe GetRecipe()
    {
        Recipe recipe = Recipe.Create(1);
        recipe.Add(ItemLibrary.Instance.BackstoneOrePref, 2);
        return recipe;
    }

    public Crafter GetCrafterPrefhab()
    {
        return World.Loaded.FurcanePref;
    }

    public float GetTime()
    {
        return 4.5F;
    }

    #region SERIALIZATION

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        base.OnPhotonSerializeView(stream, info);

        if (stream.IsReading)
        {
            state = (float)stream.ReceiveNext();
            Render();
        }
        else
        {
            stream.SendNext(state);
        }
    }

    #endregion
}

