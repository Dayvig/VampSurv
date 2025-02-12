using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BowlingBullets")]
public class BowlingBullets : BulletAttachmentModifier
{
    public BowlingBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        owner.finalBulletScale *= factors[1];
        owner.finalBulletSpeed *= factors[2];
    }

}
