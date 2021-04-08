CREATE PROCEDURE [dbo].[sp_Product_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock
	FROM dbo.Products
	ORDER BY ProductName;
END
