using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BoomerangBullets")]
public class BoomerangBullets : BulletAttachmentModifier
{
    public BoomerangBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }
}
