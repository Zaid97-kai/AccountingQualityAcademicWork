//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountingQualityAcademicWork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportCard
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReportCard()
        {
            this.StudentInReportCard = new HashSet<StudentInReportCard>();
        }
    
        public int Id { get; set; }
        public string NameDiscipline { get; set; }
        public System.DateTime DateFilling { get; set; }
        public Nullable<int> IdSpecialization { get; set; }
        public Nullable<int> IdGroup { get; set; }
        public Nullable<int> IdUsers { get; set; }
        public Nullable<int> NumberLectures { get; set; }
        public Nullable<int> NumberPractical { get; set; }
        public Nullable<int> NumberLabs { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentInReportCard> StudentInReportCard { get; set; }
    }
}
