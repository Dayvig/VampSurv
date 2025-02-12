using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/MachineGun")]
public class MachineGun : Modifier
{
    public MachineGun(PlayerCharacter newOwner) : base(newOwner)
    {
    }
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Notes = "This component shouldn't be removed, it does important stuff.";
    public override void Apply()
    {
        base.Apply();
        owner.finalFiringSpeed *= factors[0];
        owner.finalBulletDamage *= factors[1];
    }
}
