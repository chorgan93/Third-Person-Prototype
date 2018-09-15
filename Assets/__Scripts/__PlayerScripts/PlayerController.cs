using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement Properties")]
    public float movementSpeedX = 10f;
    public float movementSpeedZ = 5f;
    Vector2 _inputMovement = Vector2.zero;
    Vector3 _movementForce = Vector3.zero;


    [Header("Dash Properties")]
    public float sideStepSpeed = 100f;
    public float sideStepDuration = 0.5f;
    public float sideStepCooldown = 0.2f;
    private float sideStepDurationCountdown = 0f;
    private float sideStepCooldownCountdown = 0f;

    [Header("Attack Properties")]
    public List<AttackProperties> attackChain;
    private int currentAttack = 0;
    private float attackWarmUpCountdown = 0f;
    public Hitbox attackHitbox;
    private bool hitboxTriggered = false;
    private float attackCooldownCountdown = 0f;
    private Vector3 attackForce = Vector3.zero;
    private float attackAllowChain = 0.12f;
    private float allowChainAfterAttack = 0f;

    private Vector3 dashForce = Vector3.zero;

    private Rigidbody _myRigidbody;

	// Use this for initialization
	void Start () {
        _myRigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
       Movement();
        SideStep();
        Attack();
    }

    void Movement(){

        if (!InAttack())
        {
            transform.LookAt(CameraFollowS.F.currentTarget);

            if (!InDash())
            {
                _inputMovement.x = Input.GetAxis("Horizontal");
                _inputMovement.y = Input.GetAxis("Vertical");

                _movementForce.x = _inputMovement.x * movementSpeedX * 60f * Time.deltaTime;
                _movementForce.z = _inputMovement.y * movementSpeedZ * 60f * Time.deltaTime;
                _movementForce.y = 0f;

                _myRigidbody.AddForce(_movementForce.x * transform.right, ForceMode.Force);
                _myRigidbody.AddForce(_movementForce.z * transform.forward, ForceMode.Force);
            }
        }
    }

    void SideStep(){

        if (sideStepDurationCountdown > 0){
            sideStepDurationCountdown -= Time.deltaTime;
        }else if (sideStepCooldownCountdown > 0){
            sideStepCooldownCountdown -= Time.deltaTime;
        }else if (Input.GetKeyDown(KeyCode.Space) && !InAttackExcludeChain()){
            if (_inputMovement.x > 0){
                dashForce = transform.right;
            }else if (_inputMovement.x < 0)
            {
                dashForce = -transform.right;
            }
            else{
                dashForce = -transform.forward;
            }


            dashForce *= sideStepSpeed * Time.deltaTime * 60f;

            sideStepCooldownCountdown = sideStepCooldown;
            sideStepDurationCountdown = sideStepDuration;

            _myRigidbody.velocity = Vector3.zero;
            _myRigidbody.AddForce(dashForce, ForceMode.Impulse);
        }
    }

    void Attack(){
        if (attackWarmUpCountdown > 0){
            attackWarmUpCountdown -= Time.deltaTime;
        }
        else if (attackCooldownCountdown > 0){
            if (!hitboxTriggered){
                attackHitbox.Activate();
                hitboxTriggered = true;
            }
            attackCooldownCountdown -= Time.deltaTime;
        }
        if (!InAttackExcludeChain()){

            if (allowChainAfterAttack > 0) { allowChainAfterAttack -= Time.deltaTime; }
            // allow for attacks at this point
            if (Input.GetMouseButtonDown(0)){
                if (allowChainAfterAttack > 0f){
                    currentAttack++;
                    if (currentAttack >= attackChain.Count){
                        currentAttack = 0;
                    }
                }else{
                    currentAttack = 0;
                }
                attackWarmUpCountdown = attackChain[currentAttack].attackWarmUp;
                attackCooldownCountdown = attackChain[currentAttack].attackCooldown;

                attackHitbox = attackChain[currentAttack].myHitbox;
                hitboxTriggered = false;

                attackAllowChain = attackChain[currentAttack].attackComboChainTime;
                allowChainAfterAttack = attackChain[currentAttack].allowComboTimeAfterAttack;

                attackForce = transform.forward * attackChain[currentAttack].attackSpeed * Time.deltaTime * 60f;
                _myRigidbody.velocity = Vector3.zero;
                _myRigidbody.AddForce(attackForce, ForceMode.Impulse);
            }
        }
    }

    public bool InDash(){
        return (sideStepDurationCountdown > 0);
    }

    public bool InAttack(){
        return (attackCooldownCountdown > 0 || attackWarmUpCountdown > 0);
    }
    public bool InAttackExcludeChain(){
        return (attackCooldownCountdown > attackAllowChain || attackWarmUpCountdown > 0);
    }
}
