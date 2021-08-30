using Photon.Pun;


public class Lapiz : Item, IConsumable
{
    private const float LIFE_TIME = 60 * 5;
    private const int CHAKRA_AMOUNT = 10;

    protected override float GetLifeTime()
    {
        return LIFE_TIME;
    }

    public override string GetName()
    {
        return Manager.Instance.GetLanguage().Lapiz;
    }

    public void Consume(Ninja source)
    {
        source.IncreaseMaxChakra(CHAKRA_AMOUNT);
        DestroyItem();
    }

    public string GetReward()
    {
        return Manager.Instance.GetLanguage().Chakra;
    }
}
