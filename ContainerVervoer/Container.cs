using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerVervoer
{
    public class Container
    {
        //properties
        public int Weight { get; private set; }
        public int MaxWeight { get; private set; }
        public int MinWeight { get; private set; }
        public bool Valuable { get; private set; }
        public bool Cooled { get; private set; }

        //constructors
        public Container(int weight, bool valuable, bool cooled)
        {
            Weight = weight;
            Valuable = valuable;
            Cooled = cooled;
            MinWeight = 4000;
            MaxWeight = 30000;
            if (weight < MinWeight)
            {
                throw new InvalidOperationException("The weight of the container is too low. It needs to be at least 4000 kg");
            }
        }
    }
}
