using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingQualityAcademicWork.PartialClasses
{
    public class GenerateAttestationListItem
    {
        public string Name { get; set; }
        public List<int> Scores { get; set; } = new List<int>();
        public List<string> Disciplines { get; set; } = new List<string>();
    }
}
