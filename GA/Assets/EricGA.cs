using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EricGA : MonoBehaviour
{
    static public void swap<T>(int index1, int index2, ref List<T> myList)
    {
        var temp = myList[index1];
        myList[index1] = myList[index2];
        myList[index2] = temp;
    }

    public const int initLoad = 100;

    public class Chromosome
    {
        public List<Spot> spots;

        float fitnessValue;

        float notEnough;

        public Chromosome(Chromosome chromosome)
        {
            this.spots = new List<Spot>(chromosome.spots);
        }

        public Chromosome(List<Spot> _spots){
            spots = new List<Spot>(_spots);
            
            for (int i = 1; i < spots.Count; ++i)
            {
                var target = Random.Range(i, spots.Count);
                swap<Spot>(target, i, ref spots);
            }
        }

        public float fitness()
        {
            float sumOfDistance = 0.0f;
            float load = initLoad;
            for (int i = 0; i < spots.Count; ++i)
            {
                sumOfDistance += (spots[i].transform.position - spots[(i + 1) % spots.Count].transform.position).magnitude;
                load += spots[i].load;
                if (load < 0)
                    return 0.0001f;
            }
            
            fitnessValue = 100.0f / (sumOfDistance + 1);
            return fitnessValue;
        }
    }

    public EricLineHandler lineHandler;
    public GameObject spotsParrent;
    public Spot spot;
    public List<Spot> spots = new List<Spot>();
    public List<Chromosome> chromosomes = new List<Chromosome>();

    float CrossoverRate = 0.95f, MutationRate = 0.025f;
    int countOfGeneration = 2000, countOfChromosome = 500;

    int possibleClickSize = 5;
    List<Vector3> range = new List<Vector3>();


    void Start () {
        lineHandler = Instantiate(lineHandler.gameObject).gameObject.GetComponent<EricLineHandler>();
        spots = new List<Spot>();
        

        range.Add(new Vector3(-possibleClickSize, -possibleClickSize, 0));
        range.Add(new Vector3(-possibleClickSize, possibleClickSize, 0));
        range.Add(new Vector3(possibleClickSize, possibleClickSize, 0));
        range.Add(new Vector3(possibleClickSize, -possibleClickSize, 0));
        range.Add(new Vector3(-possibleClickSize, -possibleClickSize, 0));
    }

    public void addSpot()
    {
        lineHandler.clearLines(EricLineHandler.LINE_TYPE.STATIC);
        Spot sp = Instantiate(spot);
        sp.spotID = spots.Count;
        spot.name = "Spot" + sp.spotID;
        sp.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
        sp.transform.SetParent(spotsParrent.transform);
        spots.Add(sp);

        int load = initLoad;

        for(int i = 0; i < spots.Count; i++)
        {
            if (i != spots.Count - 1)
                spots[i].load = Random.Range(-100, 100);
            else
                spots[i].load = -load;
            load += spots[i].load;
        }
    }

    public void algorithm() {
        initChromosome(countOfChromosome);

        for(int i = 0; i < countOfGeneration; i++)
        {
            //printGeneration(chromosomes, i);
            prepareSelection();
            List<Chromosome> newGeneration = new List<Chromosome>();
            for (int j = 0; j < chromosomes.Count; j+=2)
            {
                Chromosome target1 = new Chromosome(chromosomes[selection()]);
                Chromosome target2 = new Chromosome(chromosomes[selection()]);

                if (CrossoverRate > Random.Range(0.0f, 1.0f))
                {
                    Crossover(ref target1, ref target2);
                }
                if (MutationRate > Random.Range(0.0f, 1.0f))
                {
                    Mutation(ref target1);
                }
                if (MutationRate > Random.Range(0.0f, 1.0f))
                {
                    Mutation(ref target2);
                }
                newGeneration.Add(target1);
                newGeneration.Add(target2);
            }
            chromosomes = newGeneration;
        }

        lineHandler.clearLines(EricLineHandler.LINE_TYPE.STATIC);

        prepareSelection();
        Chromosome chromosome = chromosomes[selection()];
        List<Vector3> line = new List<Vector3>();

        for (int i = 0; i < chromosome.spots.Count; i++)
        {
            if(i == 1)
                chromosome.spots[i].GetComponent<MeshRenderer>().material.color = Color.grey;
            else if(i == 0)
                chromosome.spots[i].GetComponent<MeshRenderer>().material.color = Color.blue;
            else
                chromosome.spots[i].GetComponent<MeshRenderer>().material.color = Color.black;


            line.Add(chromosome.spots[i].transform.position);
        }
        line.Add(chromosome.spots[0].transform.position);
        lineHandler.drawPointsList(line, EricLineHandler.LINE_TYPE.STATIC, lineHandler.getBallColor(0));

        printGeneration(chromosomes, 0);
    }

    void initChromosome(int countOfChromosome) {
        chromosomes.Clear();
        for (int i = 0; i < countOfChromosome; i++)
            chromosomes.Add(new Chromosome(spots));
    }


    List<float> wheel = new List<float>();

    void prepareSelection() {
        float sum = 0.0f;
        wheel.Clear();
        for (int i = 0; i < chromosomes.Count; ++i)
        {
            sum += chromosomes[i].fitness();
            wheel.Add(sum);
        }
    } 

    int selection()
    {
        
        float random = Random.Range(0.0f, wheel[wheel.Count - 1]);
        for (int index = 0; index < wheel.Count; ++index)
        {
            if (random <= wheel[index])
                return index;
        }
        return 0;
    }

    public void Crossover(ref Chromosome item1, ref Chromosome item2)
    {
        int min = Random.Range(0, item1.spots.Count);
        int max = Random.Range(min, item1.spots.Count);

        List<Spot> rangeOfItem1 = item1.spots.GetRange(min, max - min + 1);
        List<Spot> rangeOfItem2 = item2.spots.GetRange(min, max - min + 1);
        
        foreach (Spot item in rangeOfItem2)
            item1.spots.Remove(item);

        foreach (Spot item in rangeOfItem1)
            item2.spots.Remove(item);
        
        item1.spots.InsertRange(min, rangeOfItem2);
        item2.spots.InsertRange(min, rangeOfItem1);
    }

    public void Mutation(ref Chromosome item)
    {
        int min = Random.Range(1, item.spots.Count);
        int max;// = Random.Range(min, item.spots.Count);

        if (min + 1 == item.spots.Count)
            max = min - 1;
        else
            max = min + 1;

        swap<Spot>(min, max, ref item.spots);
    }

    public void printGeneration(List<Chromosome> generation, int generationIndex)
    {
        string debugMessage = "generation: " + generationIndex + " Total: " + generation.Count + "\n";
        for (int i = 0; i < generation.Count; i++)
        {
            Chromosome chromosome = generation[i];
            debugMessage += "chreomosome" + i + ": ";

            for(int j = 0; j < chromosome.spots.Count; j++)
            {
                debugMessage += chromosome.spots[j].spotID + " ";
            }
            debugMessage += ",fitnessValue: " + chromosome.fitness() + "\n";
        }
        Debug.Log(debugMessage);
    }

    private void Update()
    {
        lineHandler.clearLines(EricLineHandler.LINE_TYPE.DYNAMIC);
        lineHandler.drawPointsList(range, EricLineHandler.LINE_TYPE.DYNAMIC, Color.white);
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Mathf.Abs(ray.origin.x) < possibleClickSize && Mathf.Abs(ray.origin.y) < possibleClickSize)
            {
                lineHandler.clearLines(EricLineHandler.LINE_TYPE.STATIC);
                Spot sp = Instantiate(spot);
                sp.spotID = spots.Count;
                spot.name = "Spot" + sp.spotID;
                sp.transform.position = new Vector3(ray.origin.x, ray.origin.y, 0);
                sp.transform.SetParent(spotsParrent.transform);
                spots.Add(sp);

                int load = initLoad;

                for (int i = 0; i < spots.Count; i++)
                {
                    if (i != spots.Count - 1)
                        spots[i].load = Random.Range(-100, 100);
                    else
                        spots[i].load = -load;
                    load += spots[i].load;
                }
            }
        }
    }
}
