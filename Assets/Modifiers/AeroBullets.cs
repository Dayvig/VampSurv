using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/AeroBullets")]
public class AeroBullets : BulletAttachmentModifier
{
    public float spawnWindboxTimer = 0.0f;
    public float spawnInterval = 1f;
    public AeroBullets(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        Debug.Log(spawnWindboxTimer);
        if (spawnWindboxTimer < spawnInterval)
        {
            spawnWindboxTimer += Time.deltaTime;
        }
        Debug.Log(spawnWindboxTimer + " 2 ");
    }
}
