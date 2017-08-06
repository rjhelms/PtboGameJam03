using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suit : Enemy
{
    public Sprite BaseSprite;
    public Sprite FireSprite;
    public GameObject[] Projectile;
    public Transform FirePoint;
    public float RestTime = 0.1f;
    public float FireTime = 0.3f;
    public float fireChance = 0.25f;
    private SpriteRenderer spriteRenderer;
    private bool isFiring;
    private float nextStateChange;

    protected override void Start()
    {
        base.Start();
        nextStateChange = Time.fixedTime + RestTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (controller.IsRunning)
        {
            if (Time.fixedTime > nextStateChange)
            {
                if (!isFiring)
                {
                    float fireRoll = Random.value;
                    if (fireRoll <= fireChance)
                    {
                        spriteRenderer.sprite = FireSprite;
                        nextStateChange += FireTime;
                        int projectileIndex = Random.Range(0, 
                                                           Projectile.Length);
                        Instantiate(Projectile[projectileIndex],
                                    FirePoint.position,
                                    Quaternion.identity);
                        isFiring = true;
                    }
                } else {
                    spriteRenderer.sprite = BaseSprite;
                    nextStateChange += RestTime;
                    isFiring = false;
                }
            }
        }
    }
}
