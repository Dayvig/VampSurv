using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BouncyBullets")]
public class BouncyBullets : BulletAttachmentModifier
{
    public BouncyBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        owner.finalBulletHP += (int)factors[1];
    }

}
