using System;
using System.Collections.Generic;
using System.Text;

namespace APIExerciseTwo
{
    class ZipCode
    {
        int ID { get; set; }
        public int Zip { get; set; }

        State ParentState { get; set; }
        IEnumerable<City> Cities { get; set; }


    }
}
