using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spot : MonoBehaviour {
    public int spotID;
    public Text text;
    // Use this for initialization
    void Start () {
        text.text = "" + spotID;
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
