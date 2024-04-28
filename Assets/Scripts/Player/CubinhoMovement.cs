using System.Collections.Generic;
using Unity.Services.Leaderboards;
using UnityEngine;
using Unity.Services.CloudSave;
using System.Threading.Tasks;

public class CubinhoMovement : MonoBehaviour
{
    // Variables
    public Animator animator;
    [SerializeField] private float limMinX, limMaxX;
    public float slidingSpeed, turningSpeed;
    private float targetX;
    private bool isCollided;
    private bool isReadyToRestart;
    public bool isPlayPressed;
    private bool canSlide;

    // Start is called before the first frame update
    void Start()
    {
        isPlayPressed = false;
        canSlide = false;
        animator = GetComponentInChildren<Animator>();
        targetX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSlide)
            return;

        if (isCollided)
        {
            if (Input.anyKeyDown && isReadyToRestart)
                GameManager.instance.RestartGame();
            return;
        }

        // Move forward non stop
        transform.position -= transform.forward * Time.deltaTime * slidingSpeed;
        int direction = 0;

        // Calculate target position for left and right movement based on input type
        if (IsMobilePlatform())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.x < Screen.width / 2)
                {
                    // Move right if touch is on the left half of the screen
                    targetX += (transform.right * Time.deltaTime * turningSpeed).x;
                    direction = 1;
                }
                else
                {
                    // Move left if touch is on the right half of the screen
                    targetX -= (transform.right * Time.deltaTime * turningSpeed).x;
                    direction = 2;
                }
            }
        }
        else
        {
            // Use keyboard input for non-mobile platforms
            if (Input.GetKey(KeyCode.A)) 
            { 
                targetX += (transform.right * Time.deltaTime * turningSpeed).x;
                direction = 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                targetX -= (transform.right * Time.deltaTime * turningSpeed).x;
                direction = 2;
            }
        }

        if (targetX < limMinX) targetX = limMinX;
        if (targetX > limMaxX) targetX = limMaxX;

        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), Time.deltaTime * turningSpeed);

        // Set the animator parameters
        animator.SetInteger("direction", direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle");

            isCollided = true;
            isReadyToRestart = true;
            GameManager.instance.ShowGameOverPopUp();

            if (PlayerPrefsManager.lastLevelLoaded != 0)
                return;

            // add score
            AddScore(ScoreCounter.instance.GetScore());
            
            if (ScoreCounter.instance.GetScore() > PlayerPrefsManager.highScore) 
                PlayerPrefsManager.highScore = ScoreCounter.instance.GetScore();
            
            if (ScoreCounter.instance.GetScore() > PlayerPrefsManager.highScoreToday)
                PlayerPrefsManager.highScoreToday = ScoreCounter.instance.GetScore();

            GameManager.instance.PopulateGameOverScreen(ScoreCounter.instance.GetScore());
            
            // add coins
            PlayerPrefsManager.coins += PlayerPrefsManager.runCoins;
            PlayerPrefsManager.runCoins = 0;
            SaveCoinsCloud();

            //Task.Delay(3000).ContinueWith(t => isReadyToRestart = true);
        }
    }

    public void IncrementSpeed()
    {
        slidingSpeed += 0.5f;
        turningSpeed += 0.5f;
    }

    public void StartSliding()
    {
        canSlide = true;
        GameManager.instance.HideInitialMenu();
    }

    private bool IsMobilePlatform()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    /*
     ------------------------------------------------------ LEADERBOARD ------------------------------------------------------
     */

    public async void AddScore(float score)
    {
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardManager.ALL_TIME_HIGH_LEADERBOARD_ID, score);
        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardManager.DAY_BEST_LEADERBOARD_ID, score);
    }

    public async void SaveCoinsCloud()
    {
        var playerData = new Dictionary<string, object>{
          {"coins", PlayerPrefsManager.coins}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }
}
