using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionChecker : MonoBehaviour
{
    public Text collisionText;


    void Start()
    {
        PlayerPrefs.SetInt("CollisionScore", 50);
        PlayerPrefs.SetInt("Lap", 1);
    }

    //Ufak çarpmalar tespit edilemiyor
    void OnCollisionEnter(Collision collision)
    {
        string collObject = collision.gameObject.tag;
        Debug.Log("test: " + collObject);
        if (collObject == "Wall")
            collisionText.text = "Collision with Wall";
        if (collObject == "Door")
            collisionText.text = "Collision with Door";

        PlayerPrefs.SetInt("CollisionScore", 0);

    }
    void OnCollisionExit(Collision other)
    {
        string collObject = other.gameObject.tag;

        if (collObject == "Wall" || collObject == "Door")
            collisionText.text = "";
    }

}
