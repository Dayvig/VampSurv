using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/TornadoBullets")]
public class TornadoBullets : BulletAttachmentModifier
{
    public TornadoBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }
}
