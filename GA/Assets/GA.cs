using UnityEngine;
using System.Collections.Generic;
using GBGA;

public class GA : myGA<List<int>> {
    public GameObject spots;
    public Spot spot;
    int count = 0;
    List<Spot> spotslist = new List<Spot>();

    public override void init() {
        for (int i = 0; i < number; ++i) {
            MyGene temp = new MyGene(spotslist);
            currentGenes.Add(temp);
        }
    }

    public override void Crossover(gene<List<int>> item1, gene<List<int>> item2) {
        var min = Random.Range(0, item1.data.Count);
        var max = Random.Range(min, item1.data.Count);

        var rangeOfItem1 = item1.data.GetRange(min, max);
        var rangeOfItem2 = item2.data.GetRange(min, max);
        foreach (var i in rangeOfItem2)
            item1.data.Remove(item1.data.BinarySearch(i));
        foreach (var i in rangeOfItem1)
            item2.data.Remove(item2.data.BinarySearch(i));
        item1.data.InsertRange(min, rangeOfItem2);
        item2.data.InsertRange(min, rangeOfItem1);
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
