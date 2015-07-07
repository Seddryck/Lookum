/*
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
truncate table [CategoryValueTranslation];
truncate table [MultiKeyValue];
truncate table [State];
delete from [CategoryValue];
delete from  [CategoryType];
delete from  [IsoLanguage];


insert into [IsoLanguage] values(1, 'en', 1, 'English');
insert into [IsoLanguage] values(2, 'fr', 0, 'French');


insert into [CategoryType] values(1, 'Country', 'List of countries');
insert into [CategoryType] values(2, 'Currency', 'List of currencies');


insert into [CategoryValue] values(1, 1, 'US');
insert into [CategoryValue] values(2, 1, 'FR');
insert into [CategoryValue] values(3, 1, 'BE');
insert into [CategoryValue] values(4, 1, 'NL');
insert into [CategoryValue] values(5, 1, 'GE');
insert into [CategoryValue] values(6, 2, 'USD');
insert into [CategoryValue] values(7, 2, 'EUR');


insert into [CategoryValueTranslation] values(1, 1, 'United States of America');
insert into [CategoryValueTranslation] values(2, 1, 'France');
insert into [CategoryValueTranslation] values(3, 1, 'Belgium');
insert into [CategoryValueTranslation] values(4, 1, 'Netherlands');
insert into [CategoryValueTranslation] values(5, 1, 'Germany');
insert into [CategoryValueTranslation] values(6, 1, 'US Dollar');
insert into [CategoryValueTranslation] values(7, 1, 'Euro');
insert into [CategoryValueTranslation] values(1, 2, 'Etats-Unis');
insert into [CategoryValueTranslation] values(2, 2, 'France');
insert into [CategoryValueTranslation] values(3, 2, 'Belgique');
insert into [CategoryValueTranslation] values(4, 2, 'Pays-Bas');
insert into [CategoryValueTranslation] values(5, 2, 'Allemagne');
insert into [CategoryValueTranslation] values(6, 2, 'Dollar américain');
insert into [CategoryValueTranslation] values(7, 2, 'Euro');

insert into [MultiKeyValue] values('Training', 'Softskills', 'Soft skills training', 16);
insert into [MultiKeyValue] values('Training', '.Net', '.Net training', 16);
insert into [MultiKeyValue] values('Training', 'SQL', 'SQL training', 40);
insert into [MultiKeyValue] values('Coaching', 'Net', '.Net coaching', 40);
insert into [MultiKeyValue] values('Coaching', 'SQL',  'SQL voaoching', 80);
insert into [MultiKeyValue] values('Code review', '.Net',  '.Net vode review', 160);

insert into [State] values('Started');
insert into [State] values('Stopped');
