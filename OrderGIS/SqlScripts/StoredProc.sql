-- Add new order
CREATE PROCEDURE sp_AddOrder
    @CustomerId INT,
    @GISAssetId NVARCHAR(50),
    @Status NVARCHAR(50) = 'Pending',
    @NewOrderId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Orders (CustomerId, GISAssetId, Status, CreatedAt)
    VALUES (@CustomerId, @GISAssetId, @Status, SYSUTCDATETIME());

    SET @NewOrderId = SCOPE_IDENTITY();
END
GO

-- Get order by ID
CREATE PROCEDURE sp_GetOrderById
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.OrderId, o.CustomerId, o.GISAssetId, o.Status, o.CreatedAt, o.UpdatedAt,
           c.Name AS CustomerName, c.Email AS CustomerEmail
    FROM Orders o
    INNER JOIN Customers c ON o.CustomerId = c.CustomerId
    WHERE o.OrderId = @OrderId;
END
GO

-- Update order status
CREATE PROCEDURE sp_UpdateOrderStatus
    @OrderId INT,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Orders
    SET Status = @Status,
        UpdatedAt = SYSUTCDATETIME()
    WHERE OrderId = @OrderId;
END
GO
