
CREATE TABLE [dbo].[SalesDetailsComponants](
	[Flag] [bigint] NULL,
	[StoreId] [bigint] NULL,
	[InvoiceNo] [bigint] NULL,
	[DayDate] [datetime] NULL,
	[MainItemId] [bigint] NULL,
	[ItemId] [bigint] NULL,
	[ItemName] [nvarchar](100) NULL,
	[Qty] [float] NULL,
	[Value] [float] NULL,
	[AvgCost] [float] NULL,
	[Price] [float] NULL,
	[Line] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]

GO

