using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingHitbox : TimedHitbox
{
    public Vector2 velocity;
    public float speed;
    public SpriteRenderer ren;
    public CircleCollider2D circleCollider;
    public BoxCollider2D boxCollider;
    public Vector2 lastVelocity;

    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    public override void hitboxUpdate()
    {
        base.hitboxUpdate();
        Vector2 position = transform.position;
        Vector2 positionTarget = position + (velocity * speed);
        transform.position = Vector2.Lerp(
            position,
            positionTarget,
            Time.deltaTime * 30.0f);

        if (lastVelocity != velocity)
        {
            float rotationAngle = LookAtPoint(transform.position);
            transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }
        lastVelocity = velocity;


        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, (ren.sprite.bounds.size.x * transform.localScale.x / 2));
        if (hitColliders.Length > 1)
        {
            foreach (Collider2D collider in hitColliders)
            {
                if (collider.enabled && collider.gameObject.CompareTag("Enemy") && !collider.gameObject.Equals(gameObject))
                {
                    Debug.Log("Adding Bowling Knockback");
                    BaseEnemy enemy = collider.gameObject.GetComponent<BaseEnemy>();
                    enemy.addBowlingKnockback(enemy.findNormalBowlingPoint(this), 20f);
                }
            }
        }
    }

    private float LookAtPoint(Vector3 current)
    {
        float angleDeg = (Mathf.Rad2Deg * Mathf.Atan2((current.y + velocity.y) - current.y, (current.x + velocity.x) - current.x));
        return angleDeg;
    }
}
