using GBGA;
using System.Collections.Generic;
using UnityEngine;

public class MyGene : gene<List<int>>{
    public int numberOfSpot {
        get;
        private set;
    }

    private List<Spot> city;

    public MyGene(List<Spot> spots):base() {
        this.numberOfSpot = spots.Count;
        city = spots;
    }

    public override List<int> random() {
        List<int> randomCity = new List<int>();
        for (int i = 0; i < numberOfSpot; ++i)
            randomCity.Add(i);
        for(int i = 0; i < numberOfSpot; ++i) {
            var target = Random.Range(i, numberOfSpot);
            swap(target,i,randomCity);
        }
        return randomCity;
    }

    private void swap<T>(int index1,int index2,List<T> myList) {
        var temp = myList[index1];
        myList[index1] = myList[index2];
        myList[index2] = temp;
    }

    public override double fitness() {
        double sumOfDistance = default(double);
        for (int i = 0; i < numberOfSpot; ++i) {
            sumOfDistance += (city[data[i]].transform.position - city[data[i%numberOfSpot]].transform.position).magnitude;
        }
        return 1.0f/sumOfDistance;
    }

    public override gene<List<int>> clone() {
        MyGene temp = new MyGene(this.city);
        temp.data.Clear();//clear random();
        temp.data.AddRange(this.data);//add new this.data;
        return temp;
    }
}
