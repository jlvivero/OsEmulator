using UnityEngine;
using System.Collections;

public class hideMe : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {

	}

    public void hideThis(bool onOff)
    {
        gameObject.SetActive(onOff);
    }

    public void hideThat(bool onOff)
    {
        gameObject.SetActive(!onOff);
    }
}
