CREATE TABLE [dbo].[CategoryValueTranslation](
	[CategoryValueId] [int] NOT NULL,
	[IsoLanguageId] [int] NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
)

GO

ALTER TABLE [dbo].[CategoryValueTranslation]  WITH NOCHECK ADD  CONSTRAINT [FK.CategoryValueTranslation_app.CategoryValue_CategoryValueId] FOREIGN KEY([CategoryValueId])
REFERENCES [dbo].[CategoryValue] ([Id])
GO

ALTER TABLE [dbo].[CategoryValueTranslation] CHECK CONSTRAINT [FK.CategoryValueTranslation_app.CategoryValue_CategoryValueId]
GO

ALTER TABLE [dbo].[CategoryValueTranslation]  WITH NOCHECK ADD  CONSTRAINT [FK.CategoryValueTranslation_app.IsoLanguage_IsoLanguageId] FOREIGN KEY([IsoLanguageId])
REFERENCES [dbo].[IsoLanguage] ([Id])
GO

ALTER TABLE [dbo].[CategoryValueTranslation] CHECK CONSTRAINT [FK.CategoryValueTranslation_app.IsoLanguage_IsoLanguageId]
GO


