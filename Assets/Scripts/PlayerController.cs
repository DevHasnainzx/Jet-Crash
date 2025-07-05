using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Player Attributes")]
    public float thrustForce = 5f;
    public float maxspeed = 5f;

    [Header("Engine Flame")]
    public GameObject FlameBooster;

    [Header("Score Attribues")]
    public float scoreMultiplier = 10f;
    private float elapsedTime = 0f;
    private float score = 0f;
    private float highScore = 0f;
    private const string HighScoreKey = "HighScore";



    [Header("UI Attributes")]
    public UIDocument uiDocument;
    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;

    [Header("VFX Attributes")]
    public GameObject explosionEffect;

    [Header("VFX Attributes")]
    public GameObject borders;

    [Header("Mobile Inputs")]
    public InputAction moveForward;
    public InputAction lookPosition;

    Rigidbody2D rb;


    void Start()
    {
        moveForward.Enable();
        lookPosition.Enable();


        rb = GetComponent<Rigidbody2D>();

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        highScoreText.style.display = DisplayStyle.None;

       // highScore = PlayerPrefs.GetFloat(HighScoreKey,0f);
        UpdateHighScore();
        restartButton.clicked += RelaodScene;
    }

    // Update is called once per frame
    void Update()
    {
             MovePlayer();

            UpdateScore();

            UpdateBoost();

    }
    void MovePlayer()
    {
      //  if (Mouse.current.leftButton.isPressed) 
            if (moveForward.IsPressed())
            {
            // accesing mus eposition in screen space and coverting it to world space 
            //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);


            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());

            //   taking direction of mouse click and substracting player position to roarate the player in direction of mouse
            Vector2 direction = (mousePosition - transform.position).normalized;

            // appying force on upward direction to move the player towards pointed position
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxspeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxspeed;
            }
        }
    }

    void UpdateScore()
    {
        // aseesing actual time and assigning it as scoreboard
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;

        if (score > highScore)
        {
            highScore = score;
            UpdateHighScore();
        }
    }
    void UpdateHighScore()
        {
        highScoreText.text = "HighScore: " + highScore;
        }
    void UpdateBoost()
    {
        if (moveForward.WasPressedThisFrame())
        {
            FlameBooster.SetActive(true);
        }
        if (moveForward.WasReleasedThisFrame())
        {
            FlameBooster.SetActive(false);
        }

        /*
        // Activating and eactivating plan falame as moouse is clicked or released each frame
        if (Mouse.current.leftButton.wasPressedThisFrame)
{
    FlameBooster.SetActive(true);
}
else if (Mouse.current.leftButton.wasReleasedThisFrame)
{
    FlameBooster.SetActive(false);
}
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
     {
        PlayerPrefs.SetFloat(HighScoreKey, highScore);
        PlayerPrefs.Save();


        Destroy(gameObject);
        borders.SetActive(false);
        Instantiate(explosionEffect,transform.position,transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;
    }

    void RelaodScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
