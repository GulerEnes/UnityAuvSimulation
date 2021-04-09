using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreProcesses : MonoBehaviour
{
    public Text scoreText;
    public Text lapText;

    void OnTriggerExit(Collider other)
    {
        string collObject = other.gameObject.tag;
        string myObject = gameObject.tag;
        Debug.Log("myObject: " + myObject + "\ncollObject: " + collObject);
        
        if (collObject == "Robot")
        {
            int score = PlayerPrefs.GetInt("CollisionScore");
            lapText.text += "\n" + PlayerPrefs.GetInt("Lap");
            PlayerPrefs.SetInt("Lap", PlayerPrefs.GetInt("Lap") + 1);
            PlayerPrefs.SetInt("CollisionScore", 50);

            if (myObject == "UpperVolume")
                scoreText.text += "\n" + (score + 50);
            if (myObject == "BottomVolume")
                scoreText.text += "\n" + (score + 25);
            

        }
    }
}
