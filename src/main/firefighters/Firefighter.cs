using Firefighters.api;
using System;

namespace Firefighters.firefighters
{
  public class Firefighter : api.IFirefighter
  {
    public CityNode currentLocation;
    public int distance;
    
    public Firefighter(CityNode firestation)
    {
      currentLocation = firestation;
    }

    public CityNode GetLocation()
    {
      return currentLocation;
    }

    public int DistanceTraveled()
    {
      return distance;
    }
  }
}
