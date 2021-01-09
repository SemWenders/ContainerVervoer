using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerVervoer
{
    public class ContainerStack
    {
        //properties
        public int Row { get; private set; }
        public int Column { get; private set; }
        public Side Side { get; private set; }
        List<Container> containers;
        public bool HasValueContainer { get; private set; }
        public int MaxWeight { get; private set; }

        //constructors
        public List<Container> GetContainers()
        {
            return containers;
        }
        public ContainerStack(int row, int column, Side side)
        {
            Row = row;
            Column = column;
            Side = side;
            containers = new List<Container>();
            MaxWeight = 150000;
        }

        //methods
        public void AddContainerToStack(Container container)
        {
            containers.Add(container);
        }

        public void RemoveContainer(Container container)
        {
            containers.Remove(container);
        }

        public bool IsContainerAble(Container container, int maxColumn)
        {
            bool ability = false;
            if (WeightOnFirstContainer() + container.Weight <= 120000)
            {
                if (container.Cooled && Column == 0)
                {
                    ability = true;
                }
                else if (container.Cooled)
                {
                    return false;
                }

                if (container.Valuable && (Column == 0 || Column == maxColumn) && !HasValueContainer)
                {
                    ability = true;
                    HasValueContainer = true;
                }

                else if (container.Valuable)
                {
                    return false;
                }

                if (!container.Cooled && !container.Valuable)
                {
                    ability = true;
                }
            }
            else
            {
                ability = false;
            }

            return ability;
        }

        public int GetStackWeight()
        {
            int weight = 0;
            foreach (var container in containers)
            {
                weight += container.Weight;
            }

            return weight;
        }

        private int WeightOnFirstContainer()
        {
            int weight = 0;
            for (int i = 1; i < containers.Count; i++)
            {
                weight += containers[i].Weight;
            }
            return weight;
        }
    }
}
