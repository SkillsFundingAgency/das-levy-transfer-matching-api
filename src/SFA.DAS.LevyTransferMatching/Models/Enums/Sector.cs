using System.ComponentModel.DataAnnotations;
using SFA.DAS.LevyTransferMatching.Attributes;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum Sector
    {
        [ReferenceMetadata(Description = "Agriculture, environmental and animal care", Hint = "For example, Veterinary Nurse, Horticulture and Landscape Operative, Equine Groom, Golf Greenkeeper")]
        Agriculture = 1,
        [ReferenceMetadata(Description = "Business and administration", Hint = "For example, Team Leader or Supervisor, Operations or Departmental Manager, Business Administrator, Senior Leader (degree)")]
        Business = 2,
        [ReferenceMetadata(Description = "Care services", Hint = "For example, Lead Adult Care Worker, Adult Care Worker, Children, Young People and Families Practitioner, Children, Young People and Families Manager")]
        CareServices = 4,
        [ReferenceMetadata(Description = "Catering and hospitality", Hint = "For example, Hospitality Team Member, Hospitality Supervisor, Commis Chef, Production Chef")]
        Catering = 8,
        [ReferenceMetadata(Description = "Charity", Hint = "")]
        Charity = 16,
        [ReferenceMetadata(Description = "Construction", Hint = "For example, Installation Electrician or Maintenance Electrician, Carpentry and Joinery, Chartered Surveyor (degree), Bricklayer")]
        Construction = 32,
        [ReferenceMetadata(Description = "Creative and design", Hint = "For example, Junior Content Producer, Junior Journalist, Creative Venue Technician, Broadcast Production Assistant")]
        Creative = 64,
        [ReferenceMetadata(Description = "Digital", Hint = "For example, Infrastructure Technician, Digital and Technology Solutions Professional, Data Analyst, Software Developer")]
        Digital = 128,
        [ReferenceMetadata(Description = "Education and childcare", Hint = "For example, Early Years Educator, Teaching Assistant, Academic Professional, Learning and Skills Teacher")]
        Education = 256,
        [ReferenceMetadata(Description = "Engineering and manufacturing", Hint = "For example, Engineering Technician, Motor Vehicle Service and Maintenance Technician, Maintenance and Operations Engineering Technician, Heavy Vehicle Service and Maintenance Technician")]
        Engineering = 512,
        [ReferenceMetadata(Description = "Hair and beauty", Hint = "For example, Hair Professional, Beauty Therapist, Nail Services Technician")]
        Hair = 1024,
        [ReferenceMetadata(Description = "Health and science", Hint = "For example, Senior Healthcare Support Worker, Dental Nurse, Nursing Associate, Healthcare Support Worker")]
        Health = 2048,
        [ReferenceMetadata(Description = "Legal, finance and accounting", Hint = "For example, Accountancy or Taxation Professional, Assistant Accountant, Professional Accounting or Taxation Technician, Insurance Practitioner")]
        Legal = 4096,
        [ReferenceMetadata(Description = "Protective services", Hint = "For example, HM Forces Serviceperson, Police Constable, Operational Firefighter, Custody and Detention Officer")]
        ProtectiveServices = 8192,
        [ReferenceMetadata(Description = "Sales, marketing and procurement", Hint = "For example, Customer Service Practitioner, Retailer, Digital Marketer, Retail Team Leader")]
        Sales = 16384,
        [ReferenceMetadata(Description = "Transport and logistics", Hint = "For example, Large Goods Vehicle (LGV) Driver, Supply Chain Warehouse Operative, Passenger Transport Driver, Cabin Crew")]
        Transport = 32768,
    }
}