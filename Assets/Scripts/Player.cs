using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{

    public Text foodText;
    public int wallDamage = 1, pointsPerFood = 10, pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public AudioClip moveSound1;

    public AudioClip moveSound2;

    public AudioClip eatSound1;

    public AudioClip eatSound2;

    public AudioClip drinkSound1;

    public AudioClip drinkSound2;

    public AudioClip gameOverSound;

    private Animator animator;
    private int food;

    // Use this for initialization
    protected override void Start()
    {
        foodText.text = "Food: " + food;

        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;

        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir, yDir, out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameIsOver();

        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");

    }
    private void /// <summary>
                 /// Sent when another object enters a trigger collider attached to this
                 /// object (2D physics only).
                 /// </summary>
                 /// <param name="other">The other Collider2D involved in this collision.</param>
    OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }
    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void CheckIfGameIsOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.RandomizeSfx(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;

        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameIsOver();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0, vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);

        }
    }
}