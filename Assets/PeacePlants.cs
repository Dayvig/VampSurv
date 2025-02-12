using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/PeacePlants")]
public class PeacePlants : Modifier
{
    private float plantTimer = 0.0f;
    PlantHitbox newPlantHitbox;
    public GameObject hitboxObject;

    public PeacePlants(PlayerCharacter newOwner) : base(newOwner)
    {
    }

    public override void Apply()
    {
        base.Apply();
        plantTimer += Time.deltaTime;
        if (plantTimer > factors[0])
        {
            createNewPlant();
            plantTimer -= factors[0];
        }
        owner.finalBulletDamage *= factors[2];
    }

    void createNewPlant()
    {
        for (int i = 0; i < HitboxManager.instance.inactiveHitboxes.Count; i++)
        {
            if (HitboxManager.instance.inactiveHitboxes[i] is PlantHitbox)
            {
                newPlantHitbox = (PlantHitbox)HitboxManager.instance.inactiveHitboxes[i];
                newPlantHitbox.duration = factors[1];
                newPlantHitbox.transform.position = EnemyManager.instance.player.transform.position + (Vector3)(Random.insideUnitCircle * EnemyManager.instance.player.finalEffectRange);
                newPlantHitbox.Reset();

                HitboxManager.instance.activeHitboxes.Add(newPlantHitbox);
                HitboxManager.instance.inactiveHitboxes.Remove(newPlantHitbox);
                return;
            }
        }
        GameObject newPlantHitboxObject = HitboxManager.instance.InstantiateNewHitbox(hitboxObject);

        newPlantHitboxObject.transform.position = EnemyManager.instance.player.transform.position + (Vector3)(Random.insideUnitCircle * EnemyManager.instance.player.finalEffectRange);
        newPlantHitbox = newPlantHitboxObject.GetComponent<PlantHitbox>();   
        if (newPlantHitbox != null)
        {
            newPlantHitbox.duration = factors[1];
            newPlantHitbox.Reset();
            HitboxManager.instance.activeHitboxes.Add(newPlantHitbox);
        }
        else
        {
            Debug.Log("Attempted to instantiate a windhitbox without hitboxScript");
        }

    }
}
