using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class Tool : Item, IConsumable
{
    private const float LIFETIME = 10 * 60F;

    public void Consume(Ninja source)
    {
        throw new NotImplementedException();
    }

    public string GetReward()
    {
        return Manager.Instance.GetLanguage().Equipment;
    }

    protected override float GetLifeTime()
    {
        return LIFETIME;
    }
}

