using Firefighters.api;
using Firefighters.impls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Firefighters.scenarios
{
  [TestClass]
  public class BasicScenarios
  {
    [TestMethod]
    public void SingleFire()
    {
      ICity basicCity = new City(5, 5, new CityNode(0, 0));
      IFireDispatch fireDispatch = basicCity.GetFireDispatch();

      CityNode fireNode = new CityNode(0, 1);
      Pyromaniac.SetFire(basicCity, fireNode);

      fireDispatch.SetFirefighters(1);
      fireDispatch.DispatchFirefighers(fireNode);
      Assert.IsFalse(basicCity.GetBuilding(fireNode).IsBurning());
    }

    [TestMethod]
    public void SingleFireDistanceTraveledDiagonal()
    {
      ICity basicCity = new City(2, 2, new CityNode(0, 0));
      IFireDispatch fireDispatch = basicCity.GetFireDispatch();

      // Set fire on opposite corner from Fire Station
      CityNode fireNode = new CityNode(1, 1);
      Pyromaniac.SetFire(basicCity, fireNode);

      fireDispatch.SetFirefighters(1);
      fireDispatch.DispatchFirefighers(fireNode);

      IFirefighter firefighter = fireDispatch.GetFirefighters()[0];
      Assert.AreEqual(2, firefighter.DistanceTraveled());
      Assert.AreEqual(fireNode, firefighter.GetLocation());
    }

    [TestMethod]
    public void SingleFireDistanceTraveledAdjacent()
    {
      ICity basicCity = new City(2, 2, new CityNode(0, 0));
      IFireDispatch fireDispatch = basicCity.GetFireDispatch();

      // Set fire on adjacent X position from Fire Station
      CityNode fireNode = new CityNode(1, 0);
      Pyromaniac.SetFire(basicCity, fireNode);

      fireDispatch.SetFirefighters(1);
      fireDispatch.DispatchFirefighers(fireNode);

      IFirefighter firefighter = fireDispatch.GetFirefighters()[0];
      Assert.AreEqual(1, firefighter.DistanceTraveled());
      Assert.AreEqual(fireNode, firefighter.GetLocation());
    }

    [TestMethod]
    public void SimpleDoubleFire()
    {
      ICity basicCity = new City(2, 2, new CityNode(0, 0));
      IFireDispatch fireDispatch = basicCity.GetFireDispatch();

      CityNode[] fireNodes = {
        new CityNode(0, 1),
        new CityNode(1, 1)};
      Pyromaniac.SetFires(basicCity, fireNodes);

      fireDispatch.SetFirefighters(1);
      fireDispatch.DispatchFirefighers(fireNodes);

      IFirefighter firefighter = fireDispatch.GetFirefighters()[0];
      Assert.AreEqual(2, firefighter.DistanceTraveled());
      Assert.AreEqual(fireNodes[1], firefighter.GetLocation());
      Assert.IsFalse(basicCity.GetBuilding(fireNodes[0]).IsBurning());
      Assert.IsFalse(basicCity.GetBuilding(fireNodes[1]).IsBurning());
    }

    [TestMethod]
    public void DoubleFirefighterDoubleFire()
    {
      ICity basicCity = new City(2, 2, new CityNode(0, 0));
      IFireDispatch fireDispatch = basicCity.GetFireDispatch();

      CityNode[] fireNodes = {
        new CityNode(0, 1),
        new CityNode(1, 0)};
      Pyromaniac.SetFires(basicCity, fireNodes);

      fireDispatch.SetFirefighters(2);
      fireDispatch.DispatchFirefighers(fireNodes);

      IList<IFirefighter> firefighters = fireDispatch.GetFirefighters();
      int totalDistanceTraveled = 0;
      bool firefighterPresentAtFireOne = false;
      bool firefighterPresentAtFireTwo = false;
      foreach (IFirefighter firefighter in firefighters)
      {
        totalDistanceTraveled += firefighter.DistanceTraveled();

        if (firefighter.GetLocation() == fireNodes[0])
        {
          firefighterPresentAtFireOne = true;
        }
        if (firefighter.GetLocation() == fireNodes[1])
        {
          firefighterPresentAtFireTwo = true;
        }
      }

      Assert.AreEqual(2, totalDistanceTraveled);
      Assert.IsTrue(firefighterPresentAtFireOne);
      Assert.IsTrue(firefighterPresentAtFireTwo);
      Assert.IsFalse(basicCity.GetBuilding(fireNodes[0]).IsBurning());
      Assert.IsFalse(basicCity.GetBuilding(fireNodes[1]).IsBurning());
    }
  }
}
