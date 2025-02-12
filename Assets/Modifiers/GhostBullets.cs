using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/GhostBullets")]
public class GhostBullets : Modifier
{
    public GhostBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }
    [TextArea]
    [Tooltip("Doesn't do anything. Just comments shown in inspector")]
    public string Notes = "This component shouldn't be removed, it does important stuff.";
    public override void Apply()
    {
        base.Apply();
        owner.finalBulletHP += (int)factors[0];
        owner.finalBulletDamage *= factors[1];
    }
}
