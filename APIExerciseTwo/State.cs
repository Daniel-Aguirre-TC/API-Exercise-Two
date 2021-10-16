using System;
using System.Collections.Generic;
using System.Text;

namespace APIExerciseTwo
{
    class State
    {
        public int ID { get; set; }
        public string Name { get; set; }

        IEnumerable<ZipCode> ZipCodes { get; set; }

        IEnumerable<City> Cities { get; set; }

    }
}
