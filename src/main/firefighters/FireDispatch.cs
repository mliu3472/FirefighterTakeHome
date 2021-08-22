using Firefighters.api;
using System;
using System.Collections.Generic;

namespace Firefighters.firefighters
{
  public class FireDispatch : api.IFireDispatch
  {
    private List<Firefighter> firefighters;
    private readonly ICity city;

    public FireDispatch(ICity city)
    {
      this.city = city;
      this.firefighters = new List<Firefighter>();
    }

    // Sets a list of firefighters that each start at the firestation
    public void SetFirefighters(int numFirefighters)
    {
      var firestation = city.GetFireStation().GetLocation();
      for (int i = 1; i <= numFirefighters; i++)
      {
        firefighters.Add(new Firefighter(firestation));
      }
    }

    // Converts the list of firefighters to the list interface
    public IList<IFirefighter> GetFirefighters()
    {
      IList<IFirefighter> result = new List<IFirefighter>();
      foreach(var firefighter in this.firefighters)
      {
        result.Add(firefighter);
      }
      return result;
    }

    // For each burning building find the closest firefighter to extinguish
    // the fire. Update that firefighter's distance travelled and current location.
    public void DispatchFirefighers(params CityNode[] burningBuildings)
    {
      foreach(var building in burningBuildings)
      {
        // dispatch the closest firefighter
        var minDistance = GetDistance(firefighters[0].GetLocation(), building);
        var closestFirefighter = firefighters[0];
        var firefighterList = firefighters.GetRange(1, firefighters.Count - 1);
        foreach(var firefighter in firefighterList)
        {
            var firefighterLocation = firefighter.GetLocation();
            var distance = GetDistance(firefighterLocation, building);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestFirefighter = firefighter;
            }
        }

        // update the firefighter's distance traveled and current location
        closestFirefighter.distance += GetDistance(closestFirefighter.GetLocation(), building);
        closestFirefighter.currentLocation = building;

        // put out the fire
        city.GetBuilding(building).ExtinguishFire();
      }
    }

    private int GetDistance(CityNode node1, CityNode node2)
    {
        var distance = Math.Abs(node1.GetX() - node2.GetX()) + Math.Abs(node1.GetY() - node2.GetY());
        return distance;
    }
  }
}
