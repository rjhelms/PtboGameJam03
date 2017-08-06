using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Enemy
{
    public Sprite BaseSprite;
    public Sprite FireSprite;
    public GameObject Projectile;
    public Transform FirePoint;
    public float StartRestTime = 0.2f;
    public float RestTime = 0.3f;
    public float FireInterval = 0.1f;
    public int FireBurst = 3;

    private float nextStateChange;
    private bool isFiring;
    private int burstCount;
    private SpriteRenderer spriteRenderer;
    
    protected override void Start()
    {
        base.Start();
        nextStateChange = Time.fixedTime + StartRestTime;
        isFiring = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Time.fixedTime > nextStateChange)
        {
            // if not firing, start firing
            if (!isFiring)
            {
                isFiring = true;
                spriteRenderer.sprite = FireSprite;
            }
            if (isFiring)
            {
                // if firing, pop off a shot
                Instantiate(Projectile, FirePoint.position,
                            Quaternion.identity);
                // increment burst count
                burstCount++;
                // if at limit, reset count and set next to rest interval
                if (burstCount == FireBurst)
                {
                    nextStateChange = Time.fixedTime + RestTime;
                    isFiring = false;
                    burstCount = 0;
                    spriteRenderer.sprite = BaseSprite;
                } else {
                    // if not at limit, set next to FireInterval
                    nextStateChange = Time.fixedTime + FireInterval;
                }
            }
        }
    }
}
