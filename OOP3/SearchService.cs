using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OOP3
{
    public static class SearchService
    {
        public static IEnumerable<Student> SearchByNameRegex(List<Student> students, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return students.Where(s => regex.IsMatch(s.Name));
        }

        public static IEnumerable<Position> SearchPositionBySellaryRange(List<Work> works, float min, float max)
        {
            return works.SelectMany(w => w._positions)
                        .Where(p => float.TryParse(p.Sellary, out float value) && value >= min && value <= max);
        }
    }

}
