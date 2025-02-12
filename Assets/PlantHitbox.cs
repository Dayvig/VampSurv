using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHitbox : TimedHitbox
{
    SpriteRenderer ren;
    CircleCollider2D circleCollider;
    public List<Bullet> immuneBullets = new List<Bullet>();
    public int hits = 1;

    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public override void Reset()
    {
        base.Reset();
        immuneBullets.Clear();
        hits = 1;
    }

    private IEnumerator coroutine;

    public override void hitboxUpdate()
    {
        base.hitboxUpdate();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, (ren.sprite.bounds.size.x * transform.localScale.x / 2));
        foreach (Collider2D other in hitColliders)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                BaseEnemy enemy = other.GetComponent<BaseEnemy>();
                if (!enemy.immuneHitboxes.Contains(this))
                {
                    enemy.immuneHitboxes.Add(this);
                    enemy.TakeDamage(20f);
                    coroutine = ResetImmunity(enemy);
                    StartCoroutine(coroutine);
                }
            }
            if (other.gameObject.CompareTag("Bullet"))
            {
                Bullet b = other.gameObject.GetComponent<Bullet>();
                if (b.owner.Equals(BulletManager.BulletOwner.PLAYER))
                if (b != null)
                {
                    if (!immuneBullets.Contains(b))
                    {
                        immuneBullets.Add(b);
                        touchBullet(b);
                        b.ApplyOnHitAttachments();
                        b.HP--;
                        if (b.HP <= 0)
                        {
                            b.Die();
                        }
                        coroutine = ResetImmunity(b);
                        StartCoroutine(coroutine);
                    }
                }
            }
        }
    }
    IEnumerator ResetImmunity(BaseEnemy target)
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(0.2f);
        target.immuneHitboxes.Remove(this);
    }
    IEnumerator ResetImmunity(Bullet target)
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(0.2f);
        immuneBullets.Remove(target);
    }

    public void touchBullet(Bullet b)
    {
        hits++;
        lifeTimer -= (Mathf.Log(hits) - Mathf.Log(hits-1)) * (b.damage / 2);
    }
}
