using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{   

    [Header("UI")]
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

    void Start()
    {
        DrawHearts();
        DrawAmmunition();
    }

    void Update()
    {
        DrawHearts();
        DrawAmmunition();
        UpdateDash();
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
        Debug.Log(playerMovement.CoolDownTimer.ToString());
        timeDash.text = playerMovement.CoolDownTimer.ToString("0s");
        if(playerMovement.CoolDownTimer <= 0) dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 1f);
        else dashIcon.color = new Color(dashIcon.color.r, dashIcon.color.g, dashIcon.color.b, 0.50f);
    }
    
    public void Continue()
    {
        panelPause.SetActive(false);
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void SaveExit()
    {   
        //TODO save de progress
        SceneManager.LoadScene("SelectionLevel");
    }
}
