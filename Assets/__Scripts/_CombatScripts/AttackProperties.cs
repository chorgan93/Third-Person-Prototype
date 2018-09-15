using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperties : MonoBehaviour {

	public float attackWarmUp;
	public float attackCooldown;

	public float attackComboChainTime = 0.12f;
    public float allowComboTimeAfterAttack = 0.2f;

    public float attackSpeed;

    public Hitbox myHitbox;

    void Start()
    {
        myHitbox = GetComponent<Hitbox>();
    }
}
