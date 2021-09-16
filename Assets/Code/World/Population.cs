using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Population
{
    private const int MANGEKYO_TRIGGER = 5;

    private readonly List<NPlayer> players;
    private readonly List<Villager> villagers;
    private readonly Story story;

    private int maxPopulation = 0;
    private bool hasPopulated = false;

    public Population(Story story)
    {
        this.story = story;
        this.players = new List<NPlayer>();
        this.villagers = new List<Villager>();
    }

    public void Populate(LivingEntity entity)
    {
        if (entity is NPlayer)
            players.Add((NPlayer)entity);
        else if (entity is Villager)
            villagers.Add((Villager)entity);
        else
            throw new Exception("You can only Populate: Players or Villagers");

        maxPopulation = Math.Max(maxPopulation, GetPopulation());
        hasPopulated = true;
    }

    public void RegisterDead(LivingEntity entity)
    {
        if (entity is NPlayer)
            players.Remove((NPlayer)entity);
        else if (entity is Villager)
            villagers.Remove((Villager)entity);
        else
            throw new Exception("You can only Populate: Players or Villagers");
    }

    public int GetPopulation()
    {
        return players.Count + villagers.Count;
    }

    public int GetVillagerCount()
    {
        return villagers.Count;
    }

    public int GetMaxPopulation()
    {
        return maxPopulation;
    }

    public void AwakeMangekyo()
    {
        if (villagers.Count == 0 && players.Count == 1 && maxPopulation >= MANGEKYO_TRIGGER)
        {
            NPlayer player = players[0];
            if(player.GetHead().HasEye(EyeType.Sharingan))
                if (!player.GetHead().HasEye(EyeType.Mangekyo))
                    player.photonView.RPC("AwakeEye", player.photonView.Owner, EyeType.Mangekyo);
        }
    }

    public bool IsDead()
    {
        return GetPopulation() == 0 && hasPopulated;
    }
}
