using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityTag : MonoBehaviour
{

    [Header("Tag")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text badgeText;
    [SerializeField] private GameObject health;
    [SerializeField] private GameObject chakra;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform chakraBar;

    private Entity binded;

    public void Bind(Entity entity)
    {
        this.binded = entity;
        this.name = string.Format("-Tag for {0}-", entity.GetName());
    }

    #region UNITY

    private void Awake()
    {
        Disable();
    }

    private void Update()
    {
        if (IsEntityExist())
        {
            if (ReferenceEquals(Camera.main, null)) return;
            if (ReferenceEquals(HUD.selected, null)) return;

            Disable();
            TurnToCamera();
            binded.Update(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private bool IsEntityExist()
    {
        return binded != null;
    }

    private void TurnToCamera()
    {
        try
        {
            transform.position = binded.transform.position + Vector3.up * 4;
            transform.rotation = Quaternion.LookRotation(HUD.selected.transform.position - transform.position, Vector3.up);
        }
        catch (Exception) {}
    }

    private void Disable()
    {
        nameText.gameObject.SetActive(false);
        badgeText.gameObject.SetActive(false);
        health.SetActive(false);
        chakra.SetActive(false);
    }

    public void SetName(string name)
    {
        nameText.gameObject.SetActive(true);
        nameText.text = name;
    }

    public void SetBadge(string badge)
    {
        badgeText.gameObject.SetActive(true);
        badgeText.text = badge;
    }

    public void SetHealth(LivingEntity entity)
    {
        health.SetActive(true);
        healthBar.localScale = new Vector3(entity.GetHP() / entity.GetMaxHP(), 1 , 1);
    }

    public void SetChakra(LivingEntity entity)
    {
        if (NPlayer.Local.GetHead().GetEye() >= EyeType.Sharingan)
        {
            chakra.SetActive(true);
            chakraBar.localScale = new Vector3(entity.GetChakra() / entity.GetMaxChakra(), 1, 1);
        }
        else
            chakra.SetActive(false);
    }

}
