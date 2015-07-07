CREATE TABLE [dbo].[MultiKeyValue]
(
	[Class] varchar(50) not null
	, [SubClass] varchar(50) not null
	, [Label] varchar(50) not null
	, [Quantity] int not null, 
    CONSTRAINT [PK_MultiKeyValue] PRIMARY KEY ([Class], [SubClass])
)
