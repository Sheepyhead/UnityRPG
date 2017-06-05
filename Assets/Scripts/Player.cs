using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{

    public Text foodText;
    public int wallDamage = 1, pointsPerFood = 10, pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    // Define character stats
    public int strength = 10, dexterity = 10, constitution = 10, intelligence = 10, wisdom = 10, charisma = 10;
    public int strengthMod
    {
        get { return (strength - 10) / 2; }
    }
    public int dexterityMod
    {
        get { return (dexterity - 10) / 2; }
    }
    public int constitutionMod
    {
        get { return (constitution - 10) / 2; }
    }
    public int intelligenceMod
    {
        get { return (intelligence - 10) / 2; }
    }
    public int wisdomMod
    {
        get { return (wisdom - 10) / 2; }
    }
    public int charismaMod
    {
        get { return (charisma - 10) / 2; }
    }
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

        food = GameManager.instance.playerHitPoints;

        foodText.text = "HP: " + strengthMod;

        animator = GetComponent<Animator>();

        base.Start();
    }

    void OnDisable()
    {
        GameManager.instance.playerHitPoints = food;
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        strength++;
        foodText.text = "StrMod: " + strengthMod;

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
            foodText.text = "+" + pointsPerFood + " HP: " + food;
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " HP: " + food;
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

        foodText.text = "-" + loss + " HP: " + food;
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