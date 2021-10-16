using System;
using System.Collections.Generic;
using System.Text;

namespace APIExerciseTwo
{
    interface ICitiesRepository
    {
        IEnumerable<City> GetAllCities();

        IEnumerable<City> CitiesByName(string name);

        IEnumerable<City> CitiesByState(string state);

        IEnumerable<City> CitiesByState(State state);

        IEnumerable<City> CitiesByZip(string zip);

        IEnumerable<State> GetState(string state);

    }
}
