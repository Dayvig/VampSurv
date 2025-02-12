using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/SniperBullets")]
public class SniperBullets : Modifier
{
    public SniperBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        owner.finalBulletDamage *= (int)factors[0];
        owner.finalBulletSpeed *= (int)factors[1];
        owner.finalFiringSpeed *= (int)factors[2];
    }

}
