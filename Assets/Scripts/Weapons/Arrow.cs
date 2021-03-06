using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Arrow : Projectile
{
    protected bool isFrozen = false;
    private readonly float defaultGravityScale = 1f;

    protected override void Awake()
    {
        base.Awake();
        rb2D.gravityScale = defaultGravityScale;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isFrozen)
        {
            Vector2 velo = rb2D.velocity;
            float angle = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - angleOffset, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFrozen || IsHurtboxAndCreator(collision))
            return;

        GameObject hitGameObject = collision.gameObject;
        Transform hitParentTrans = collision.transform.parent;
        if (!hitParentTrans)
            return;

        GameObject hitParentGameObject = hitParentTrans.gameObject;
        bool hasHealthController = hitParentGameObject.GetComponent<HealthController>() != null;

        // Debug.Log("freezing arrow trigger enter " + collision.name);
        isFrozen = true;
        rb2D.velocity = Vector3.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (hasHealthController)
        {
            // Debug.Log($"arrow hurtbox hit: {hitGameObject.name} - arrow hit player or enemy {knockbackPower}");
            hitParentGameObject.GetComponent<HealthController>().DealDamage(creator, damage);
            hitParentGameObject.GetComponent<KnockbackController>().Knock(gameObject.transform.position, knockbackPower, knockbackTime);
            transform.parent = hitParentTrans;
        }
        else if (hitGameObject.CompareTag(Constants.GROUND_TAG))
        {
            // Debug.Log($"{gameObject.name}: arrow hit ground");
            transform.parent = hitGameObject.transform;
        }
    }

    public Vector3 CalculateArrowForceVector(Vector2 target, float arrowSpeed, bool isArrowDirect, bool shouldIgnoreCantReach=false)
    {
        Vector2 source = transform.position;
        // Debug.Log($"Target {target}, Source {source}");

        float y = target.y - source.y;
        float x = (new Vector2(source.x - target.x, 0)).magnitude;
        float g = -Physics2D.gravity.y;
        float v = arrowSpeed;
        float v2 = v * v;
        float v4 = v2 * v2;
        float x2 = x * x;

        float sqrt = v4 - (g * (g * x2 + 2 * y * v2));
        if (sqrt <= 0)
        {
            if (shouldIgnoreCantReach)
            {
                sqrt = 0.000001f;       
            } else
            {
                throw new Exception("arrow cant reach");
            }
        }

        sqrt = Mathf.Sqrt(sqrt);
        sqrt = isArrowDirect ? sqrt * (-1) : sqrt;
        float upper = v2 + sqrt;
        float lower = g * x;
        float angle = Mathf.Atan(upper/lower);

        Vector3 force = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0);
        // Debug.Log($"arrow Force {force}, sqrt {sqrt}, v {v}, angle {angle}, g {g}, x {x}, upper {upper}, lower {lower}");

        if (target.x - source.x < 0) force.x *= -1;

        return force;
    }
}
