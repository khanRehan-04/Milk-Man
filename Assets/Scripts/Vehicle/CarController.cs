using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarController : MonoBehaviour
{
    public float speed = 15f;
    public float turnSpeed = 50f;
    public float turnSmoothness = 5f;

    private Rigidbody rb;
    private float moveInput = 0;
    private float turnInput = 0;

    private bool usingKeyboard = false;
    private bool usingUIButton = false;

    // UI Buttons
    public Button forwardButton;
    public Button backwardButton;
    public Button leftButton;
    public Button rightButton;
    public GameObject exitCarButton;

    // Audio
    public AudioSource carAudioSource;
    public AudioClip engineIdleClip;
    public AudioClip drivingClip;

    private bool isDriving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assign UI button listeners
        if (forwardButton) AddButtonListeners(forwardButton, 1, 0);
        if (backwardButton) AddButtonListeners(backwardButton, -1, 0);
        if (leftButton) AddButtonListeners(leftButton, 0, -1);
        if (rightButton) AddButtonListeners(rightButton, 0, 1);

        // Make sure exit button is initially hidden
        if (exitCarButton) exitCarButton.SetActive(false);

        // Initialize audio
        if (carAudioSource)
        {
            carAudioSource.loop = true; // Loop audio clips
            carAudioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        float keyboardMove = (Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));
        float keyboardTurn = (Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0));

        if (keyboardMove != 0 || keyboardTurn != 0)
        {
            usingKeyboard = true;
            usingUIButton = false;
        }

        if (usingKeyboard)
        {
            moveInput = keyboardMove;
            turnInput = keyboardTurn;
        }

        if (!usingKeyboard && usingUIButton)
        {
            // UI inputs remain active
        }

        if (!usingKeyboard && !usingUIButton)
        {
            moveInput = 0;
            turnInput = 0;
        }

        // Show Exit Car button if car is fully stopped
        if (exitCarButton)
        {
            exitCarButton.SetActive(moveInput == 0 && turnInput == 0);
        }

        // Handle sound transitions
        HandleCarSounds();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * moveInput * speed;

        float targetTurn = turnInput * turnSpeed;
        float currentTurn = rb.angularVelocity.y;
        float smoothedTurn = Mathf.Lerp(currentTurn, targetTurn, Time.fixedDeltaTime * turnSmoothness);
        rb.angularVelocity = new Vector3(0, smoothedTurn, 0);
    }

    void AddButtonListeners(Button button, float move, float turn)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

        button.onClick.AddListener(() => { });

        EventTrigger.Entry pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pointerDown.callback.AddListener((data) => SetUIInput(move, turn));
        trigger.triggers.Add(pointerDown);

        EventTrigger.Entry pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        pointerUp.callback.AddListener((data) => ResetUIInput());
        trigger.triggers.Add(pointerUp);
    }

    void SetUIInput(float move, float turn)
    {
        usingUIButton = true;
        usingKeyboard = false;
        moveInput = move;
        turnInput = turn;
    }

    void ResetUIInput()
    {
        usingUIButton = false;
        if (!usingKeyboard)
        {
            moveInput = 0;
            turnInput = 0;
        }
    }

    void HandleCarSounds()
    {
        if (carAudioSource)
        {
            if (moveInput != 0 || turnInput != 0) // If car is moving
            {
                if (!isDriving || carAudioSource.clip != drivingClip)
                {
                    carAudioSource.clip = drivingClip;
                    carAudioSource.Play();
                    isDriving = true;
                }
            }
            else if (isDriving) // Car is idle but in use
            {
                carAudioSource.clip = engineIdleClip;
                carAudioSource.Play();
                isDriving = false;
            }
        }
    }

    public void StopCarSound() // Call this on exit
    {
        if (carAudioSource)
        {
            carAudioSource.Stop();
        }
    }
}
