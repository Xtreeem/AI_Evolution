using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class HeroGenerator
    {
        //List<Actor> _breedList;
         public HeroGenerator()
        {

        }

        public List<Hero> Breed(List<Actor> BreedList)
        {
            List<Hero> newGeneration = new List<Hero>();
            for (int i = 0; i<BreedList.Count();i+=2)
            {
                newGeneration.Add( Breed_These(BreedList[i], BreedList[i + 1]));
                newGeneration.Add(Breed_These(BreedList[i+1], BreedList[i]));
            }
            return newGeneration;
        }

        private Hero Breed_These(Actor FirstGenes, Actor SecondGenes)
        {
           //Stats stats = new Stats(FirstGenes.Stats.Strength,FirstGenes.Stats.Dexterity,FirstGenes.Stats.Constitution,SecondGenes.Stats.Intelligence,SecondGenes.Stats.Wisdom,SecondGenes.Stats.Faith,SecondGenes.Stats.Perception);
           return new Hero(Weighing_Stats(FirstGenes,SecondGenes));
        }

        //Ej komplett
        private Stats Weighing_Stats(Actor FirstGenes, Actor SecondGenes)
        {
            //Parent1 Str/200 = prio på str
            float str = FirstGenes.Stats.Strength;
            float dex = FirstGenes.Stats.Dexterity;
            float con = FirstGenes.Stats.Constitution;
            float intell = SecondGenes.Stats.Intelligence;
            float wis = SecondGenes.Stats.Wisdom;
            float faith = SecondGenes.Stats.Faith;
            float perc = SecondGenes.Stats.Perception;
            float together = str + dex + con + intell + wis + faith + perc;
            float corrVal;
            if(together > 200)
            {
                 corrVal = -(together % 200)/7;//Om det sammanlaggda värdet på statsen överstiger poolen så räknas här ut hur mycket <corrVal>(correctionValue)
            }
            else
            {
                corrVal = (200 - together) / 7;//Om det sammanlaggda värdet på statsen är under poolen ellse lika med så räknas här framm ett korrigerings värde <corrVal>(correctionValue)
            }

            Stats stats = new Stats(str + corrVal, dex + corrVal, con + corrVal, intell + corrVal, wis + corrVal, faith + corrVal, perc + corrVal);//Här korrigeras värdena med corrVal.

            return stats;
        }

        //Ej komplett
        public List<Hero> Rando_Heroes()
        {
            return new List<Hero>();
        }
        //Ej komplett
        private Hero Make_Rando_Hero()
        {
            float str = Misc.Random.Next(0,200);
            float dex = Misc.Random.Next(0, 200);
            float con = Misc.Random.Next(0, 200);
            float intell = Misc.Random.Next(0, 200);
            float wis = Misc.Random.Next(0, 200);
            float faith = Misc.Random.Next(0, 200);
            float perc = Misc.Random.Next(0, 200);
            float together = str + dex + con + intell + wis + faith + perc;
            float corrVal;
            if (together > 200)
            {
                corrVal = -(together % 200) / 7;//Om det sammanlaggda värdet på statsen överstiger poolen så räknas här ut hur mycket <corrVal>(correctionValue)
            }
            else
            {
                corrVal = (200 - together) / 7;//Om det sammanlaggda värdet på statsen är under poolen ellse lika med så räknas här framm ett korrigerings värde <corrVal>(correctionValue)
            }

            Stats stats = new Stats(str + corrVal, dex + corrVal, con + corrVal, intell + corrVal, wis + corrVal, faith + corrVal, perc + corrVal);//Här korrigeras värdena med corrVal.

            
            return new Hero(stats);
        }
    }
}
