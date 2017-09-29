CREATE TABLE [dbo].[Sizes](
	[QuotaPaths] [nvarchar](255) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Size] [numeric](38, 0) NULL,
 CONSTRAINT [PK_Sizes] PRIMARY KEY CLUSTERED 
(
	[QuotaPaths] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sizes]  WITH CHECK ADD  CONSTRAINT [FK_Sizes_Dates] FOREIGN KEY([Date])
REFERENCES [dbo].[Dates] ([Date])
GO

ALTER TABLE [dbo].[Sizes] CHECK CONSTRAINT [FK_Sizes_Dates]
GO
ALTER TABLE [dbo].[Sizes]  WITH CHECK ADD  CONSTRAINT [FK_Sizes_QuotaPaths] FOREIGN KEY([QuotaPaths])
REFERENCES [dbo].[QuotaPaths] ([QuotaPath])
GO

ALTER TABLE [dbo].[Sizes] CHECK CONSTRAINT [FK_Sizes_QuotaPaths]