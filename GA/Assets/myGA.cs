using UnityEngine;
using System.Collections.Generic;
//using System;
using GC = System.GC;

namespace GBGA {
    public abstract class gene<T> {
        public double fitnessValues { 
            get {
                return this.fitness();
            }
        }
        public T data {
            get {
                return data;
            }
            private set {
                data = value;
            }
        }

        public gene() {
            data = this.init();
        }

        public abstract T init();

        public abstract double fitness();
    }

    public abstract class myGA<T> : MonoBehaviour {
        public int number {
            get {
                return number;
            }
            private set {
                number = value;
            }
        }

        public double CrossoverRate {
            get {
                return CrossoverRate;
            }
            private set {
                CrossoverRate = value;
            }
        }

        public double MutationRate {
            get {
                return MutationRate;
            }
            private set {
                MutationRate = value;
            }
        }

        private List<gene<T>> genes = new List<gene<T>>();

        public myGA(int number = 10, double CrossoverRate = 0.02, double MutationRate = 0.05) {
            this.number = number;
            this.CrossoverRate = CrossoverRate;
            this.MutationRate = MutationRate;
        }

        public virtual void init() {
            for (int i = 0; i < number; ++i) {
                gene<T> temp = default(gene<T>);
                genes.Add(temp);
            }
            GC.Collect();
        }

        public virtual void selection() {
            double sum = default(double);
            List<double> wheel = new List<double>();
            wheel.Add(sum);
            for (int i = 0;i<number; ++i) {
                sum += genes[i].fitnessValues;
                wheel.Add(sum);
            }
            double random = Random.Range(0.0f, (float)sum);
            int index = 0;
            for (; index < wheel.Count-1; ++index) {
                if (random <= wheel[index + 1] && random > wheel[index])
                    break;   
            }

        }

        public abstract void Crossover();

        public abstract void Mutation();


    }
}