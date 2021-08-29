using Photon.Pun;


public class Lapiz : Item
{
    private const float LIFE_TIME = 60 * 5; 

    protected override float GetLifeTime()
    {
        return LIFE_TIME;
    }

}
