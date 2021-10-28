using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOption : MonoBehaviour
{
	public Slider mainSlider;
    AudioSource audiosource;

    void Start() {
        audiosource = gameObject.GetComponent<AudioSource>();
        mainSlider.value = 1;
        mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
    }

    // Update is called once per frame
	public void ValueChangeCheck() {
        audiosource.volume = mainSlider.value;
	}
}
