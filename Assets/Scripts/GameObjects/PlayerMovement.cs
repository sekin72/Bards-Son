using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player Player;
    public CharacterController CharacterController;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    private float _cooldown = 0f, _cooldownMax = 4f;

    private Enemy enemy;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    void Update()
    {
        if (!GameManager.Instance.IsWaitingForInputForText)
        {
            if (isGrounded && velocity.y < 0)
                velocity.y = -5f;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            if (x == 0 && z == 00 && !Player.AttackStarted)
            {
                Player.StartIdleAnim();
            }
            else if (!Player.AttackStarted)
            {
                Player.StartRunningAnim();
            }

            CharacterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
                velocity.y = Mathf.Sqrt(-2f * gravity * jumpHeight);

            velocity.y += gravity * Time.deltaTime;

            CharacterController.Move(velocity * Time.deltaTime);


            _cooldown += Time.deltaTime;
            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Player>().StartAttackAnim();
                if (_cooldown >= _cooldownMax && enemy != null)
                {
                    _cooldown = 0;
                    if (FRPSystem.RollD20(Player.attackBonus, enemy.AC))
                    {
                        enemy.Hurt(FRPSystem.RollDamage(Player.DamageDie, Player.damageBonus));
                        if (!enemy.gameObject.activeInHierarchy)
                            enemy = null;
                    }
                }
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Ground"))
            isGrounded = true;
        if (hit.gameObject.tag.Equals("Enemy"))
        {
            enemy = hit.gameObject.GetComponent<Enemy>();
            if (enemy == null)
                enemy = hit.gameObject.GetComponentInParent<Enemy>();
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
            isGrounded = false;
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            enemy = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.AfterCheckpoint && other.gameObject.tag.Equals("Checkpoint"))
        {
            GameManager.Instance.AbandonText();
        }
    }
}
