using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContainerVervoer;
using System.Collections.Generic;
using System;

namespace ContainerTests
{
    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void ContainerTest1()
        {
            Ship ship = new Ship(3, 5);
            Dock dock = new Dock(ship);
            for (int i = 0; i < 6; i++)
            {
                Container container1 = new Container(6000, false, false);
                dock.ContainersToBeAdded.Add(container1);
            }

            for (int i = 0; i < 3; i++)
            {
                Container container2 = new Container(9000, true, false);
                dock.ContainersToBeAdded.Add(container2);
            }

            for (int i = 0; i < 2; i++)
            {
                Container container3 = new Container(25000, false, true);
                dock.ContainersToBeAdded.Add(container3);
            }

            
            Container container = new Container(6000, true, true);
            dock.ContainersToBeAdded.Add(container);

            dock.CalculateContainerPlacement();

            decimal balancePercentage = CalculateDifferencePercentage(ship);
            Assert.AreEqual(balancePercentage < 0.20M, true);
        }

        [TestMethod]
        public void ContainersTest2()
        {
            Ship ship = new Ship(4, 6);
            Dock dock = new Dock(ship);

            Container container1 = new Container(4000, false, false);
            dock.ContainersToBeAdded.Add(container1);

            for (int i = 0; i < 5; i++)
            {
                Container container = new Container(25000, false, false);
                dock.ContainersToBeAdded.Add(container);
            }

            dock.CalculateContainerPlacement();
            decimal balancePercentage = CalculateDifferencePercentage(ship);
            Assert.AreEqual(balancePercentage < 0.20M, true);
            //check if the correct amount of containerstacks are used
            Assert.AreEqual(ship.ContainerStacks.FindAll(c => c.GetContainers().Count != 0).Count, 2);
        }

        private decimal CalculateDifferencePercentage(Ship ship)
        {
            int weightRight = 0;
            int weightLeft = 0;
            foreach (var containerStack in ship.ContainerStacks)
            {
                if (ship.Rows % 2 == 0)
                {
                    if (containerStack.Row % 2 == 0)
                    {
                        foreach (var leftContainer in containerStack.GetContainers())
                        {
                            weightLeft += leftContainer.Weight;
                        }
                    }

                    else
                    {
                        foreach (var rightContainer in containerStack.GetContainers())
                        {
                            weightRight += rightContainer.Weight;
                        }
                    }
                }

                else
                {
                    if (containerStack.Row % 2 == 0)
                    {
                        foreach (var leftContainer in containerStack.GetContainers())
                        {
                            weightLeft += leftContainer.Weight;
                        }
                    }

                    else if (containerStack.Row == (Convert.ToDecimal(ship.Rows) / 2) + 0.5M)
                    {
                        //don't add weight to any side because, the container stack is in the middle
                    }

                    else
                    {
                        foreach (var rightContainer in containerStack.GetContainers())
                        {
                            weightRight += rightContainer.Weight;
                        }
                    }
                }
            }

            decimal weightDifference = 0;
            if (weightLeft > weightRight)
            {
                weightDifference = weightLeft - weightRight;
            }

            else if (weightRight > weightLeft)
            {
                weightDifference = weightRight - weightLeft;
            }
            decimal balancePercentage = weightDifference / (weightLeft + weightRight);
            return balancePercentage;
        }
    }
}
