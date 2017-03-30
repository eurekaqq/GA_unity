using UnityEngine;
using System.Collections.Generic;

public class GA  {

    public GameObject spots;
    public Spot spot;

    

    int count = 0;
    List<Spot> spotslist = new List<Spot>();

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    public void addSpot() {
        count++;
        Spot sp = GameObject.Instantiate(spot);
        sp.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        sp.name = "spot" + count;
        sp.spotID = count;
        sp.transform.SetParent(spots.transform);
        spotslist.Add(sp);
    }
}
