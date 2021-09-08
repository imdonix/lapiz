using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class TargetedTask : Task
{
    protected readonly LivingEntity target;

    protected TargetedTask(Ninja owner, LivingEntity target) : base(owner)
    {
        this.target = target;
    }

    protected override void DoFixed()
    {
        base.DoFixed();

        if (CheckDead()) return;

        owner.SetTargetLook(target.GetTargetedLookPosition());
    }

    protected bool CheckDead()
    {
        try
        {
            if (ReferenceEquals(target.gameObject, null))
            {
                Fail();
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            Fail();
            return true;
        }

    }
}

