using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "SwordAnimation", order = 1)]
public class SwordAnimation : GameplayAnimation
{
    public GameObject owner;
    public List<Vector3> basePositions = new List<Vector3>();
    public List<Vector3> baseRotations = new List<Vector3>();
    public List<Vector3> baseScales = new List<Vector3>();

    public SwordAnimation MakeCopy()
    {
        return new SwordAnimation
        {
            positions = this.positions,
            rotations = this.rotations,
            durations = this.durations,
            specialActivationTimings = this.specialActivationTimings,
            arcLengths = this.arcLengths,
            hitboxes = this.hitboxes,
            hitboxObjects = this.hitboxObjects,
            basePositions = this.basePositions,
            baseRotations =this.baseRotations,
            baseScales = this.baseScales,
        };
    }

    public List<GameObject> InstantiateHitboxes()
    {
        List<GameObject> toReturn = new List<GameObject>();
        GameObject newHitbox;
        if (owner == null)
        {
            return null;
        }

        hitboxes[0].transform.localPosition = basePositions[0];
        hitboxes[0].transform.localRotation = Quaternion.Euler(baseRotations[0]);
        hitboxes[0].transform.localScale = baseScales[0];
        newHitbox = Instantiate(hitboxes[0], owner.transform);
        newHitbox.SetActive(false);
        toReturn.Add(newHitbox);

        hitboxes[1].transform.localPosition = basePositions[1];
        hitboxes[1].transform.localRotation = Quaternion.Euler(baseRotations[1]);
        hitboxes[1].transform.localScale = baseScales[1];
        newHitbox = Instantiate(hitboxes[1], owner.transform);
        newHitbox.SetActive(false);
        toReturn.Add(newHitbox);

        return toReturn;
    }

    public override void activateSpecialEffects(int timing, float angle, List<GameObject> hitboxesToUse, GameObject owner)
    {
        switch (timing)
        {
            case 0:
                hitboxesToUse[0].transform.localPosition = PlayerCharacter.RotateAround(basePositions[0], Vector2.zero, angle);
                hitboxesToUse[0].transform.localRotation = Quaternion.Euler(0f, 0f, baseRotations[0].z + angle);
                hitboxesToUse[0].SetActive(true);
                break;
            case 1:
                hitboxesToUse[1].transform.localPosition = PlayerCharacter.RotateAround(basePositions[1], Vector2.zero, angle);
                hitboxesToUse[1].transform.localRotation = Quaternion.Euler(0f, 0f, baseRotations[1].z + angle);
                hitboxesToUse[1].SetActive(true);
                break;
            case 2:
                hitboxesToUse[0].SetActive(false);
                hitboxesToUse[1].SetActive(false);
                break;
        }
    }

}
