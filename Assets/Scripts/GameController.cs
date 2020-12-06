using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    private bool lockScreen = false;
    [SerializeField]
    GameObject infoText, guideText;
    [SerializeField]
    Camera[] cameras;
    [SerializeField]
    GameObject miniMap;

    public enum WhosCamera { Ken, Rocket }

    bool nearAPlanet;
    public bool NearAPlanet
    {
        get { return nearAPlanet; }
        set { nearAPlanet = value; }
    }
    GravityController nearPlanet;
    public GravityController NearPlanet
    {
        get { return nearPlanet; }
        set { nearPlanet = value; }
    }

    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    Slider healthBar;

    bool isAlive = true;
    int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            healthBar.value = health;
        }
    }
    GameObject player;
    public GameObject Player
    {
        get { return player; }
    }
    GameObject rocket;
    public GameObject Rocket
    {
        get { return rocket; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (lockScreen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        rocket = GameObject.FindGameObjectWithTag("PlayerRocket");
        ResetHealth();
    }
    private void ResetHealth()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }
    public void decreaseHealth(int h)
    {
        health -= h;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
        }
        healthBar.value = health;
    }
    public void increaseHealth(int h)
    {
        health += h;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
        healthBar.value = health;
    }
    public void displayText(string t)
    {
        infoText.SetActive(true);
        infoText.GetComponent<Text>().text += "\n" + t;

        StopCoroutine(TurnOffText());
        StartCoroutine(TurnOffText());
    }
    private IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(3f);
        infoText.GetComponent<Text>().text = "";
        infoText.SetActive(false);
    }
    public void displayGuide(string t)
    {
        string content = "";
        switch (t)
        {
            case "Player":
                content += "- WSAD to move \n";
                content += "- SPACE to jump \n ";
                content += "- MOUSE look around \n ";
                content += "- LMB shoot \n ";
                content += "- E board the you-f-ooh \n ";
                break;
            case "Rocket":
                content += "- SPACE launch/land \n ";
                content += "- WSAD move \n ";
                content += "- QE roll \n ";
                content += "- F exit when landed \n ";
                content += "- MOUSE look around \n ";
                break;
            default:
                break;
        }
        guideText.GetComponent<Text>().text = content;
    }
    public void SetCameraTo(WhosCamera who)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = false;
        }
        switch (who)
        {
            case WhosCamera.Ken:
                cameras[(int)WhosCamera.Ken].enabled = true;
                break;
            case WhosCamera.Rocket:
                cameras[(int)WhosCamera.Rocket].enabled = true;
                break;
            default:
                cameras[(int)WhosCamera.Ken].enabled = true;
                break;
        }
        miniMap.SetActive(cameras[(int)WhosCamera.Ken].enabled);
    }
}
