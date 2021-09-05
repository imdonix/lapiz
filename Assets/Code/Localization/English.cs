﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class English : MonoBehaviour, ILanguage
{
    public static English I = new English();


    public string PickUp => "Pick up";
    public string Lapiz => "Lapiz Crystal";
    public string ThrowAway => "Throw away";
    public string Chakra => "Chakra";
    public string Use => "Consume";
    public string IronOre => "Iron ore";
    public string IronIngot => "Iron ingot";
    public string Furnace => "Furnace";
    public string LapizIngot => "Lapiz ingot";
    public string ChakraMoon => "Chakra moon";
    public string Equipment => "Equipment";
    public string HighBlade => "High blade";
    public string NatureSword => "Nature sword";
    public string Shuriken => "Shuriken";
    public string Sword => "Sword";
    public string Axe => "Axe";
    public string PickAxe => "Pickaxe";
}

