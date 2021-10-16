using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace APIExerciseTwo
{
    class CitiesRepository : ICitiesRepository
    {

        private readonly IDbConnection _connection;

        // constructor takes in the IDbConnection to use for accessing the database.
        public CitiesRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<City> GetAllCities()
        {
            return _connection.Query<City>("SELECT * FROM Cities");
        }

        public IEnumerable<City> CitiesByName(string name)
        {
            return _connection.Query<City>("SELECT * FROM Cities WHERE Name LIKE @name",
                new {name = name + "%"});          
        }

        public IEnumerable<City> CitiesByState(string state)
        {
            return _connection.Query<City>("SELECT * FROM Cities WHERE State LIKE @State",
                new { State = state + "%" });
        }

        public IEnumerable<City> CitiesByState(State state)
        {
            return CitiesByState(state.StateName);
        }

        public IEnumerable<City> CitiesByZip(string zip)
        {
            return _connection.Query<City>("SELECT * FROM Cities WHERE Zip LIKE @Zip",
                new { Zip = zip + "%" });
        }

       public IEnumerable<State> GetState(string state)
       {
           return _connection.Query<State>("SELECT * FROM states WHERE StateName LIKE @State",
                new { State = state + "%" });
       }


    }
}
