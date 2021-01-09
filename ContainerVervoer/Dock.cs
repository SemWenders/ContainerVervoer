using System;
using System.Collections.Generic;

namespace ContainerVervoer
{
    public class Dock
    {
        //properties
        public Ship Ship { get; private set; }
        public List<Container> ContainersToBeAdded { get; private set; }

        //constructors
        public Dock(Ship ship)
        {
            Ship = ship;
            ContainersToBeAdded = new List<Container>();
        }

        //methods
        public void CreateContainer(int weight, bool valuable, bool cooled)
        {
            Container container = new Container(weight, valuable, cooled);
            ContainersToBeAdded.Add(container);
        }

        public void DeleteContainer(Container container)
        {
            ContainersToBeAdded.Remove(container);
        }

        public int TotalContainersWeight()
        {
            int totalWeight = 0;
            foreach (var container in ContainersToBeAdded)
            {
                totalWeight += container.Weight;
            }
            return totalWeight;
        }

        public void PlaceContainers()
        {
            foreach (var containerStack in Ship.ContainerStacks)
            {
                List<Container> cooledContainers = ContainersToBeAdded.FindAll(c => c.Cooled);
                foreach (var container in cooledContainers)
                {
                    if (containerStack.IsContainerAble(container, Ship.Columns))
                    {
                        containerStack.AddContainerToStack(container);
                        ContainersToBeAdded.Remove(container);
                    }
                }

                List<Container> regularContainers = ContainersToBeAdded.FindAll(c => !c.Cooled && !c.Valuable);
                foreach (var container in regularContainers)
                {
                    if (containerStack.IsContainerAble(container, Ship.Columns))
                    {
                        containerStack.AddContainerToStack(container);
                        ContainersToBeAdded.Remove(container);
                    }
                }

                List<Container> valuedContainers = ContainersToBeAdded.FindAll(c => c.Valuable);
                foreach (var container in valuedContainers)
                {
                    if (containerStack.IsContainerAble(container, Ship.Columns))
                    {
                        containerStack.AddContainerToStack(container);
                        ContainersToBeAdded.Remove(container);
                    }
                }
            }
        }

        public void Balance()
        {
            Side heavySide = Side.Left;
            Side lightSide = Side.Left;
            int heavySideWeight = 0;
            int lightSideWeight = 0;

            if (Ship.WeightLeftSide() > Ship.WeightRightSide())
            {
                heavySide = Side.Left;
                lightSide = Side.Right;

                heavySideWeight = Ship.WeightLeftSide();
                lightSideWeight = Ship.WeightRightSide();
            }

            else if (Ship.WeightRightSide() > Ship.WeightLeftSide())
            {
                heavySide = Side.Right;
                lightSide = Side.Left;

                heavySideWeight = Ship.WeightRightSide();
                lightSideWeight = Ship.WeightLeftSide();
            }

            int difference = heavySideWeight - lightSideWeight;

            List<ContainerStack> heavyContainerStacks = Ship.ContainerStacks.FindAll(c => c.Side == heavySide);
            List<ContainerStack> lightContainerStacks = Ship.ContainerStacks.FindAll(c => c.Side == lightSide);

            foreach (var heavyContainerStack in heavyContainerStacks)
            {
                foreach (var lightContainerStack in lightContainerStacks)
                {
                    foreach (var container in heavyContainerStack.GetContainers())
                    {
                        if (lightContainerStack.IsContainerAble(container, Ship.Columns))
                        {
                            heavyContainerStack.RemoveContainer(container);
                            lightContainerStack.AddContainerToStack(container);
                            return;
                        }
                    }
                }
            }
        }

        public void Calculate()
        {
            if (TotalContainersWeight() > Ship.MaxWeight)
            {
                throw new InvalidOperationException("The containers are too heavy");
            }

            //valuable containers can only be placed on the first or the last column and can't be stacked, so there's a limited amount
            int valuableContainers = ContainersToBeAdded.FindAll(c => c.Valuable).Count;
            if (valuableContainers > (2 * Ship.Rows))
            {
                throw new InvalidOperationException("You have added too many valuable containers, we can't add them all on the ship");
            }

            PlaceContainers();

            while (!Ship.IsShipBalanced())
            {
                Balance();
            }
        }
    }
}
