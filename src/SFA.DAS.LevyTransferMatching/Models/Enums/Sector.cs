using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum Sector
    {
        [Display(Name = "Agriculture, environmental and animal care", Description = "For example, Veterinary Nurse, Horticulture and Landscape Operative, Equine Groom, Golf Greenkeeper")]
        Agriculture = 1,
        [Display(Name = "Business and administration", Description = "For example, Team Leader or Supervisor, Operations or Departmental Manager, Business Administrator, Senior Leader (degree)")]
        Business = 2,
        [Display(Name = "Care services", Description = "For example, Lead Adult Care Worker, Adult Care Worker, Children, Young People and Families Practitioner, Children, Young People and Families Manager")]
        CareServices = 4,
        [Display(Name = "Catering and hospitality", Description = "For example, Hospitality Team Member, Hospitality Supervisor, Commis Chef, Production Chef")]
        Catering = 8,
        [Display(Name = "Charity", Description = "")]
        Charity = 16,
        [Display(Name = "Construction", Description = "For example, Installation Electrician or Maintenance Electrician, Carpentry and Joinery, Chartered Surveyor (degree), Bricklayer")]
        Construction = 32,
        [Display(Name = "Creative and design", Description = "For example, Junior Content Producer, Junior Journalist, Creative Venue Technician, Broadcast Production Assistant")]
        Creative = 64,
        [Display(Name = "Digital", Description = "For example, Infrastructure Technician, Digital and Technology Solutions Professional, Data Analyst, Software Developer")]
        Digital = 128,
        [Display(Name = "Education and childcare", Description = "For example, Early Years Educator, Teaching Assistant, Academic Professional, Learning and Skills Teacher")]
        Education = 256,
        [Display(Name = "Engineering and manufacturing", Description = "For example, Engineering Technician, Motor Vehicle Service and Maintenance Technician, Maintenance and Operations Engineering Technician, Heavy Vehicle Service and Maintenance Technician")]
        Engineering = 512,
        [Display(Name = "Hair and beauty", Description = "For example, Hair Professional, Beauty Therapist, Nail Services Technician")]
        Hair = 1024,
        [Display(Name = "Health and science", Description = "For example, Senior Healthcare Support Worker, Dental Nurse, Nursing Associate, Healthcare Support Worker")]
        Health = 2048,
        [Display(Name = "Legal, finance and accounting", Description = "For example, Accountancy or Taxation Professional, Assistant Accountant, Professional Accounting or Taxation Technician, Insurance Practitioner")]
        Legal = 4096,
        [Display(Name = "Protective services", Description = "For example, HM Forces Serviceperson, Police Constable, Operational Firefighter, Custody and Detention Officer")]
        ProtectiveServices = 8192,
        [Display(Name = "Sales, marketing and procurement", Description = "For example, Customer Service Practitioner, Retailer, Digital Marketer, Retail Team Leader")]
        Sales = 16384,
        [Display(Name = "Transport and logistics", Description = "For example, Large Goods Vehicle (LGV) Driver, Supply Chain Warehouse Operative, Passenger Transport Driver, Cabin Crew")]
        Transport = 32768,
    }
}