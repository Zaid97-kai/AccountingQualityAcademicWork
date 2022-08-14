using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingQualityAcademicWork.Models
{
    public partial class JournalDBEntities : DbContext
    {
        private static JournalDBEntities journalDBEntities;
        public static JournalDBEntities GetContext()
        {
            if(journalDBEntities == null)
            {
                journalDBEntities = new JournalDBEntities();
            }
            return journalDBEntities;
        }
    }
}
