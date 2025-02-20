using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/WindBoost")]
public class WindBoost : Modifier
{
    public float windboostTimer = 0.0f;
    public bool active = true;
    public WindBoost(PlayerCharacter newOwner) : base(newOwner)
    { 
    }

    public override void Apply()
    {
        base.Apply();
        if (active)
        {
            owner.finalMoveSpeed *= factors[0];
            windboostTimer += Time.deltaTime;
            if (windboostTimer > factors[1])
            {
                owner.toDispose.Add(this);
                active = false;
            }
        }
    }
    public override void Setup()
    {
        base.Setup();
        windboostTimer = 0.0f;
        active = true;
    }
}
