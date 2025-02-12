using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttachmentModifier : Modifier
{
    public enum Key
    {
        BOOMERANG,
        EGG,
        AERO,
        BOUNCY,
        BOWLING,
        ACCEL,
        TORNADO
    }
    public Key key;
    public BulletAttachmentModifier(PlayerCharacter newOwner) : base(newOwner)
    {
    }
}
