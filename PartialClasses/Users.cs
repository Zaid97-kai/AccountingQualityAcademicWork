//------------------------------------------------------------------------------
namespace AccountingQualityAcademicWork.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Users
    {
        public string FullName
        {
            get { return Surname + " " + Name + " " + Patronymic; }
        }
    }
}
