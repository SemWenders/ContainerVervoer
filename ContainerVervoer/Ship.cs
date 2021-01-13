using System;
using System.Collections.Generic;
using System.Text;

namespace ContainerVervoer
{
    public class Ship
    {
        //properties
        public int MinWeight { get; private set; }
        public int MaxWeight { get; private set; }
        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public List<ContainerStack> ContainerStacks { get; private set; }

        //constructors
        public Ship(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            MaxWeight = CalculateMaxWeight();
            MinWeight = MaxWeight / 2;
            ContainerStacks = new List<ContainerStack>();
            AddContainerStacks(); 
        }
        //methods
        public int CalculateMaxWeight()
        {
            int containerStacks = Rows * Columns;
            int weight = containerStacks * 150000;
            return weight;
        }
        public bool IsShipBalanced()
        {
            decimal heavySide;
            decimal lightSide;
            if (WeightLeftSide() > WeightRightSide())
            {
                heavySide = WeightLeftSide();
                lightSide = WeightRightSide();
            }

            else if (WeightRightSide() > WeightLeftSide())
            {
                heavySide = WeightRightSide();
                lightSide = WeightLeftSide();
            }

            else
            {
                return true;
            }

            decimal difference = (heavySide - lightSide) / (heavySide + lightSide) * 100;

            if (difference > 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void AddContainerStacks()
        {
            for (int i = 0; i < Columns; i++)
            {
                for (int ii = 0; ii < Convert.ToDecimal(Rows); ii++)
                {
                    Side side = DecideSideOfStack(ii);
                    ContainerStack containerStack = new ContainerStack(ii, i, side);
                    ContainerStacks.Add(containerStack);
                }
            }
        }

        private Side DecideSideOfStack(int row)
        {
            if (Rows % 2 == 0)
            {
                if (row % 2 == 0)
                {
                    return Side.Left;
                }
                else
                {
                    return Side.Right;
                }
            }

            else
            {
                if (row % 2 == 0)
                {
                    return Side.Left;
                }

                else
                {
                    if (row == ((Rows / 2) + 0.5))
                    {
                        return Side.Middle;
                    }

                    else
                    {
                        return Side.Right;
                    }
                }
            }
        }

        public int WeightLeftSide()
        {
            int weight = 0;
            List<ContainerStack> containerStacks = ContainerStacks.FindAll(c => c.Side == Side.Left);
            foreach (var containerStack in containerStacks)
            {
                weight += containerStack.GetStackWeight();
            }
            return weight;
        }

        public int WeightRightSide()
        {
            int weight = 0;
            List<ContainerStack> containerStacks = ContainerStacks.FindAll(c => c.Side == Side.Right);
            foreach (var containerStack in containerStacks)
            {
                weight += containerStack.GetStackWeight();
            }
            return weight;
        }
    }
}
