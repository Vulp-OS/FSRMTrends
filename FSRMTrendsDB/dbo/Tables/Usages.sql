CREATE TABLE [dbo].[Usages](
	[QuotaPath] [nvarchar](255) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UsageBytes] [numeric](38, 0) NULL,
 CONSTRAINT [PK_Usages] PRIMARY KEY CLUSTERED 
(
	[QuotaPath] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usages]  WITH CHECK ADD  CONSTRAINT [FK_Usages_Dates] FOREIGN KEY([Date])
REFERENCES [dbo].[Dates] ([Date])
GO

ALTER TABLE [dbo].[Usages] CHECK CONSTRAINT [FK_Usages_Dates]
GO
ALTER TABLE [dbo].[Usages]  WITH CHECK ADD  CONSTRAINT [FK_Usages_QuotaPaths] FOREIGN KEY([QuotaPath])
REFERENCES [dbo].[QuotaPaths] ([QuotaPath])
GO

ALTER TABLE [dbo].[Usages] CHECK CONSTRAINT [FK_Usages_QuotaPaths]