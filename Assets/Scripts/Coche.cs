using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coche : MonoBehaviour
{
    public AudioClip freno;
    public AudioClip aceleracion;
    public AudioSource audioSource;
    public GameObject luzFrenoIzquierda;
    public GameObject luzFrenoDerecha;
    public Wheel frontLeftWheel;
    public Wheel frontRightWheel;
    public Wheel RearLeftWheel;
    public Wheel ReartRightWheel;

    private Wheel[] wheels;

    private Rigidbody rb;
    public float motorTorque;
    public float brakeTorque;
    private float moveDirection;
    private bool brake;
    private bool rearBrake;
    private bool acelerando;
    private bool onTime;
    private float steerDirection;
    private float marchaEngranada;
    private float timer;

    void Start()
    {
        //Bajar el centro de masa para que no vuelque el coche
        rb = GetComponent<Rigidbody>();
        this.rb.centerOfMass = new Vector3(0, -0.1f, 0);

        wheels = new Wheel[4];
        wheels[0] = frontLeftWheel;
        wheels[1] = frontRightWheel;
        wheels[2] = RearLeftWheel;
        wheels[3] = ReartRightWheel;
        marchaEngranada = 1;
        timer = 0.0f;
        onTime = false;
        luzFrenoDerecha.SetActive(false);
        luzFrenoIzquierda.SetActive(false);
    }

    void Update()
    {

        var rot = transform.rotation;
        rot.z = Mathf.Clamp(rot.z, -1.0f, 1.0f);
        transform.rotation = rot;

        UICoche.instance.CambiarTextoVelocidad(rb.velocity.magnitude);

        //Gestion del tiempo
        if (onTime)
        {
            timer += Time.deltaTime;
            UICoche.instance.UpdateTemporizador(timer);
        }
        if (!brake)
        {
            audioSource.pitch = rb.velocity.magnitude / 20;
            audioSource.pitch = Mathf.Clamp(audioSource.pitch, 0.35f, 100.0f);
        }
    }

    void FixedUpdate()
    {
        frontLeftWheel.Steer(steerDirection);
        frontRightWheel.Steer(steerDirection);
        if (brake)
        {
            ReproducirSonido("freno");
            luzFrenoDerecha.SetActive(true);
            luzFrenoIzquierda.SetActive(true);
            if (!rearBrake)
            {
                foreach (Wheel wheel in wheels)
                {
                    wheel.Brake(brakeTorque);
                }
            }
            else
            {
                RearLeftWheel.Brake(brakeTorque * 2);
                ReartRightWheel.Brake(brakeTorque * 2);
            }
        }
        else
        {
            if (audioSource.clip == freno)
            {
                ReproducirSonido("aceleracion");
            }
            foreach (Wheel wheel in wheels)
            {
                wheel.Brake(0);
            }

            foreach (Wheel wheel in wheels)
            {
                wheel.Accelerate(moveDirection * motorTorque / 2);
            }
            luzFrenoDerecha.SetActive(false);
            luzFrenoIzquierda.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Start")
        {
            onTime = true;
        }

        if (other.gameObject.tag == "Finish")
        {
            onTime = false;
            Invoke("Finalizar", 3f);
        }
        if (other.gameObject.tag == "FinishMulti")
        {
            onTime = false;
            FinalizarMulti();
        }
        if (other.gameObject.tag == "NearFinish")
        {
            SceneManagement.instance.CargarEfectosFinales();
        }
        if (other.gameObject.tag == "SecondLapTrigger")
        {
            ControlObstaculos.instance.CambiarVuelta();
        }
    }
    private void Finalizar()
    {
        Time.timeScale = 0;
        double tiempotext = System.Math.Round(timer, 2);
        UIPausa.instance.MenuFinal(tiempotext);
    }

    private void FinalizarMulti()
    {
        MultiplayerUIPausa.instance.FinalizarPantalla("Coche Amarillo");
    }
    private void ReproducirSonido(string sonido)
    {
        if (sonido == "freno")
        {
            if (audioSource.clip != freno)
            {
                audioSource.pitch = 1;
                audioSource.clip = freno;
                audioSource.Play();
            }
        }
        else if (sonido == "aceleracion")
        {
            if (audioSource.clip != aceleracion)
            {
                audioSource.clip = aceleracion;
                audioSource.Play();
            }
        }
    }

    public void Acelerar()
    {
        Debug.Log("Acelerando");
        ReproducirSonido("aceleracion");
        switch (marchaEngranada)
        {
            case 1:
                moveDirection = 1;
                break;
            case 0:
                moveDirection = 0;
                break;
            case -1:
                moveDirection = -1;
                break;
        }
    }

    public void DejarAcelerar()
    {
        Debug.Log("Dejando de acelerar");
        moveDirection = 0;
    }

    public void Frenar()
    {
        brake = true;
        rearBrake = false;
    }

    public void DejarDeFrenar()
    {
        brake = false;
        rearBrake = false;
    }

    public void GirarIzquiera()
    {
        steerDirection = -1;
    }

    public void GirarDerecha()
    {
        steerDirection = 1;
    }

    public void DejardeGirar()
    {
        steerDirection = 0;
    }

    public void FrenoDeMano()
    {
        brake = true;
        rearBrake = true;
    }

    public void SubirMarcha()
    {
        marchaEngranada++;
        UICoche.instance.CambiarTextoMarcha(marchaEngranada);
        marchaEngranada = Mathf.Clamp(marchaEngranada, -1, 1);
    }

    public void BajarMarcha()
    {
        marchaEngranada--;
        UICoche.instance.CambiarTextoMarcha(marchaEngranada);
        marchaEngranada = Mathf.Clamp(marchaEngranada, -1, 1);
    }
}
