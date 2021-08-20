using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SealStorage : List<HandSeal>
{

    public SealStorage() : base() { Reset(); }

    public void Store(HandSeal seal) 
    {
        Insert(0, seal);
    }

    public HandSeal[] Read() 
    {
        return ToArray();
    }

    public void Reset() 
    {
        Clear();

        for (int i = 0; i < 5; i++) 
            Store(HandSeal.NONE);
    }

}
