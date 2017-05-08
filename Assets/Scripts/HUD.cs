/*
This script holds variables and methods used for the HUD interactions in game.
*/

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    //instance variables
    public Text distanceLabel, velocityLabel;

    //This method sets the in game text labels for the distance traveled, and the current velocity.
    public void SetValues (float distanceTraveled, float velocity) {
		distanceLabel.text = ((int)(distanceTraveled * 10f)).ToString();
		velocityLabel.text = ((int)(velocity * 10f)).ToString();
	}
}