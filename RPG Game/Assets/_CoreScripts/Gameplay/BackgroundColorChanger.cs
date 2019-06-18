using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] Color color1 = Color.red;
    [SerializeField] Color color2 = Color.blue;
    [SerializeField] float duration = 3.0F;


    private Camera camera;

	void Start ()
    {
        camera = GetComponent<Camera>();
	}

    private void Update()
    {
        ColorChanger();
    }

    void ColorChanger()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        camera.backgroundColor = Color.Lerp(color1, color2, t);
    }

}
