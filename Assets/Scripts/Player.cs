/*
This method holds instance variables and methods used by the player character in game
It deals with collision, distance travelled, velocity and position in the pipe.
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {

    //Instance variables
	public PipeSystem pipeSystem;

	public float startVelocity;
	public float rotationVelocity;

	public MainMenu mainMenu;
	public HUD hud;

	public float[] accelerations;

	private Pipe currentPipe;

	public float acceleration, velocity;
	private float distanceTraveled;
	private float deltaToRotation;
	private float systemRotation;
	private float worldRotation;
	public float avatarRotation;
    public Avatar avatar;

    private Vector2 touchOrigin = -Vector2.one;

	private Transform world, rotater;

    //This method initialises the player and game values when a new game is started
    public void StartGame (int accelerationMode) {
		distanceTraveled = 0f;
		avatarRotation = 0f;
		systemRotation = 0f;
		worldRotation = 0f;
		acceleration = accelerations[accelerationMode];
		velocity = startVelocity;
		currentPipe = pipeSystem.SetupFirstPipe();
		SetupCurrentPipe();
		gameObject.SetActive(true);
		hud.SetValues(distanceTraveled, velocity);
	}

    //This method turns the player inactive when the game has ended. It also switches the screen to the end screen
	public void Die () {
		mainMenu.EndGame(distanceTraveled);
		gameObject.SetActive(false);
	}

	private void Awake () {
		world = pipeSystem.transform.parent;
		rotater = transform.GetChild(0);
		gameObject.SetActive(false);
	}

    //This method updates the players position in the pipe as the game progresses
	private void Update () {
		velocity += acceleration * Time.deltaTime;
		float delta = velocity * Time.deltaTime;
		distanceTraveled += delta;
		systemRotation += delta * deltaToRotation;

		if (systemRotation >= currentPipe.CurveAngle) {
			delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
			currentPipe = pipeSystem.SetupNextPipe();
			SetupCurrentPipe();
			systemRotation = delta * deltaToRotation;
		}

		pipeSystem.transform.localRotation =
			Quaternion.Euler(0f, 0f, systemRotation);

		UpdateAvatarRotation();

        hud.SetValues(distanceTraveled, velocity);
	}

    //This method deals with moving the rotator sideways in the pipe (left or right). It also holds functionality for letting the player jump.
    private void UpdateAvatarRotation()
    {
        float rotationInput = 0f;
        if (Application.isMobilePlatform) // If on mobile
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).position.x < Screen.width * 0.5f)
                {
                    rotationInput = -1f; // Move left if touched on left half of screen
                }
                else
                {
                    rotationInput = 1f; // else move right
                }
            }
        }

        //if jumping
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            print("I am jumping");
            avatar.Jump();
        }
        else
        {
            rotationInput = Input.GetAxis("Horizontal");
        }
        avatarRotation += rotationVelocity * Time.deltaTime * rotationInput;
        if (avatarRotation < 0f)
        {
            avatarRotation += 360f;
        }
        else if (avatarRotation >= 360f)
        {
            avatarRotation -= 360f;
        }

        rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
    }




    /* float rotationInput = 0f;

     //mobile input
     if (Application.isMobilePlatform)
     {

         //if user has tapped screen
         if (Input.touchCount == 1)
         {
             if (Input.GetTouch(0).position.x < Screen.width * 0.5f)
             {
                 rotationInput = -1f;
             }
             else
             {
                 rotationInput = 1f;
             }
         }
          //if user has swiped
          else
          {
              Touch myTouch = Input.touches[0];

              if (myTouch.phase == TouchPhase.Began)
              {
                  touchOrigin = myTouch.position;
              }
              else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
              {
                  Vector2 touchEnd = myTouch.position;
                  float x = touchEnd
              }
          }
     }

     //keyboard input
     else
     {
         //if (inputDirection == InputDirection.Left || inputDirection == InputDirection.Right)
         // {

         rotationInput = Input.GetAxis("Horizontal");

         avatarRotation += rotationVelocity * Time.deltaTime * rotationInput;
         if (avatarRotation < 0f)
         {
             avatarRotation += 360f;
         }
         else if (avatarRotation >= 360f)
         {
             avatarRotation -= 360f;
         }
     }
         rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
         /* }
          else if (inputDirection == InputDirection.Top)
          {
              rotater.localRotation = Quaternion.Euler(0f, 8.0f, 0f);
          }*/


    //This method sets the current pipe relative to the previous pipe so its a transition.
    private void SetupCurrentPipe () {
		deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
		worldRotation += currentPipe.RelativeRotation;
		if (worldRotation < 0f) {
			worldRotation += 360f;
		}
		else if (worldRotation >= 360f) {
			worldRotation -= 360f;
		}
		world.localRotation = Quaternion.Euler(worldRotation, 0f, 0f);
	}
}