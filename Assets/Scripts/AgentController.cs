﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    // player input components
    PlayerInput pi;
    SteeringBehavior ai;
    GameMan gm;

    Vector2 moveInput;
    Vector2 aimInput;

    private DynamicJoystick movStk;
    private DynamicJoystick shtStk;
    private GameObject model;
    private Rigidbody rb;

    private ParticleSystem wakeParticle;
    private ParticleSystem deathParticle;
    private GameObject indicator;

    public bool connected = false;
    public bool isPlayer;
    public int team = 0;
    public float speed = 10f;
    public float speedLimit = 15f;
    public int power = 10;

    // shoot time
    float lastShootTime = 0f;
    public float shootCD = 1f;
    public float projSpd = 20f;
    public float shootAcc = 20f;


    public float connectDist = 2f;
    private Material mat;
    private Color playerBlue = new Color(0f, 0.589f, 1f);

    private bool playerReviving = false;

    //ai var
    // For speed 
    private Vector3 position;        // local pointer to the RigidBody's Location vector
    private Vector3 velocity;        // Will be needed for dynamic steering

    // For rotation
    private float orientation;       // scalar float for agent's current orientation
    public float rotation;          // Will be needed for dynamic steering

    private Vector3 linear;         // The resilts of the kinematic steering requested
    private float angular;          // The resilts of the kinematic steering requested

    // wall stuck 
    private float stuckCountdown = 3f;
    private float stuckCountdownTimer;
    private bool isStuck;

    private void Awake() {
        gm = FindObjectOfType<GameMan>();
        ai = GetComponent<SteeringBehavior>();
        model = gameObject.transform.Find("Cube").gameObject;
        rb = GetComponent<Rigidbody>();
        pi = new PlayerInput();
        mat = model.GetComponent<MeshRenderer>().material;
        wakeParticle = transform.Find("WakeParticle").GetComponent<ParticleSystem>();
        deathParticle = transform.Find("DeathParticle").GetComponent<ParticleSystem>();
        indicator = transform.Find("Indicator").gameObject;
        movStk = gm.movStk;
        shtStk = gm.shtStk;

        ai.target = gm.PlayerAgent;
        position = rb.position;
        orientation = transform.eulerAngles.y;

        if (isPlayer) {
            connected = true;
        }
    }

    private void Start() {
        if (GameMan.Instance.controlScheme == GameMan.ControlScheme.mouse)
        {
            pi.PlayerControls.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            pi.PlayerControls.Move.canceled += ctx => moveInput = Vector2.zero;
            pi.PlayerControls.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
            pi.PlayerControls.Shoot.performed += ctx => HandleShoot();
            pi.PlayerControls.Aim.canceled += ctx => aimInput = Vector2.zero;

            movStk.gameObject.SetActive(false);
            shtStk.gameObject.SetActive(false);
        }

        gm.LevelUpUI.GetComponent<LevelUpUI>().Agents.Add(this);
        gm.LevelUpUI.GetComponent<LevelUpUI>().Sync(this);
    }

    private void Update() {
        // touch control
        if (GameMan.Instance.controlScheme == GameMan.ControlScheme.touch) {
            moveInput = movStk.Direction;
            aimInput = shtStk.Direction;
            if (aimInput.magnitude > 0.5f)
                HandleShoot();
        }
        
        if (isPlayer && connected) {
            indicator.SetActive(true);
        }
        else {
            indicator.SetActive(false);
        }
    }

    private void FixedUpdate() {
        if (isPlayer) {
            if (playerReviving) {
                mat.SetColor("_BaseColor", Color.Lerp(mat.GetColor("_BaseColor"), playerBlue, 0.1f));
                model.transform.Rotate(0f, 10f, 0f);
                gameObject.transform.Translate(0f, Time.fixedDeltaTime, 0f);

                if (gameObject.transform.position.y > 1.23f) {
                    mat.SetColor("_BaseColor", playerBlue);
                    gameObject.transform.position -= new Vector3(0f, gameObject.transform.position.y - 1.23f, 0f);
                    wakeParticle.Play();
                    UpdateFollowTarget();
                    print(gameObject.name + " connected");
                    connected = true;
                    playerReviving = false;
                }
            }
            else {
                switch (GameMan.Instance.controlScheme) {
                    case GameMan.ControlScheme.touch:
                        HandleMove_Player_Touch();
                        break;
                    case GameMan.ControlScheme.mouse:
                        HandleMove_Player_Mouse();
                        break;
                }
                //model.GetComponent<MeshRenderer>().material.color = Color.Lerp(model.GetComponent<MeshRenderer>().material.color, Color.red, 0.1f);
            }

        }
        else {
            if (connected) {
                HandleMove_AI();
                CheckStuck();
            }
            else {
                if (Vector3.Distance(transform.position, gm.PlayerAgent.transform.position) < connectDist 
                    && gm.PlayerAgent.GetComponent<AgentController>().connected) {

                    // wake up
                    mat.SetColor("_BaseColor", Color.Lerp(mat.GetColor("_BaseColor"), playerBlue, 0.1f));
                    model.transform.Rotate(0f, 10f, 0f);
                    gameObject.transform.Translate(0f, Time.fixedDeltaTime, 0f);

                    if (gameObject.transform.position.y > gm.PlayerAgent.transform.position.y) {
                        mat.SetColor("_BaseColor", playerBlue);
                        gameObject.transform.position -= new Vector3(0f, gameObject.transform.position.y - gm.PlayerAgent.transform.position.y, 0f);
                        wakeParticle.Play();
                        gm.Agents.Add(gameObject);
                        UpdateFollowTarget();
                        print(gameObject.name + " connected");
                        connected = true;
                    }
                }
                else {
                    mat.SetColor("_BaseColor", Color.grey);
                    gameObject.transform.position -= new Vector3(0f, gameObject.transform.position.y - 0.25f, 0f);
                }
            }
        }
        
    }

    public void ReviveAsPlayer() {
        playerReviving = true;
        gm.Agents.Add(gameObject);
        gm.SwitchPlayer();
    }

    private void HandleMove_Player_Mouse() {
        if (!connected) return;
        //move
        Vector2 moveVec = Square2Circle(moveInput);
        rb.velocity = Vector3.Lerp(rb.velocity, ToVec3(moveVec)*speed, 0.2f);

        //aim
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(model.transform.position);
        model.transform.forward = ToVec3((aimInput - playerScreenPos).normalized);
    }

    private void HandleMove_Player_Touch()
    {
        if (!connected) return;
        //move
        Vector2 moveVec = Square2Circle(moveInput);
        rb.velocity = Vector3.Lerp(rb.velocity, ToVec3(moveVec) * speed, 0.2f);

        //aim
        if (aimInput.magnitude > 0f)
            model.transform.forward = new Vector3(aimInput.x, 0f, aimInput.y);
    }

    private void HandleShoot() {
        if (Time.time - lastShootTime < shootCD) {
            return;
        }

        if (!connected) {
            return;
        }

        if (GameMan.Instance.LevelUpUI.activeInHierarchy) {
            return;
        }

        if (isPlayer) {
            lastShootTime = Time.time;
            GameObject proj = gm.GetPlayerProj();
            proj.transform.position = model.transform.position;
            proj.transform.rotation = model.transform.rotation;
            proj.transform.Rotate(0f, Random.value * shootAcc - shootAcc/2f, 0f);
            proj.GetComponent<PlayerProj>().speed = projSpd;
            proj.GetComponent<PlayerProj>().dmg = power/10f;
            proj.SetActive(true);
        }
        else {
            float randomFloat = Random.value/2f;
            GameObject proj = gm.GetPlayerProj();
            lastShootTime = Time.time + randomFloat * shootCD;
            proj.transform.position = model.transform.position;
            proj.transform.rotation = model.transform.rotation;
            proj.GetComponent<PlayerProj>().speed = projSpd;
            proj.GetComponent<PlayerProj>().dmg = power / 10f;
            proj.transform.Rotate(0f, Random.value * shootAcc - shootAcc / 2f, 0f);
            proj.SetActive(true);
        }
    }
    IEnumerator ShootAfterTime(float time) {
        yield return new WaitForSeconds(time);
        GameObject proj = gm.GetPlayerProj();
        lastShootTime = Time.time;
        proj.transform.position = model.transform.position;
        proj.transform.rotation = model.transform.rotation;
        proj.GetComponent<PlayerProj>().speed = projSpd;
        proj.GetComponent<PlayerProj>().dmg = power / 10f;
        proj.transform.Rotate(0f, Random.value * shootAcc - shootAcc / 2f, 0f);
        proj.SetActive(true);
    }

    private void HandleMove_AI() {
        Vector4 linear_angular;
        linear_angular = ai.FlockingCP();
        linear = linear_angular;
        angular = linear_angular.w;

        UpdateMovement(linear, angular, Time.deltaTime);
    }

    private void CheckStuck()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, GameMan.Instance.PlayerAgent.transform.position - transform.position);
        isStuck = Physics.Raycast(ray, out hit, 1f, LayerMask.GetMask("Wall"), QueryTriggerInteraction.Ignore);
        isStuck &= Vector3.Distance(transform.position, GameMan.Instance.PlayerAgent.transform.position - transform.position) > 5f;

        if (isStuck)
            Debug.DrawLine(transform.position, hit.point, playerBlue, 0.1f);

        if (isStuck)
        {
            stuckCountdownTimer -= Time.fixedDeltaTime;
        }
        else
        {
            stuckCountdownTimer = stuckCountdown;
        }

        if (isStuck && stuckCountdownTimer < 0f)
        {
            Disconnect();
        }
    }

    /// <summary>
    /// UpdateMovement is used to apply the steering behavior output to the agent itself.
    /// It also brings together the linear and acceleration elements so that the composite
    /// result gets applied correctly.
    /// </summary>
    /// <param name="steeringlin"></param>
    /// <param name="steeringang"></param>
    /// <param name="time"></param>
    private void UpdateMovement(Vector3 steeringlin, float steeringang, float time) {
        // Update the orientation, velocity and rotation
        orientation += rotation * time;
        velocity += steeringlin * time;
        rotation += steeringang * time;

        //rb.AddForce(velocity - rb.velocity, ForceMode.VelocityChange);
        rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.2f);
        if (rb.velocity.magnitude > speedLimit) {
            rb.velocity = rb.velocity.normalized * speedLimit;
            velocity = rb.velocity;
        }
        position = rb.position;
        if (GameMan.Instance.controlScheme == GameMan.ControlScheme.touch)
        {
            if (aimInput.magnitude > 0f)
                model.transform.forward = new Vector3(aimInput.x, 0f, aimInput.y);
        }
        else {
            Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(model.transform.position);
            model.transform.forward = ToVec3((aimInput - playerScreenPos).normalized);
        }
    }

    private void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.layer == LayerMask.NameToLayer("EnemyProj")) {
            Disconnect();

            if (isPlayer) {
                StartCoroutine("SwitchPlayerAfterTime", 1f);
            }
        }
    }

    public void Disconnect()
    {
        connected = false;
        mat.SetColor("_BaseColor", Color.grey);
        gameObject.transform.position -= new Vector3(0f, gameObject.transform.position.y - 0.25f, 0f);
        gm.Agents.Remove(gameObject);
        deathParticle.Play();
    }

    public void DisconnectNotRemove()
    {
        connected = false;
        mat.SetColor("_BaseColor", Color.grey);
        gameObject.transform.position -= new Vector3(0f, gameObject.transform.position.y - 0.25f, 0f);
        //gm.Agents.Remove(gameObject);
        deathParticle.Play();
    }

    public void UpdateFollowTarget() {
        ai.target = gm.PlayerAgent;
    }

    IEnumerator SwitchPlayerAfterTime(float time) {
        yield return new WaitForSeconds(time);
        isPlayer = false;
        gm.SwitchPlayer();
    }

    protected Vector2 Square2Circle(Vector2 input) {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2f);

        return output;
    }

    protected Vector3 ToVec3(Vector2 vec2) {
        return new Vector3(vec2.x, 0f, vec2.y);
    }

    private void OnEnable() {
        pi.PlayerControls.Enable();
    }

    private void OnDisable() {
        pi.PlayerControls.Disable();
    }
}
