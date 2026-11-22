using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderEfectos;

    [Header("Percetange Texts")]
    [SerializeField] private TMP_Text txtPorcentajeMusica;
    [SerializeField] private TMP_Text txtPorcentajeEfectos;

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

    private void ActualizarPorcentaje(TMP_Text texto, float valorLineal)
    {
        if (texto == null) return;
        int porcentaje = Mathf.RoundToInt(valorLineal * 100f);
        texto.text = $"{porcentaje}%";
    }
}
