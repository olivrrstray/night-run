using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

//TODO: Consider implementing Unity's Input System instead of using these legacy options. (https://www.youtube.com/watch?v=HmXU4dZbaMw)
public class PlayerMovement : MonoBehaviour
{
    //Declaration of serialized field variables.
    [SerializeField] private float speed; 
    [SerializeField] private float jumpHeight;
    [SerializeField] private float maxJumpTime;

    //Declartion of variables
    private Rigidbody2D body;
    private ContactFilter2D GroundContactFilter;
    private float jumpCounter = 0;
    private bool isJumping = false;
    private bool isGrounded => body.IsTouching(GroundContactFilter);
    private float horizonalKeyValue;
    private bool spaceHeld;

    

    //This method is called every time the script is created/ the game is launched.
    private void Awake () 
    {
        //Defines the rigidbody connected to the player.
        body = GetComponent<Rigidbody2D>(); 

        //Sets the parameters on the contact filter.
        GroundContactFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        GroundContactFilter.SetNormalAngle(45, 135);
    }

    //Runs every single frame. Good for inputs.
    private void Update() 
    {
        //Tracks if the character moves.
        horizonalKeyValue = Input.GetAxis("Horizontal");

        //Tracks if the character jumps, and for how long.
        spaceHeld = Input.GetKey(KeyCode.Space);
    }


    //Runs automatically in-step with the physics engine.
    private void FixedUpdate() 
    {
        //Sets the body's force (Affected by Mass, Gravity, Friction, etc).
        body.AddForce(new Vector2(horizonalKeyValue * speed, 0));

        //Jumping
        if (spaceHeld && isGrounded && !isJumping) 
        {
            Debug.Log("Blast off!");
            isJumping = true;
            jumpCounter = maxJumpTime;
            body.AddForce(new Vector2(0, 4*jumpHeight), ForceMode2D.Impulse);
        }
        else if (spaceHeld && isJumping && jumpCounter > 0) 
        {
            jumpCounter -= Time.deltaTime;

            if (jumpCounter > maxJumpTime * (3/4)) {
                body.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
            else if (jumpCounter > maxJumpTime * (1/2)) {
                body.AddForce(new Vector2(0, jumpHeight/2), ForceMode2D.Impulse);
            }
            else {
                body.AddForce(new Vector2(0, jumpHeight/4), ForceMode2D.Impulse);
            }
            Debug.Log("Extra Force!");
        }
        else 
        {
            jumpCounter = 0;
            isJumping = false;
        }    
    }

}

/*
NOTES SECTION!!!
//GetKey returns a bool depending on if it's input (the key) was pressed or not. KeyCode is an enumeration that contains all buttons.
//Sets the body's velocity, changing only the X variable. Unity automatically produces a value of -1 (left) or 1 (right) when you press A/D or Left/Right arrows.
//"Serialize Field" allows you to change the speed within the Unity Engine.
//GetComponent checks the object this script is attached to for an object of type "Rigidbody" and passes the reference to body.
*/
