using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AeroBullets")]
public class AeroBullets : BulletAttachmentModifier
{
    public AeroBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }

}
