using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum JobRole
    {
        [Display(Name = "Agriculture, environmental and animal care", Description = "For example, Veterinary Nurse, Horticulture and Landscape Operative, Equine Groom, Golf Greenkeeper")]
        Agriculture = 1,
        [Display(Name = "Business and administration", Description = "For example, Team Leader or Supervisor, Operations or Departmental Manager, Business Administrator, HR support")]
        Business = 2,
        [Display(Name = "Care services", Description = "For example, Social worker (degree), Adult Care Worker, Children, Young People and Families Practitioner, Youth support worker")]
        CareServices = 4,
        [Display(Name = "Catering and hospitality", Description = "For example, Hospitality Team Member, Hospitality Supervisor, Commis Chef, Production Chef, Baker")]
        Catering = 8,
        [Display(Name = "Construction", Description = "For example, Installation Electrician or Maintenance Electrician, Carpentry and Joinery, Chartered Surveyor (degree), Bricklayer")]
        Construction = 16,
        [Display(Name = "Creative and design", Description = "For example, Junior Content Producer, Junior Journalist, Creative Venue Technician, Broadcast Production Assistant")]
        Creative = 32,
        [Display(Name = "Digital", Description = "For example, Infrastructure Technician, Digital and Technology Solutions Professional, Data Analyst, Software Developer")]
        Digital = 64,
        [Display(Name = "Education and childcare", Description = "For example, Early Years Educator, Teaching Assistant, Academic Professional, Learning and Skills Teacher")]
        Education = 128,
        [Display(Name = "Engineering and manufacturing", Description = "For example, Engineering Technician, Motor Vehicle Service and Maintenance Technician, Metal fabricator, Aerospace engineer (degree)")]
        Engineering = 256,
        [Display(Name = "Hair and beauty", Description = "For example, Hair Professional, Beauty Therapist, Nail Services Technician")]
        Hair = 512,
        [Display(Name = "Health and science", Description = "For example, Healthcare Support Worker, Dental Nurse, Nursing Associate, Midwife (degree), Laboratory technician")]
        Health = 1024,
        [Display(Name = "Legal, finance and accounting", Description = "For example, Assistant Accountant, Mortgage adviser, Payroll administrator, Solicitor, Insurance Practitioner")]
        Legal = 2048,
        [Display(Name = "Protective services", Description = "For example, HM Forces Serviceperson, Police Constable, Operational Firefighter, Custody and Detention Officer")]
        ProtectiveServices = 4096,
        [Display(Name = "Sales, marketing and procurement", Description = "For example, Customer Service Practitioner, Digital Marketer, Retail Team Leader, Junior estate agent")]
        Sales = 8192,
        [Display(Name = "Transport and logistics", Description = "For example, Large Goods Vehicle (LGV) Driver, Supply Chain Warehouse Operative, Cabin Crew, Train driver")]
        Transport = 16384,
    }
}
