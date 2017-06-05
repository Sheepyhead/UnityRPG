using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{

    public static Dice instance = null;

    // Use this for initialization
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Rolls one or more D6s and adds individual bonuses and a final bonus
    /// </summary>
    /// <param name="amount">Amount of D6s to roll</param> <br/>
    /// <param name="eachBonus">Bonus applied to every roll</param> <br/>
    /// <param name="totalBonus">Bonus applied to the roll total</param> <br/>
    /// <returns>The roll result with all bonuses applied</returns>
    public int rollD6(int amount = 1, int totalBonus = 0, int eachBonus = 0)
    {
        int result = 0;
        for (int i = 0; i < amount; i++)
        {
            result += Random.Range(1, 7) + eachBonus;
        }
        return result + totalBonus;
    }

    /// <summary>
    /// Rolls one or more D20s and adds individual bonuses and a final bonus
    /// </summary>
    /// <param name="amount">Amount of D20s to roll</param> <br/>
    /// <param name="eachBonus">Bonus applied to every roll</param> <br/>
    /// <param name="totalBonus">Bonus applied to the roll total</param> <br/>
    /// <returns>The roll result with all bonuses applied</returns>
    public int rollD20(int amount = 1, int totalBonus = 0, int eachBonus = 0)
    {
        int result = 0;
        for (int i = 0; i < amount; i++)
        {
            result += Random.Range(1, 21) + eachBonus;
        }
        return result + totalBonus;
    }

}