using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Chaingun")]
public class Chaingun : Modifier
{    public Chaingun(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        if (!owner.bonusFiringSpeedMults.ContainsKey("Chaingun"))
        {
            owner.bonusFiringSpeedMults.Add("Chaingun", 1f);
        }
        owner.finalFiringSpeed *= (1 / owner.bonusFiringSpeedMults["Chaingun"]);
        if (owner.finalWeaponHeat == -1) { owner.finalWeaponHeat = 0f; }
        owner.finalWeaponHeat += factors[1];
        if (owner.finalCoolingTime == -1) { 
            owner.finalCoolingTime = 0f;
            owner.finalCoolingTime += factors[2];
        }
        else
        {
            owner.finalCoolingTime /= 1.5f;
        }
    }

    public override void OnFire()
    {
        base.OnFire();
        Debug.Log("triggering on fire");
        owner.bonusFiringSpeedMults["Chaingun"] += factors[0];
    }

    public override void OnCoolingReset()
    {
        base.OnCoolingReset();
        owner.bonusFiringSpeedMults["Chaingun"] = 1f;
    }

}
