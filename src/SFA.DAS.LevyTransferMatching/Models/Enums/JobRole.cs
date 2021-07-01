using SFA.DAS.LevyTransferMatching.Attributes;

namespace SFA.DAS.LevyTransferMatching.Models.Enums
{
    public enum JobRole
    {
        [ReferenceMetadata(Description = "Agriculture, environmental and animal care", Hint = "For example, Veterinary Nurse, Horticulture and Landscape Operative, Equine Groom, Golf Greenkeeper")]
        Agriculture = 1,
        [ReferenceMetadata(Description = "Business and administration", Hint = "For example, Team Leader or Supervisor, Operations or Departmental Manager, Business Administrator, HR support")]
        Business = 2,
        [ReferenceMetadata(Description = "Care services", Hint = "For example, Social worker (degree), Adult Care Worker, Children, Young People and Families Practitioner, Youth support worker")]
        CareServices = 4,
        [ReferenceMetadata(Description = "Catering and hospitality", Hint = "For example, Hospitality Team Member, Hospitality Supervisor, Commis Chef, Production Chef, Baker")]
        Catering = 8,
        [ReferenceMetadata(Description = "Construction", Hint = "For example, Installation Electrician or Maintenance Electrician, Carpentry and Joinery, Chartered Surveyor (degree), Bricklayer")]
        Construction = 16,
        [ReferenceMetadata(Description = "Creative and design", Hint = "For example, Junior Content Producer, Junior Journalist, Creative Venue Technician, Broadcast Production Assistant")]
        Creative = 32,
        [ReferenceMetadata(Description = "Digital", Hint = "For example, Infrastructure Technician, Digital and Technology Solutions Professional, Data Analyst, Software Developer")]
        Digital = 64,
        [ReferenceMetadata(Description = "Education and childcare", Hint = "For example, Early Years Educator, Teaching Assistant, Academic Professional, Learning and Skills Teacher")]
        Education = 128,
        [ReferenceMetadata(Description = "Engineering and manufacturing", Hint = "For example, Engineering Technician, Motor Vehicle Service and Maintenance Technician, Metal fabricator, Aerospace engineer (degree)")]
        Engineering = 256,
        [ReferenceMetadata(Description = "Hair and beauty", Hint = "For example, Hair Professional, Beauty Therapist, Nail Services Technician")]
        Hair = 512,
        [ReferenceMetadata(Description = "Health and science", Hint = "For example, Healthcare Support Worker, Dental Nurse, Nursing Associate, Midwife (degree), Laboratory technician")]
        Health = 1024,
        [ReferenceMetadata(Description = "Legal, finance and accounting", Hint = "For example, Assistant Accountant, Mortgage adviser, Payroll administrator, Solicitor, Insurance Practitioner")]
        Legal = 2048,
        [ReferenceMetadata(Description = "Protective services", Hint = "For example, HM Forces Serviceperson, Police Constable, Operational Firefighter, Custody and Detention Officer")]
        ProtectiveServices = 4096,
        [ReferenceMetadata(Description = "Sales, marketing and procurement", Hint = "For example, Customer Service Practitioner, Digital Marketer, Retail Team Leader, Junior estate agent")]
        Sales = 8192,
        [ReferenceMetadata(Description = "Transport and logistics", Hint = "For example, Large Goods Vehicle (LGV) Driver, Supply Chain Warehouse Operative, Cabin Crew, Train driver")]
        Transport = 16384,
    }
}
