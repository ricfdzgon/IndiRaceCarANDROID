using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    private float totalTime;
    public List<GameObject> efectosFinales = new List<GameObject>();
    public ScoreTable scoreTable;
    void Start()
    {
        Time.timeScale = 1;
        instance = this;
        totalTime = 0;
    }

    public void FinalizarPantalla(double tiempo, string nombre, string nombreEscena)
    {
        //En vez de este debug tengo que guardar el nombre junto al tiempo para guardarlo en un archivo
        Debug.Log(nombre);
        Debug.Log(tiempo);
        Debug.Log(nombreEscena);
        UIPausa.instance.DatosDetrasScrollView();
        scoreTable.AddScore(new ScoreData(nombre, tiempo, nombreEscena));
    }

    public void CargarEfectosFinales()
    {
        foreach (GameObject efecto in efectosFinales)
        {
            efecto.SetActive(true);
        }
    }

    public void BorrarDatosGuardados()
    {
        DataManager.instance.ClearData(SceneManager.GetActiveScene().name);
        scoreTable.UpdateData();
    }
}
