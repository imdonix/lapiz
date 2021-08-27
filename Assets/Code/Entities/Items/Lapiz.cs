using Photon.Pun;


public class Lapiz : Item
{
    public override void Iteract(Player source)
    {
        //Pickup
    }

    protected override float GetLifeTime()
    {
        return 60F;
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

}
