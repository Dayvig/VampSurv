using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/EggBullets")]
public class EggBullets : BulletAttachmentModifier
{
    public EggBullets(PlayerCharacter newOwner) : base(newOwner)
    {
        
    }

    public override void Apply()
    {
        base.Apply();
        foreach (Bullet b in payloads)
        {
            owner.finalPayloads.Add(b);
        }
    }
}
