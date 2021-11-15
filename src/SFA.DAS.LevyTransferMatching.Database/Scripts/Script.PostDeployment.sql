﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/* DashbordReporting Role Access */

IF DATABASE_PRINCIPAL_ID('DashbordReporting') IS NULL
BEGIN
    CREATE ROLE [DashbordReporting]
END

GRANT SELECT ON vwDashboardPledge TO DashbordReporting
GRANT SELECT ON vwDashboardPledgeApplication TO DashbordReporting


/* TM-203 - Close a specific pledge */
update Pledge set Status=1 where Id = 51 and Status=0

/* TM-204-Backfill-Standard-Info */
UPDATE [Application]  SET [StandardTitle] = 'Carpentry and joinery',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 11000  where [StandardTitle] = ''  AND [StandardId] = 'ST0264_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Commis chef',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0228_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Advanced carpentry and joinery',   [StandardLevel] = 3,   [StandardDuration] = 15,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 11000  where [StandardTitle] = ''  AND [StandardId] = 'ST0263_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Hospitality supervisor',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0230_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Senior production chef',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0232_1.3'
UPDATE [Application]  SET [StandardTitle] = 'Hospitality team member',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0233_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Housing and property management',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0234_1.1'
UPDATE [Application]  SET [StandardTitle] = 'HR support',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 4500  where [StandardTitle] = ''  AND [StandardId] = 'ST0239_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Land-based service engineering technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0243_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Large goods vehicle (LGV) driver C + E',   [StandardLevel] = 2,   [StandardDuration] = 13,   [StandardRoute] = 'Transport and logistics',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0257_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Maintenance and operations engineering technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 26000  where [StandardTitle] = ''  AND [StandardId] = 'ST0154_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Food and drink process operator',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0199_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Gas network operative',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 13000  where [StandardTitle] = ''  AND [StandardId] = 'ST0204_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Hair professional',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Hair and beauty',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0213_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Senior healthcare support worker',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0217_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Assistant accountant',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0002_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Professional accounting or taxation technician',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0003_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Highways electrician or service operative',   [StandardLevel] = 3,   [StandardDuration] = 24,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0052_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Heavy vehicle service and maintenance technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0068_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Customer service practitioner',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 3500  where [StandardTitle] = ''  AND [StandardId] = 'ST0072_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Civil engineering technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 14000  where [StandardTitle] = ''  AND [StandardId] = 'ST0091_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Dental nurse (integrated)',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0113_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Digital and technology solutions professional (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 25000  where [StandardTitle] = ''  AND [StandardId] = 'ST0119_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Digital marketer',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 11000  where [StandardTitle] = ''  AND [StandardId] = 'ST0122_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Chartered manager (degree)',   [StandardLevel] = 6,   [StandardDuration] = 48,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 22000  where [StandardTitle] = ''  AND [StandardId] = 'ST0272_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Associate project manager',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0310_1.3'
UPDATE [Application]  SET [StandardTitle] = 'Accident repair technician',   [StandardLevel] = 3,   [StandardDuration] = 24,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0352_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Team leader or supervisor',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 4500  where [StandardTitle] = ''  AND [StandardId] = 'ST0384_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Operations or departmental manager',   [StandardLevel] = 5,   [StandardDuration] = 30,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0385_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Engineering technician',   [StandardLevel] = 3,   [StandardDuration] = 42,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 26000  where [StandardTitle] = ''  AND [StandardId] = 'ST0457_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Senior leader',   [StandardLevel] = 7,   [StandardDuration] = 24,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 14000  where [StandardTitle] = ''  AND [StandardId] = 'ST0480_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Architectural assistant (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 48,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 25000  where [StandardTitle] = ''  AND [StandardId] = 'ST0534_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Ambulance support worker (emergency, urgent and non-urgent)',   [StandardLevel] = 3,   [StandardDuration] = 13,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0627_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Community activator coach',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0478_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Paramedic (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 25000  where [StandardTitle] = ''  AND [StandardId] = 'ST0567_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Train driver',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Transport and logistics',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0645_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Teacher',   [StandardLevel] = 6,   [StandardDuration] = 12,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0490_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Autocare technician',   [StandardLevel] = 2,   [StandardDuration] = 30,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0499_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Senior insurance professional',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0520_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Food and drink advanced engineer (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 60,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 24000  where [StandardTitle] = ''  AND [StandardId] = 'ST0529_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Optical assistant',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0530_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Architect (integrated degree)',   [StandardLevel] = 7,   [StandardDuration] = 48,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0533_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Teaching assistant',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0454_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Post graduate engineer',   [StandardLevel] = 7,   [StandardDuration] = 30,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 27000  where [StandardTitle] = ''  AND [StandardId] = 'ST0456_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Business to business sales professional (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0423_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Rehabilitation worker (visual impairment)',   [StandardLevel] = 5,   [StandardDuration] = 24,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0431_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Plumbing and domestic heating technician',   [StandardLevel] = 3,   [StandardDuration] = 48,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0303_1.0'
UPDATE [Application]  SET [StandardTitle] = 'General welder (arc processes)',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0349_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Project manager (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 48,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 22000  where [StandardTitle] = ''  AND [StandardId] = 'ST0411_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Recruitment consultant',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0320_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Recruitment resourcer',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0321_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Junior estate agent',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0329_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Chartered surveyor (degree)',   [StandardLevel] = 6,   [StandardDuration] = 60,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 27000  where [StandardTitle] = ''  AND [StandardId] = 'ST0331_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Associate ambulance practitioner',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0287_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Painter and decorator',   [StandardLevel] = 2,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0295_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Infrastructure technician',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0125_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Accountancy or taxation professional',   [StandardLevel] = 7,   [StandardDuration] = 36,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0001_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Highway electrical maintenance and installation operative',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0051_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Learning and skills teacher',   [StandardLevel] = 5,   [StandardDuration] = 24,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 10000  where [StandardTitle] = ''  AND [StandardId] = 'ST0149_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Installation electrician and maintenance electrician',   [StandardLevel] = 3,   [StandardDuration] = 42,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST0152_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Software developer',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST0116_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Business analyst',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST0117_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Data analyst',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0118_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Community sport and health officer',   [StandardLevel] = 3,   [StandardDuration] = 16,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0093_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Bricklayer',   [StandardLevel] = 2,   [StandardDuration] = 30,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0095_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Butcher',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0078_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Children, young people and families manager',   [StandardLevel] = 5,   [StandardDuration] = 24,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0087_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Children, young people and families practitioner',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0088_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Business administrator',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0070_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Customer service specialist',   [StandardLevel] = 3,   [StandardDuration] = 15,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0071_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Adult care worker',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 3000  where [StandardTitle] = ''  AND [StandardId] = 'ST0005_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Lead adult care worker',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 3000  where [StandardTitle] = ''  AND [StandardId] = 'ST0006_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Arborist',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0223_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Horticulture or landscape operative',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0225_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Chef de partie',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0227_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Healthcare assistant practitioner',   [StandardLevel] = 5,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0215_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Healthcare support worker',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 3000  where [StandardTitle] = ''  AND [StandardId] = 'ST0216_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Sports turf operative',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0210_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Furniture manufacturer',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0203_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Equine groom',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0166_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Event assistant',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0168_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Property maintenance operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0171_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Financial services administrator',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0177_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Mortgage adviser',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0182_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Fire emergency and security systems technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST0189_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Supply chain warehouse operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Transport and logistics',   [StandardMaxFunding] = 3000  where [StandardTitle] = ''  AND [StandardId] = 'ST0259_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Junior content producer',   [StandardLevel] = 3,   [StandardDuration] = 12,   [StandardRoute] = 'Creative and design',   [StandardMaxFunding] = 12000  where [StandardTitle] = ''  AND [StandardId] = 'ST0105_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Hospitality manager',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0229_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Digital engineering technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0266_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Roofer',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 11000  where [StandardTitle] = ''  AND [StandardId] = 'ST0270_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Construction assembly and installation operative',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 13000  where [StandardTitle] = ''  AND [StandardId] = 'ST0265_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Advanced golf greenkeeper',   [StandardLevel] = 3,   [StandardDuration] = 24,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0207_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Advanced beauty therapist',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Hair and beauty',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0211_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Lead practitioner in adult care',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0007_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Leader in adult care',   [StandardLevel] = 5,   [StandardDuration] = 18,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0008_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Stockperson (beef, pigs, sheep, dairy)',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 10000  where [StandardTitle] = ''  AND [StandardId] = 'ST0017_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Crop technician',   [StandardLevel] = 3,   [StandardDuration] = 24,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0018_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Construction quantity surveyor (degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST0045_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Construction site supervisor',   [StandardLevel] = 4,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0048_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Construction quantity surveying technician',   [StandardLevel] = 4,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0049_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Plasterer',   [StandardLevel] = 2,   [StandardDuration] = 36,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 10000  where [StandardTitle] = ''  AND [StandardId] = 'ST0096_2.0'
UPDATE [Application]  SET [StandardTitle] = 'Early years educator',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0135_1.2'
UPDATE [Application]  SET [StandardTitle] = 'Assessor coach',   [StandardLevel] = 4,   [StandardDuration] = 15,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0146_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Pharmacy technician (integrated)',   [StandardLevel] = 3,   [StandardDuration] = 24,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0300_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Personal trainer',   [StandardLevel] = 3,   [StandardDuration] = 15,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0302_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Digital community manager',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 13000  where [StandardTitle] = ''  AND [StandardId] = 'ST0345_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Lean manufacturing operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0420_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Interior systems installer',   [StandardLevel] = 2,   [StandardDuration] = 24,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 14000  where [StandardTitle] = ''  AND [StandardId] = 'ST0388_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Animal care and welfare assistant',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0397_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Engineering fitter',   [StandardLevel] = 3,   [StandardDuration] = 42,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0432_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Vehicle damage paint technician',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0448_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Smart home technician',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0464_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Digital user experience (UX) professional (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 48,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 24000  where [StandardTitle] = ''  AND [StandardId] = 'ST0470_1.0'
UPDATE [Application]  SET [StandardTitle] = 'IT solutions technician',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 13000  where [StandardTitle] = ''  AND [StandardId] = 'ST0505_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Digital and technology solutions specialist (integrated degree)',   [StandardLevel] = 7,   [StandardDuration] = 18,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0482_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Process leader',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 11000  where [StandardTitle] = ''  AND [StandardId] = 'ST0695_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Historic environment advice assistant',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Creative and design',   [StandardMaxFunding] = 10000  where [StandardTitle] = ''  AND [StandardId] = 'ST0749_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Information manager',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Business and administration',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0762_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Sports coach',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0770_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Marketing assistant',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0807_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Nursing associate (NMC 2018)',   [StandardLevel] = 5,   [StandardDuration] = 24,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0827_1.1'
UPDATE [Application]  SET [StandardTitle] = 'Telecoms field operative',   [StandardLevel] = 2,   [StandardDuration] = 15,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0832_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Engineering manufacturing technician',   [StandardLevel] = 4,   [StandardDuration] = 42,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 21000  where [StandardTitle] = ''  AND [StandardId] = 'ST0841_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Healthcare cleaning operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Health and science',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0843_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Plate welder',   [StandardLevel] = 3,   [StandardDuration] = 36,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 27000  where [StandardTitle] = ''  AND [StandardId] = 'ST0852_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Early intervention practitioner',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 3500  where [StandardTitle] = ''  AND [StandardId] = 'ST0868_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Fundraiser',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0887_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Early years practitioner',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 4000  where [StandardTitle] = ''  AND [StandardId] = 'ST0888_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Youth support worker',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Care services',   [StandardMaxFunding] = 4500  where [StandardTitle] = ''  AND [StandardId] = 'ST0906_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Junior advertising creative',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Creative and design',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0925_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Dog groomer',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Agriculture, environmental and animal care',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0943_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Information communications technician',   [StandardLevel] = 3,   [StandardDuration] = 18,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 15000  where [StandardTitle] = ''  AND [StandardId] = 'ST0973_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Cyber security technologist (2021)',   [StandardLevel] = 4,   [StandardDuration] = 24,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 18000  where [StandardTitle] = ''  AND [StandardId] = 'ST1021_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Sales executive',   [StandardLevel] = 4,   [StandardDuration] = 18,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0572_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Data scientist (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 19000  where [StandardTitle] = ''  AND [StandardId] = 'ST0585_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Production chef',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Catering and hospitality',   [StandardMaxFunding] = 5000  where [StandardTitle] = ''  AND [StandardId] = 'ST0589_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Marina and boatyard operative',   [StandardLevel] = 2,   [StandardDuration] = 18,   [StandardRoute] = 'Transport and logistics',   [StandardMaxFunding] = 10000  where [StandardTitle] = ''  AND [StandardId] = 'ST0592_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Marketing executive',   [StandardLevel] = 4,   [StandardDuration] = 15,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0596_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Accounts or finance assistant',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Legal, finance and accounting',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0608_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Marketing manager',   [StandardLevel] = 6,   [StandardDuration] = 24,   [StandardRoute] = 'Sales, marketing and procurement',   [StandardMaxFunding] = 9000  where [StandardTitle] = ''  AND [StandardId] = 'ST0612_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Facilities services operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Construction',   [StandardMaxFunding] = 3000  where [StandardTitle] = ''  AND [StandardId] = 'ST0617_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Creative digital design professional (integrated degree)',   [StandardLevel] = 6,   [StandardDuration] = 36,   [StandardRoute] = 'Digital',   [StandardMaxFunding] = 25000  where [StandardTitle] = ''  AND [StandardId] = 'ST0625_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Beauty therapist',   [StandardLevel] = 2,   [StandardDuration] = 15,   [StandardRoute] = 'Hair and beauty',   [StandardMaxFunding] = 7000  where [StandardTitle] = ''  AND [StandardId] = 'ST0630_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Engineering operative',   [StandardLevel] = 2,   [StandardDuration] = 12,   [StandardRoute] = 'Engineering and manufacturing',   [StandardMaxFunding] = 6000  where [StandardTitle] = ''  AND [StandardId] = 'ST0537_1.0'
UPDATE [Application]  SET [StandardTitle] = 'Early years lead practitioner',   [StandardLevel] = 5,   [StandardDuration] = 24,   [StandardRoute] = 'Education and childcare',   [StandardMaxFunding] = 8000  where [StandardTitle] = ''  AND [StandardId] = 'ST0551_1.0'


update Application
set TotalAmount = NumberOfApprentices * StandardMaxFunding
where TotalAmount = 0
