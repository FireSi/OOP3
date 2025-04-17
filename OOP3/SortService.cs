using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{    public static class SortService
    {
        public static IEnumerable<Student> SortByNameThenMark(List<Student> students)
        {
            return students.OrderBy(s => s.Name)
                           .ThenByDescending(s => s.AvgMark);
        }

        public static IEnumerable<Position> SortBySellary(List<Work> works)
        {
            return works.SelectMany(work => work._positions)
                        .OrderByDescending(position => position.Sellary);
        }
    }
}
