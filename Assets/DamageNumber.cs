using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{

    public float timer = 0.0f;
    public float spawnTime = 0.0f;
    public bool active = false;
    public TextMeshPro text;

    private void FixedUpdate()
    {
        if (active)
        {
            timer += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, (spawnTime - timer) / spawnTime);
            if (timer > spawnTime)
            {
                active = false;
                VfxManager.instance.markedForDeathDamageNumbers.Add(this);
            }
        }
    }

    public void Spawn(Vector2 location, float duration, Color color, int number)
    {
        foreach (DamageNumber damageNumber in VfxManager.instance.activeDamageNumbers)
        {
            if (Vector2.Distance(damageNumber.gameObject.transform.position, location) < 0.1f)
            {
                location += Random.insideUnitCircle.normalized * Random.Range(0.3f, 0.6f);
            }
        }
        transform.position = location;
        spawnTime = duration;
        timer = 0.0f;
        text.color = color;
        text.text = number.ToString();
        active = true;
        gameObject.SetActive(true);
    }
}
