using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GetRoutes.Model
{
    public class Pupil
    {
        public int Id { get; set; }
        public string Class { get; set; } = default!;
        public string Lastname { get; set; } = default!;
        public string Firstname { get; set; } = default!;
        public string Gender { get; set; } = default!;
    }
}
