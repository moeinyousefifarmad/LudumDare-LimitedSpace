using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum State{
        idle,
        run,
        rise,
        fall,
        getHit,
        dead,
        None,
    }
    enum Gun{
        simple,
        ak47
    }
    [SerializeField] private float speed , acceleration , deceleration ,velPower;
    [SerializeField] private float jumpPower;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject GunGameObject;
    private Rigidbody2D rb2d;
    private State currentState;
    private Gun currentGun;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        currentState = State.idle;
        currentGun = Gun.simple;
    }

    private void Update()
    {
        if(currentState == State.idle)// -----------------------------idle state 
        {
            Shoot();
            Aim();
            if(xInput()!= 0) currentState = State.run; 
            if(Input.GetKeyDown(KeyCode.Space) && IsOnGround())
            {
                currentState = State.rise;
                Jump();
            }  
        }

        if(currentState == State.run)// ----------------------------------run state 
        {
            Shoot();
            Aim();
            Move();
            if(Mathf.Abs(rb2d.velocity.x) <= 0.5f) currentState = State.idle; 
            if(Input.GetKeyDown(KeyCode.Space) && IsOnGround())
            {
                currentState = State.rise;
                Jump();
            } 
        }
        if(currentState == State.rise) // ---------------------------------rise state 
        {
            Shoot();
            Aim();
            Move();
            if(rb2d.velocity.y <= 0) currentState = State.fall;
        }
        if(currentState == State.fall)// ------------------------------------fall state 
        {
            Shoot();
            Aim();
            Move();
            if(IsOnGround()) currentState = State.idle;
        }
    }

    private void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x , jumpPower);
    }
    private void Move()
    {
        float targetSpeed;
        float speedDif;
        targetSpeed = xInput() * speed; 
        speedDif = targetSpeed - rb2d.velocity.x; 
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration:deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate , velPower) * Mathf.Sign(speedDif);
        rb2d.AddForce(movement * Vector2.right);
    }

    private float xInput()=> Input.GetAxisRaw("Horizontal");
    private bool IsOnGround() => Physics2D.Raycast(groundCheckPos.position , Vector2.down , groundCheckRayDistance , groundLayer);

    private void Aim()
    {
        GunGameObject.transform.right = MousePos() - (Vector2) GunGameObject.transform.position;
    }

    private Vector2 MousePos() => (Vector2) Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , 0));

    private void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(currentGun == Gun.simple) GunGameObject.GetComponentInChildren<SimpleGun>().ShootingManual();
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(currentGun == Gun.simple) GunGameObject.GetComponentInChildren<SimpleGun>().AutomaticShooting();
        }
    }
}
