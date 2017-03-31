using UnityEngine;
using System.Collections.Generic;
using GC = System.GC;

namespace GBGA {
    public abstract class gene<T> {
        public double fitnessValues {
            get {
                return this.fitness();
            }
        }
        public T data {
            get;
            private set;
        }

        public gene() {
            data = random();
        }

        public abstract T random();

        public abstract double fitness();

        public abstract gene<T> clone();
    }

    public abstract class myGA<T> : MonoBehaviour {
        public int number {
            get;
            private set;
        }

        public double CrossoverRate {
            get;
            private set;
        }

        public double MutationRate {
            get;
            private set;
        }

        public int generation {
            get;
            private set;
        }

        protected List<gene<T>> currentGenes = new List<gene<T>>();
        protected List<gene<T>> newGenes = new List<gene<T>>();

        public myGA(int number = 10, double CrossoverRate = 0.02, double MutationRate = 0.01) {
            this.number = number;
            this.CrossoverRate = CrossoverRate;
            this.MutationRate = MutationRate;
        }

        public virtual void init() {
            for (int i = 0; i < number; ++i) {
                gene<T> temp = default(gene<T>);
                currentGenes.Add(temp);
            }
            GC.Collect();
        }

        public virtual void algorithm() {
            for (var j = 0; j < generation; ++j) {
                for (var i = 0; i < number; i += 2) {
                    var target1 = currentGenes[selection()].clone();
                    var target2 = currentGenes[selection()].clone();
                    if (CrossoverRate > Random.Range(0.0f, 1.0f)) {
                        Crossover(target1, target2);
                    }
                    if (MutationRate > Random.Range(0.0f, 1.0f)) {
                        Mutation(target1);
                    }
                    if (MutationRate > Random.Range(0.0f, 1.0f)) {
                        Mutation(target2);
                    }
                    newGenes.Add(target1);
                    newGenes.Add(target2);
                }
            }
            currentGenes = newGenes;
            newGenes = new List<gene<T>>();
        }

        protected int selection() {
            double sum = default(double);
            List<double> wheel = new List<double>();
            wheel.Add(sum);//add 0.0f
            for (int i = 0; i < number; ++i) {
                sum += currentGenes[i].fitnessValues;
                wheel.Add(sum);
            }
            double random = Random.Range(0.0f, (float)sum);
            int index = default(int);
            for (; index < wheel.Count - 1; ++index) {
                if (random <= wheel[index + 1] && random > wheel[index])
                    break;
            }
            return index;
        }

        public abstract void Crossover(gene<T> item1, gene<T> item2);

        public abstract void Mutation(gene<T> item);

        public virtual void berakGA() {

        }
    }
}