CREATE TABLE [dbo].[SaleDetails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [SaleId] INT NOT NULL,
    [ProductId] INT NOT NULL,
    [Quantity] INT NOT NULL DEFAULT 1,
    [PurchasePrice] MONEY NOT NULL,
    [Tax] MONEY NOT NULL DEFAULT 0,
    CONSTRAINT [FK_SaleDetails_ToProducts] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_SaleDetails_ToSales] FOREIGN KEY ([SaleId]) REFERENCES [Sales]([Id]),
)
