CREATE PROCEDURE [dbo].[sp_Product_GetById]
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable
	FROM dbo.Products
	WHERE Id = @id;
END
