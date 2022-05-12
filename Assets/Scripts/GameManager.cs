using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //boid varible UI
    public Slider coSlider;
    public TMP_Text coVal;
    public Slider alSlider;
    public TMP_Text alVal;
    public Slider seSlider;
    public TMP_Text seVal;
    public Slider viewDistSlider;
    public TMP_Text viewDistVal;
    public Slider viewDistSepSlider;
    public TMP_Text viewDistSepVal;

    public TMP_Dropdown flightMode;

    public BoidsManager boids;

    private void Awake()
    {
        boids = GameObject.FindObjectOfType<BoidsManager>();
        //change the weights of the boids
        boids.SetWeights((int)coSlider.value, (int)alSlider.value, (int)seSlider.value);

        //set the view distance
        boids.SetViewDistance((int)viewDistSlider.value, (int)viewDistSepSlider.value);
    }

    public void SliderWeightChanged()
    {
        //change the weights of the boids
        boids.SetWeights((int)coSlider.value, (int)alSlider.value, (int)seSlider.value);

        //set the view distance
        boids.SetViewDistance((int)viewDistSlider.value, (int)viewDistSepSlider.value);

        //change the value displayed
        coVal.text = ((int)coSlider.value).ToString();
        alVal.text = ((int)alSlider.value).ToString();
        seVal.text = ((int)seSlider.value).ToString();
        viewDistVal.text = ((int)viewDistSlider.value).ToString();
        viewDistSepVal.text = ((int)viewDistSepSlider.value).ToString();
    }

    public void FlightModeChanged()
    {
        if (flightMode.value == 0)
            boids.SetFlightMode(true);
        else
        {
            boids.SetFlightMode(false);
        }
    }
}
