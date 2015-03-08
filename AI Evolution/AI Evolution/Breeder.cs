using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    public struct StatWeight
    {
        public float STR;
        public float CON;
        public float DEX;
        public float INT;
        public float WIS;
        public float FTH;
        public float PER;

        public float[] Weights ;

        public StatWeight(float Str, float Con, float Dex, float Int, float Wis, float Fth, float Per)
        {
            Weights = new float[7];
            STR = Str; CON = Con; DEX = Dex; INT = Int; WIS = Wis; FTH = Fth; PER = Per;
            Weights[0] = STR; Weights[0] = CON; Weights[0] = DEX; Weights[0] = INT; Weights[0] = WIS; Weights[0] = FTH; Weights[0] = PER;
        }

        public StatWeight(Actor A)
        {
            Weights = new float[7];

            float totalStat =
                A.Stats.Strength +
                A.Stats.Constitution +
                A.Stats.Dexterity +
                A.Stats.Intelligence +
                A.Stats.Wisdom +
                A.Stats.Faith +
                A.Stats.Perception;

            float statPerPercent = totalStat / 100;
            STR = A.Stats.Strength / statPerPercent;
            CON = A.Stats.Constitution / statPerPercent;
            DEX = A.Stats.Dexterity / statPerPercent;
            INT = A.Stats.Intelligence / statPerPercent;
            WIS = A.Stats.Wisdom / statPerPercent;
            FTH = A.Stats.Faith / statPerPercent;
            PER = A.Stats.Perception / statPerPercent;

            Weights[0] = STR; Weights[0] = CON; Weights[0] = DEX; Weights[0] = INT; Weights[0] = WIS; Weights[0] = FTH; Weights[0] = PER;
        }
    }

    public static class Breeder
    {
        public static List<Actor> Breed_Actors(List<Tuple<float, Actor>> Stock)
        {
            List<Actor> result = Dispose_of_Bottomhalf(Stock);

            



            return result;
        }
        private static List<Actor> Dispose_of_Bottomhalf(List<Tuple<float, Actor>> Stock)
        {
            List<Actor> result = new List<Actor>();
            for (int i = 0; i < Stock.Count / 2; i++)
            {
                result.Add(Stock[i].Item2);
            }
            return result;
        }

        private static Actor[] Breed(Actor P1, Actor P2)
        {
            Actor[] result = new Actor[2];
            StatWeight W1 = new StatWeight(P1);
            StatWeight W2 = new StatWeight(P2);
            int dividingPoint = Misc.Random.Next(1, W1.Weights.Length);
            float[] C1 = new float[W1.Weights.Length];
            float[] C2 = new float[W1.Weights.Length];



            for (int i = 0; i < W1.Weights.Length; i++)
            {
                if (i >= dividingPoint)
                {
                    C1[i] = W1.Weights[i];
                    C2[i] = W2.Weights[i];
                }
                else
                {
                    C2[i] = W1.Weights[i];
                    C1[i] = W2.Weights[i];
                }
            }
            StatWeight CW1 = new StatWeight(C1[0], C1[1], C1[2], C1[3], C1[4], C1[5], C1[6]);
            StatWeight CW2 = new StatWeight(C2[0], C2[1], C2[2], C2[3], C2[4], C2[5], C2[6]);

            result[0] = new Hero(CW1, 300f);
            result[0] = new Hero(CW2, 300f);




            return result;
        }


    }
}
