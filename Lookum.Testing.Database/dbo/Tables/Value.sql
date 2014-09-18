CREATE TABLE [dbo].[CategoryValue](
	[Id] [int] NOT NULL,
	[CategoryTypeId] [int] NOT NULL,
	[Key] [nvarchar](50) NOT NULL
 CONSTRAINT [PK.CategoryValue] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CategoryValue]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.CategoryValue_app.CategoryType_CategoryTypeId] FOREIGN KEY([CategoryTypeId])
REFERENCES [dbo].[CategoryType] ([Id])
GO

ALTER TABLE [dbo].[CategoryValue] CHECK CONSTRAINT [FK_dbo.CategoryValue_app.CategoryType_CategoryTypeId]
GO


