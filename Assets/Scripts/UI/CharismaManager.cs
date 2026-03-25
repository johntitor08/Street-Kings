using TMPro;
using UnityEngine;

public class CharismaManager : MonoBehaviour
{
    public static CharismaManager Instance;
    [SerializeField] private TextMeshProUGUI charismaText;
    private int charismaPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCharisma(int amount)
    {
        charismaPoints += amount;
        charismaText.text = "Charisma: " + charismaPoints;
    }

    public int GetCharismaPoints() => charismaPoints;
}
