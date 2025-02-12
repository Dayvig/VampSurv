using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : BulletAttachment
{
    public bool triggered = false;
    public PlayerCharacter character;
    public Egg(Bullet b) : base(b)
    {
        triggered = false;
        type = Type.ONDEATH;
        character = null;
    }

    public override void Apply()
    {
        List<Bullet> newSpawnedBullets = new List<Bullet>();
        int numBullets = owner.payloadCount;
        if (numBullets <= 0)
        {
            return;
        }
        float angle = LookAtPoint(owner.transform.position) + 270;
        if (numBullets == 1)
        {
            newSpawnedBullets.Add(character.SpawnBullet(angle, owner.transform.position, true));
            return;
        }
        else
        {
            int initial = 1 - (numBullets % 2);
            for (int s = initial; s < numBullets + initial; s++)
            {
                float angleOffSet =
                    ((((float)s / (numBullets + (1 - (2 * (numBullets % 2))))) * 180) -
                     (180 / 2));
                newSpawnedBullets.Add(character.SpawnBullet(angle + angleOffSet, owner.transform.position, true));
            }
        }
        foreach (Bullet b in newSpawnedBullets)
        {
            if (owner.lastHitEnemy != null)
            {
                owner.lastHitEnemy.immuneBullets.Add(b);
            }
        }
    }

    private float LookAtPoint(Vector3 current)
    {
        float angleDeg = (Mathf.Rad2Deg * Mathf.Atan2((current.y + owner.velocity.y) - current.y, (current.x + owner.velocity.x) - current.x));
        return angleDeg;
    }

    public override void Setup()
    {
    }
}
