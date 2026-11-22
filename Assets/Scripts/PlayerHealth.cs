using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] private float invulnerableDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private Color damageColor = new Color(1f, 0.25f, 0.25f);
    [SerializeField] private ParticleSystem deadParticle;

    Renderer[] bodyRenderers;
    MaterialPropertyBlock tintBlock;

    int currentLives;
    bool isVulnerable = true;
    bool dead;
    float delay = 1f;
    
    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");
    static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    public bool IsVulnerable{ 
        get  { return isVulnerable; } 
        set  { isVulnerable = value; } 
    }

    public int CurrentLives 
    {
        get { return currentLives; } 
        set { if(currentLives > 3) currentLives = lives; else currentLives = value; }
    }

    public int MaxLives 
    {
        get { return lives; } 
    }

    void Awake()
    {
        dead = false;
        CurrentLives = lives;
        tintBlock = new MaterialPropertyBlock();

        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        List<Renderer> body = new List<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] is MeshRenderer || renderers[i] is SkinnedMeshRenderer)
            {
                body.Add(renderers[i]);
            }
        }

        bodyRenderers = body.ToArray();
    }

    public void LoseHealth(int damage = 1)
    {
        if(!isVulnerable) return;

        CurrentLives -= damage;

        if(dead)
        {
            deadParticle.Play();
            Invoke(nameof(GameOver),delay);
        }
        if(isVulnerable) StartCoroutine(Invulnerable());
        
        Debug.Log($"Vida Jugador: {CurrentLives}");
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Level1");
    }
    
    public void RecoverHealth()
    {
        CurrentLives = lives;
    }

    IEnumerator Invulnerable()
    {
        float elapsed = 0f;
        bool showDamageColor = false;
        isVulnerable = false;
        while (elapsed < invulnerableDuration)
        {
            showDamageColor = !showDamageColor;
            if (showDamageColor)
            {
                SetTint(damageColor);
            }
            else
            {
                ClearTint();
            }

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        ClearTint();

        if (currentLives <= 0) dead = true;

        isVulnerable = true;
    }

    void SetTint(Color color)
    {
        for (int i = 0; i < bodyRenderers.Length; i++)
        {
            bodyRenderers[i].GetPropertyBlock(tintBlock);
            tintBlock.SetColor(BaseColorId, color);
            tintBlock.SetColor(ColorId, color);
            tintBlock.SetColor(EmissionColorId, color);
            bodyRenderers[i].SetPropertyBlock(tintBlock);
        }
    }

    void ClearTint()
    {
        for (int i = 0; i < bodyRenderers.Length; i++)
        {
            bodyRenderers[i].SetPropertyBlock(null);
        }
    }

}
