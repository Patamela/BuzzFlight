using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderEfectos;

    [Header("Percetange Texts")]
    [SerializeField] private TextMeshProUGUI txtPorcentajeMaster;
    [SerializeField] private TextMeshProUGUI txtPorcentajeMusica;
    [SerializeField] private TextMeshProUGUI txtPorcentajeEfectos;


    private const string PREVIOUS_SCENE_KEY = "PreviousScene"; 
    // -------------------------
    // Configuración inicial
    // -------------------------

    public void ConfigurarListeners(
        UnityAction onVolver,
        UnityAction<float> onCambioMusica,
        UnityAction<float> onCambioEfectos)
    {
        sliderMusica.onValueChanged.AddListener(onCambioMusica);
        sliderEfectos.onValueChanged.AddListener(onCambioEfectos);
    }
    public void MasterChanges(float volume)
    {
        AudioListener.volume = volume;
    }
    // -------------------------
    // Lectura de valores
    // -------------------------

    public float GetSliderMusicaValue()  => sliderMusica.value;
    public float GetSliderEfectosValue() => sliderEfectos.value;

    // -------------------------
    // Actualización de UI
    // -------------------------

    public void SetSliderMusica(float valor)
    {
        sliderMusica.value = valor;
    }

    public void SetSliderEfectos(float valor)
    {
        sliderEfectos.value = valor;
    }

    public void ActualizarPorcentajeMusica(float valorLineal)
    {
        ActualizarPorcentaje(txtPorcentajeMusica, valorLineal);
    }

    public void ActualizarPorcentajeEfectos(float valorLineal)
    {
        ActualizarPorcentaje(txtPorcentajeEfectos, valorLineal);
    }

    private void ActualizarPorcentaje(TextMeshProUGUI texto, float valorLineal)
    {
        if (texto == null) return;
        int porcentaje = Mathf.RoundToInt(valorLineal * 100f);
        texto.text = $"{porcentaje}%";
    }

    public void Back()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }
}
