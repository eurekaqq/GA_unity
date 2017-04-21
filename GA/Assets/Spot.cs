using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Spot : MonoBehaviour {
    public int spotID;
    public int load = 0;
    public Text text;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
        text.text = "" + spotID + " " + load;
    }
}
