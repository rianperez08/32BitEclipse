using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : Player
{
    PlayerInput playerInput;
    InputAction attack_action;
    InputAction charge_action;
    InputAction parry_action;
    InputAction look_action;

    [Header("Player Combat")]
    public float attack_cooldown;
    public float attack_duration;
    public float charge_duration;
    public float invul_duration;
    public bool canBeHurt;

    [Header("Box Cast Attributes")]
    [SerializeField] float box_width;
    [SerializeField] float box_height;
    [SerializeField] float box_distance;
    [SerializeField] LayerMask enemyLayer;

    Vector3 direction = new Vector3(0f, 0f, 5f); // set direction as ray length and direction (1 for 2)

    Vector3 LocalDirection => transform.TransformDirection(direction);

    private void Awake()
    {
        canBeHurt = true;
        playerInput = new PlayerInput();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Player Attacked");
    }
    public void OnCharge(InputAction.CallbackContext context)
    {
        Debug.Log("Player Charged Attack");
    }
    public void OnParry(InputAction.CallbackContext context)
    {
        Debug.Log("Player Parried");
    }

    private void OnEnable()
    {
        attack_action = playerInput.Combat.Attack;
        attack_action.performed += OnAttack;
        attack_action.Enable();
        charge_action = playerInput.Combat.ChargeAttack;
        charge_action.performed += OnCharge;
        charge_action.Enable();
        parry_action = playerInput.Combat.Parry;
        parry_action.performed += OnParry;
        parry_action.Enable();
        look_action = playerInput.Combat.Look;
        look_action.Enable();
    }

    private void OnDisable()
    {
        attack_action.Disable();
        charge_action.Disable();
        parry_action.Disable();
    }
    

    private void FixedUpdate()
    {
        Vector2 lookDir = look_action.ReadValue<Vector2>();
    }


}
