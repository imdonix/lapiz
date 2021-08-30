using System;
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
    public string Consume => "Consume";
}

