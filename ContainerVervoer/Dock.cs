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
                PlaceCooledContainers(containerStack);

                PlaceRegularContainers(containerStack);

                PlaceValuableContainers(containerStack);
            }
        }

        private void PlaceCooledContainers(ContainerStack containerStack)
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
        }

        private void PlaceRegularContainers(ContainerStack containerStack)
        {
            List<Container> regularContainers = ContainersToBeAdded.FindAll(c => !c.Cooled && !c.Valuable);
            foreach (var container in regularContainers)
            {
                if (containerStack.IsContainerAble(container, Ship.Columns))
                {
                    containerStack.AddContainerToStack(container);
                    ContainersToBeAdded.Remove(container);
                }
            }
        }

        private void PlaceValuableContainers(ContainerStack containerStack)
        {
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

        public void BalanceTheShip()
        {
            Side heavySide = GetHeavySide();
            Side lightSide = GetLightSide();

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

        private Side GetHeavySide()
        {
            if (Ship.WeightLeftSide() > Ship.WeightRightSide())
            {
                return Side.Left;
            }

            else
            {
                return Side.Right;
            }
        }

        private Side GetLightSide()
        {
            if (Ship.WeightLeftSide() < Ship.WeightRightSide())
            {
                return Side.Left;
            }

            else
            {
                return Side.Right;
            }
        }

        public void CalculateContainerPlacement()
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
                BalanceTheShip();
            }
        }
    }
}
