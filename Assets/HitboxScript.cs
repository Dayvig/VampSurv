using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HitboxScript : MonoBehaviour
{
    public Collider2D thisCollider;
    public List<BaseEnemy> immuneEnemies = new List<BaseEnemy>();

    public virtual void Reset()
    {
        immuneEnemies.Clear();
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            BaseEnemy b = other.gameObject.GetComponent<BaseEnemy>();
            if (!immuneEnemies.Contains(b))
            {
                b.touchHitbox(this);
                immuneEnemies.Add(b);
            }
        }
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            BaseEnemy b = other.gameObject.GetComponent<BaseEnemy>();
            if (!immuneEnemies.Contains(b))
            {
                b.touchHitbox(this);
                immuneEnemies.Add(b);
            }
        }
    }

    public virtual void hitboxUpdate()
    {

    }
}
