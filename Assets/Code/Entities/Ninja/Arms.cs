using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Arms : MonoBehaviour
{
    private const float SEAL_TIME = .15F;
    private const float PUTAWAY_TIME = .35F;
    private const float THROW_FORCE = 400F;

    [Header("Property")]
    [SerializeField] private Vector3 IdlePosition;
    [SerializeField] private Vector3 PutAwayPosition;
    [SerializeField] private Vector3 DefendPosition;

    [Header("Components")]
    [SerializeField] private Hand Left;
    [SerializeField] private Hand Right;

    [Header("Equipments")]
    [Header("Weapon")]
    [SerializeField] public HandSword BaseSword;
    [SerializeField] public HandShuriken Shuriken;
    [SerializeField] public HandChakraMoon ChakraMoon;
    [SerializeField] public HandHighBlade HighBlade;
    [SerializeField] public HandNatureSword NatureSword;

    [Header("Tool")]
    [SerializeField] public HandAxe Axe;
    [SerializeField] public HandPickaxe Pickaxe;

    [Header("Debug")]
    [SerializeField] private int slot = -1;
    [SerializeField] private float Cooldown = 0;

    private Weapon[] WeaponSlots;

    private Ninja owner;
    private SealStorage storage;

    private bool idle = false;
    private bool isMine = false;
    private Item item = null;

    #region UNITY

    private void Awake()
    {
        owner = gameObject.GetComponentInParent<Ninja>();
        storage = new SealStorage();
        LoadSlots();
    }

    private void Start()
    {
        if (isMine) 
        {
            Idle();
        }
    }

    private void Update()
    {
        if (isMine)
        {
            Cooldown -= Time.deltaTime;
            if (Cooldown < -3.5F && !idle)
                Idle();
            PositionItem();
        }
    }

    #endregion

    public void Attack()
    {
        if (Cooldown > 0) return;
        if (slot < 0) return;
        Weapon weapon = GetSlot(slot);
        Cooldown = weapon.GetSpeed() + .1F;
        StartCoroutine(Continue());

        IEnumerator Continue()
        {
            var next = weapon.GetNext();
            Right.MoveTo(next.Item1, next.Item2, weapon.GetSpeed());
            yield return new WaitForSeconds(weapon.GetSpeed() * weapon.GetDamageTime());
            weapon.Attack(owner, owner.GetLookDirection());
            idle = false;
        }
    }

    public void Harvest(HarvestTask task)
    {
        if (Cooldown > 0) return;
        if (slot < 0) return;
        Weapon weapon = GetSlot(slot);
        Cooldown = weapon.GetSpeed() + .1F;
        StartCoroutine(Continue());

        IEnumerator Continue()
        {
            var next = weapon.GetNext();
            Right.MoveTo(next.Item1, next.Item2, weapon.GetSpeed());
            yield return new WaitForSeconds(weapon.GetSpeed() * weapon.GetDamageTime());
            if (task.GetHarvestable().Harvest(owner,(HandTool) weapon, out Item item))
            {
                task.End(item);
            }
            idle = false;
        }
    }

    public bool Defend(bool active)
    {
        if (Cooldown > 0 || slot < 0 || !active) return false;

        Weapon weapon = GetSlot(slot);
        Right.Set(DefendPosition);
        idle = false;
        return active;
    }

    public void Swap(int next)
    {
        if (Cooldown > 0) return;

        ThrowAway();
        Weapon weapon = next < 0 ? null : GetSlot(next);
        Cooldown = PUTAWAY_TIME + .05F + (weapon != null ? weapon.GetEquipTime() : 0);
        StartCoroutine(Continue());

        IEnumerator Continue()
        {
            Right.MoveTo(PutAwayPosition, PUTAWAY_TIME);
            yield return new WaitForSeconds(PUTAWAY_TIME);

            if (slot >= 0)
            {
                GetSlot(slot).gameObject.SetActive(false);
                slot = -1;
            }

            if (next < 0) yield break;

            weapon.gameObject.SetActive(true);
            slot = next;
            Right.MoveTo(weapon.GetFirstPos(), weapon.GetEquipTime());
            idle = false;
        }
    }

    public void ShowSeal(HandSeal seal)
    {
        if (Cooldown > 0) return;
        if (ReferenceEquals(seal, HandSeal.NONE)) return;

        Cooldown = SEAL_TIME;
        Left.MoveTo(Left.GetSealRotation(seal), SEAL_TIME);
        Right.MoveTo(Right.GetSealRotation(seal), SEAL_TIME);
        idle = false;

        storage.Store(seal);
    }

    public void CastJutsu()
    {
        if (Cooldown > 0) return;
        
        Jutsu jutsu;
        if (JutsuLibrary.Instance.ExistActivation(storage.Read(), out jutsu))
        {
            if (owner.GetChakra() < jutsu.GetCost())
            {
                Debug.Log("You are out of chakra");
                //TODO POP OUT OF CHAKRA
            }
            else
            {
                Cooldown = SEAL_TIME;
                storage.Reset();
                owner.Cast(jutsu);
                owner.SpendChakra(jutsu.GetCost());
                Idle();
            }
        }
    }

    public void PickUpItem(Item item)
    {
        ThrowAway();

        SetSlot(-1);
        Left.MoveTo(Left.GetHandItemRotation(), SEAL_TIME);
        Right.MoveTo(Right.GetHandItemRotation(), SEAL_TIME);
        Cooldown = SEAL_TIME;
        idle = true;

        this.item = item;
    }

    public void ThrowAway()
    {
        if (!ReferenceEquals(this.item, null)) 
        {
            item.ThrowAway(owner.GetHead().transform.forward * THROW_FORCE + owner.GetVelocity());
            this.item = null;
            Idle();
        }
    }

    public void ThrowAway(Item item)
    {
        if (!ReferenceEquals(this.item, null))
        {
            item.ThrowAway(owner.GetHead().transform.forward * THROW_FORCE + owner.GetVelocity());
            this.item = null;
            Idle();
        }
    }

    public void Consume()
    {
        if (ReferenceEquals(this.item, null)) return;
        if (this.item is IConsumable)
        {
            IConsumable consumable = this.item as IConsumable;
            consumable.Consume(owner);
            this.item = null;
        }

        Idle();
    }

    public void SetSlot(int next)
    {
        if (slot == next) return;
        if (slot >= 0) GetSlot(slot).gameObject.SetActive(false);
        if (next >= 0) GetSlot(next).gameObject.SetActive(true);

        slot = next;
    }

    public int GetSlot()
    {
        return slot;
    }

    public Item GetItemInHand()
    {
        return item;
    }

    public Weapon GetWeaponInHand()
    {
        return GetSlot(slot);
    }

    public void Claim()
    {
        isMine = true;
        Right.Claim();
        Left.Claim();
    }

    public Weapon[] GetEquipmentSlots()
    {
        return WeaponSlots;
    }

    #region PRIVATE

    private void Idle()
    {
        Right.MoveTo(IdlePosition, 1.25F);
        Left.MoveTo(IdlePosition, 1.25F);
        idle = true;
    }

    private Weapon GetSlot(int slot)
    {
        if (slot >= 0)
            return WeaponSlots[slot];
        else
            return null;
    }

    private void PositionItem()
    {
        if (ReferenceEquals(item, null)) return;

        item.transform.position = Right.GetItemHolder().position;
        item.transform.rotation = owner.transform.rotation;
    }

    private void LoadSlots()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (FieldInfo info in typeof(Arms).GetFields())
        {
            object obj = info.GetValue(this);
            if (obj is Weapon) weapons.Add((Weapon) obj);
        }

        WeaponSlots = weapons.ToArray();
    }

    #endregion
}
