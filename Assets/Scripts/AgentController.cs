using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    // player input components
    PlayerInput pi;
    SteeringBehavior ai;
    AgentMan am;

    Vector2 moveInput;
    Vector2 aimInput;

    private GameObject model;
    private Rigidbody rb;

    public bool isPlayer;
    public int team = 0;
    public float speed = 10f;

    //ai var
    // For speed 
    private Vector3 position;        // local pointer to the RigidBody's Location vector
    private Vector3 velocity;        // Will be needed for dynamic steering

    // For rotation
    private float orientation;       // scalar float for agent's current orientation
    public float rotation;          // Will be needed for dynamic steering

    private Vector3 linear;         // The resilts of the kinematic steering requested
    private float angular;          // The resilts of the kinematic steering requested

    private void Awake() {
        am = FindObjectOfType<AgentMan>();
        ai = GetComponent<SteeringBehavior>();
        model = gameObject.transform.Find("Cube").gameObject;
        rb = GetComponent<Rigidbody>();
        pi = new PlayerInput();


        ai.target = am.PlayerAgent;
        position = rb.position;
        orientation = transform.eulerAngles.y;

        pi.PlayerControls.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        pi.PlayerControls.Move.canceled += ctx => moveInput = Vector2.zero;
        pi.PlayerControls.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        //pi.PlayerControls.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    private void FixedUpdate() {
        if (isPlayer) {
            HandleMove_Player();
        }
        else {
            HandleMove_AI();
        }
        
    }

    private void HandleMove_Player() {
        //move
        Vector2 moveVec = Square2Circle(moveInput);
        rb.velocity = Vector3.Lerp(rb.velocity, ToVec3(moveVec)*speed, 0.2f);

        //aim
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(model.transform.position);
        model.transform.forward = ToVec3((aimInput - playerScreenPos).normalized);
    }

    private void HandleMove_AI() {
        Vector4 linear_angular;
        linear_angular = ai.FlockingCP();
        linear = linear_angular;
        angular = linear_angular.w;

        UpdateMovement(linear, angular, Time.deltaTime);
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

        rb.AddForce(velocity - rb.velocity, ForceMode.VelocityChange);
        position = rb.position;
        //rb.MoveRotation(Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * orientation, 0)));
        //rb.angularVelocity = new Vector3(rb.angularVelocity.x, rotation * Mathf.Rad2Deg, rb.angularVelocity.z);
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
