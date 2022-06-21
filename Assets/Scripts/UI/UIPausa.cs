using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class UIPausa : MonoBehaviour
{
    public Canvas menuPausa;
    public Canvas menuFinal;
    public TextMeshProUGUI tiempoFinal;
    public TextMeshProUGUI youWin;

    public GameObject botonesMenuFinal;
    public GameObject scrollView;
    public TMP_InputField nombre;
    private double finalTime;

    public static UIPausa instance;
    void Start()
    {
        menuPausa.enabled = false;
        menuFinal.enabled = false;
        instance = this;
    }


    public void Pausa()
    {

        Time.timeScale = 0;
        menuPausa.enabled = true;
    }

    public void Continuar()
    {
        Time.timeScale = 1;
        menuPausa.enabled = false;
    }

    public void ReiniciarPantalla()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        menuPausa.enabled = false;
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        menuPausa.enabled = false;
    }

    public void MenuFinal(double time)
    {
        finalTime = time;
        menuFinal.enabled = true;
        menuPausa.enabled = false;
        botonesMenuFinal.SetActive(false);
        scrollView.SetActive(false);
        tiempoFinal.text = finalTime.ToString();
    }

    public void CargarDatos()
    {
        SceneManagement.instance.FinalizarPantalla(finalTime, nombre.text, SceneManager.GetActiveScene().name);
        scrollView.SetActive(true);
        botonesMenuFinal.SetActive(true);
        nombre.gameObject.SetActive(false);
    }

    public void DatosDetrasScrollView()
    {
        youWin.text = "";
        tiempoFinal.text = "";
    }
}
