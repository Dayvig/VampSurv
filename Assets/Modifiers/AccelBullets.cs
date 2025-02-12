using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AccelBullets")]
public class AccelBullets : BulletAttachmentModifier
{
    public AccelBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }
}
