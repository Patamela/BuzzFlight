using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{   

    [Header("UI Settings")]
    [SerializeField] private InputAction pause;

    [Header("UI")]
    [SerializeField] private int levelId;
    [SerializeField] private List<GameObject> hearts;
    [SerializeField] private Image[] ammunition;
    [SerializeField] private TextMeshProUGUI timeDash;
    [SerializeField] private Image dashIcon;
    [SerializeField] private TextMeshProUGUI textScore;
    [SerializeField] private GameObject panelPause;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Sprites")]
    [SerializeField] private Sprite imageAmmoEmpty;
    [SerializeField] private Sprite imageAmmoFull;
    
    bool isPaused;

    void OnEnable()
    {
        pause.Enable();
    }

    void OnDisable()
    {
        pause.Disable();
    }

    void Start()
    {
        DrawHearts();
        DrawAmmunition();
        UpdateScore();
    }

    void Update()
    {
        DrawHearts();
        DrawAmmunition();
        UpdateDash();
        TogglePause();
        UpdateScore();
    }

    void TogglePause()
    {
        if (pause.WasPressedThisFrame())
        {
            isPaused = !isPaused;
            panelPause.SetActive(isPaused);

            Time.timeScale = isPaused ? 0 : 1f;
        }
    }

    void DrawHearts()
    {
        for(int i=0; i < playerHealth.MaxLives; i++)
        {
            hearts[i].SetActive(i < playerHealth.CurrentLives);
        }
    }

    void DrawAmmunition()
    {
        for(int i=0; i < ammunition.Length; i++)
        {
            if (i < playerShooting.CurrentAmmo) ammunition[i].sprite = imageAmmoFull;
            else ammunition[i].sprite = imageAmmoEmpty;
        }
    }

    void UpdateDash()
    {
        timeDash.text = playerMovement.CoolDownTimer.ToString("0s");
        if(playerMovement.CoolDownTimer <= 0) dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 1f);
        else dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 0.50f);
    }
    
    void UpdateScore()
    {   
        int score = PlayerPrefs.GetInt("MaxScore");
        textScore.text = score.ToString("000000 pollen");
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void Settings()
    {   
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }

    public void SaveExit()
    {   
        //TODO save de progress
        SceneManager.LoadScene("SelectionLevel");
    }
}
