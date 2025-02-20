using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public Vector2 lastVelocity = Vector2.zero;
    [SerializeField]
    public Dictionary<string, Vector2> bonusVelocity = new Dictionary<string, Vector2>();
    public float modifiedSpeed = 1f;
    public float baseSpeed = 1f;
    public float bonusSpeedMult = 1f;
    public BulletManager.BulletOwner owner;
    public Vector2 positionTarget;
    public CircleCollider2D thisCollider;
    public Sprite thisSprite;
    public SpriteRenderer ren;

    public List<BulletAttachment> attachments = new List<BulletAttachment>();
    public Vector2 firingOrigin;
    public float effectRange;

    public int HP = 1;
    public List<Bullet> payloads = new List<Bullet>();
    public BaseEnemy lastHitEnemy;
    public int payloadsToSpawn = 1;
    public int payloadCount = 0;
    public bool isSpawning = false;
    public float damage = 0;
    public int bounces = 0;
    public TrailRenderer trailRenderer;
    public enum CauseOfDeath
    {
        SELF,
        HITENEMY
    }

    private void Start()
    {
        ren = GetComponent<SpriteRenderer>();
    }

    public Bullet resetValues()
    {
        this.velocity = Vector2.zero;
        this.positionTarget = Vector2.zero;
        this.owner = BulletManager.BulletOwner.NONE;
        gameObject.SetActive(true);
        attachments.Clear();
        firingOrigin = Vector2.zero;
        effectRange = -1f;
        HP = 1;
        lastHitEnemy = null;
        thisSprite = ren.sprite;
        payloadsToSpawn = 1;
        payloadCount = payloads.Count;
        damage = 0;
        bonusSpeedMult = 1f;
        baseSpeed = 0f;
        modifiedSpeed = baseSpeed;
        bounces = 0;
        bonusVelocity.Clear();
        return this;
    }

    public void ApplyOneTimeAttachments()
    {
        foreach (BulletAttachment a in attachments)
        {
            if (a.type.Equals(BulletAttachment.Type.ONETIME))
            {
                a.Apply();
            }
        }
    }

    public void ApplyOnDeathAttachments()
    {
        foreach (BulletAttachment a in attachments)
        {
            if (a.type.Equals(BulletAttachment.Type.ONDEATH))
            {
                a.Apply();
            }
        }
    }

    public void ApplyOnHitAttachments()
    {
        foreach (BulletAttachment a in attachments)
        {
            if (a.type.Equals(BulletAttachment.Type.ONHIT))
            {
                a.Apply();
            }
        }
    }


    public void BulletUpdate()
    {
        modifiedSpeed = baseSpeed * bonusSpeedMult;
        foreach (BulletAttachment a in attachments)
        {
            if (a.type.Equals(BulletAttachment.Type.CONTINUOUS))
            {
                a.Apply();
            }
            a.Update();
        }

        if (payloadsToSpawn == 0)
        {
            payloadCount = 0;
        }
        else
        {
            payloadCount = payloads.Count;
        }

        Vector2 position = transform.position;
        Vector2 positionPrevious = position;
        foreach (KeyValuePair<string, Vector2> vect in bonusVelocity)
        {
            velocity += vect.Value;
        }
        positionTarget += velocity * modifiedSpeed;
        transform.position = Vector2.Lerp(
            position,
            positionTarget,
            Time.deltaTime * 30.0f);
        if (thisCollider.enabled)
        {
            CheckCollisions(positionPrevious);
        }

        if (position.x > BulletManager.instance.xBounds || position.x < -BulletManager.instance.xBounds ||
            position.y > BulletManager.instance.yBounds || position.y < -BulletManager.instance.yBounds)
        {
            Die(true);
        }
        if (lastVelocity != velocity)
        {
            float rotationAngle = LookAtPoint(transform.position);
            transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }
        lastVelocity = velocity;

    }

    void CheckCollisions(Vector2 previous)
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(previous, transform.position, 7);
        if (hits.Length != 0)
        {
            foreach (RaycastHit2D h in hits)
            {
                if (h.transform.gameObject.CompareTag("Enemy") || h.transform.gameObject.CompareTag("Plant"))
                {
                    Collide(h.collider);
                    break;
                }
            }
        }
        else {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(thisCollider.bounds.center, thisCollider.radius * transform.localScale.x);
            foreach (Collider2D c in hitColliders)
            {
                Collide(c);
            }
        }
    }

    private void Collide(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && owner.Equals(BulletManager.BulletOwner.PLAYER))
        {
            BaseEnemy enemy = col.gameObject.GetComponent<BaseEnemy>();
            if (!enemy.immuneBullets.Contains(this))
            {
                enemy.immuneBullets.Add(this);
                enemy.touchBullet(this);
                ApplyOnHitAttachments();
                lastHitEnemy = enemy;
                HP--;
                if (HP <= 0)
                {
                    Die();
                }
            }
        }
        else if (col.gameObject.CompareTag("Player") && owner.Equals(BulletManager.BulletOwner.ENEMY))
        {
            EnemyManager.instance.player.touchBullet(this);
            Die();
        }
    }
    public void Die()
    {
        ApplyOnDeathAttachments();
        gameObject.SetActive(false);
        BulletManager.instance.markedForDeathBullets.Add(this);
    }
    void Die(bool dontTriggerAttachments)
    {
        if (!dontTriggerAttachments)
        {
            ApplyOnDeathAttachments();
        }
        gameObject.SetActive(false);
        BulletManager.instance.markedForDeathBullets.Add(this);
    }
    private float LookAtPoint(Vector3 current)
    {
        float angleDeg = (Mathf.Rad2Deg * Mathf.Atan2((current.y + velocity.y) - current.y, (current.x + velocity.x) - current.x));
        return angleDeg;
    }

}
