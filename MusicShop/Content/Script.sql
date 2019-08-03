CREATE PROCEDURE uspCreateGuitarTable
AS 
CREATE TABLE dbo.Guitars 
( 
	GuitarId INT IDENTITY(1,1)  NOT NULL,
	GuitarName VARCHAR(50) NOT NULL,
	GuitarImage NVARCHAR(MAX) NOT NULL,
	Price DECIMAL(6,2) NOT NULL,
	ItemType VARCHAR(10) NOT NULL,
	Stock INT NOT NULL,
	Brand VARCHAR(20) NOT NULL,
	 
	
	CONSTRAINT PK_Guitar PRIMARY KEY (GuitarId)
)
GO

CREATE PROCEDURE uspCreateCustomersTable
AS 
CREATE TABLE dbo.Customers
( 
	CustomerEmail VARCHAR(50) NOT NULL,
	CustomerName VARCHAR(40) NOT NULL, 
	Pass VARCHAR(80) NOT NULL,
	Phone VARCHAR(20) NOT NULL,
	DateRegistered DATE NOT NULL,
	AddressLine1 VARCHAR(40) NOT NULL,
	AddressLine2 VARCHAR(40) NOT NULL,
	City VARCHAR(40) NOT NULL,
	Country VARCHAR(40) NOT NULL

	CONSTRAINT PK_Customer PRIMARY KEY (CustomerEmail)
)
GO

CREATE PROCEDURE uspCreateStaffTable
AS 
CREATE TABLE dbo.Staff
( 
	StaffEmail VARCHAR(50) NOT NULL,
	Pass VARCHAR(80) NOT NULL,
	

	CONSTRAINT PK_Staff PRIMARY KEY (StaffEmail)
)
GO

CREATE PROCEDURE uspCreateOrdersTable
AS 
CREATE TABLE dbo.Orders
( 
	OrderId INT IDENTITY(1,1) NOT NULL,
	CustomerEmail VARCHAR(50) NOT NULL,  
	DateOfOrder DATETIME NOT NULL,

	CONSTRAINT PK_Orders PRIMARY KEY (OrderId),
	CONSTRAINT FK_OrderCustomer FOREIGN KEY (CustomerEmail) REFERENCES Customers (CustomerEmail) 
)
GO

CREATE PROCEDURE uspCreateOrderItemTable
AS 
CREATE TABLE dbo.OrderItem
( 
	OrderId INT NOT NULL,
	GuitarId INT NOT NULL,
	Quantity INT NOT NULL,
	Price DECIMAL(6,2) NOT NULL,	

	CONSTRAINT PK_OrderItem PRIMARY KEY (GuitarId, OrderId),
	CONSTRAINT FK_OrderItem_Orders FOREIGN KEY (OrderId) REFERENCES Orders (OrderId),
	CONSTRAINT FK_OrderItem_Guitars FOREIGN KEY (GuitarId) REFERENCES Guitars (GuitarId) 
)
GO

CREATE PROC uspInsertGuitar
	
	@Name VARCHAR(50),
	@Image VARCHAR(100),
	@Price DECIMAL(6,2),
	@Stock INT,
	@ItemType VARCHAR(10),
	@Brand VARCHAR(20)
	
AS
INSERT INTO dbo.Guitars (GuitarName, GuitarImage, Price, Stock, ItemType, Brand)
	VALUES( @Name, @Image, @Price, @Stock, @ItemType, @Brand)
GO

CREATE PROC uspShowAllGuitars
AS
SELECT * FROM dbo.Guitars
GO

CREATE PROC uspShowOneGuitar
	@GuitarId INT
AS
SELECT * FROM dbo.Guitars WHERE GuitarId = @GuitarId
GO

CREATE PROC uspDeleteGuitar
@GuitarId INT 
AS
DELETE FROM dbo.Guitars
WHERE GuitarId = @GuitarId
GO
 
CREATE PROC uspEditGuitar
@GuitarId INT,
@Name VARCHAR(50),
@Price DECIMAL(6,2),
@Stock INT,
@Image NVARCHAR(MAX),
@ItemType VARCHAR(10),
@Brand VARCHAR(20)

AS 
UPDATE  dbo.Guitars
SET 
GuitarName = @Name,
Price = @Price,
Stock = @Stock,
GuitarImage = @Image,
ItemType = @ItemType,
Brand = @Brand
WHERE GuitarId = @GuitarId
Go


CREATE PROC uspInserttblCustomer
	@custEmail VARCHAR(50),
	@custName VARCHAR(40), 
	@pass VARCHAR(80),
	@Phone VARCHAR(20),
	@addressLine1 VARCHAR(40),
	@addressLine2 VARCHAR(40),
	@city VARCHAR(40),
	@country VARCHAR(40)
	
AS
INSERT INTO dbo.Customers 
	(CustomerEmail, CustomerName, DateRegistered, Pass,	Phone,  AddressLine1,
	AddressLine2, City, Country)
	VALUES
		(@custEmail, @custName, GetDate(), @pass, @Phone,  @addressLine1,
		@addressLine2, @city, @country)
GO

CREATE PROC uspUpdateCustomer
	@custEmail VARCHAR(50),
	@custName VARCHAR(40), 
	@pass VARCHAR(80),
	@Phone VARCHAR(20),
	@DateRegistered DATE,
	@addressLine1 VARCHAR(40),
	@addressLine2 VARCHAR(40),
	@city VARCHAR(40),
	@country VARCHAR(40)
	
AS
UPDATE dbo.Customers 
SET
	CustomerName =@custName,	 
	DateRegistered = @DateRegistered, 
	Pass = @pass,	
	Phone = @Phone,  
	AddressLine1 = @addressLine1,
	AddressLine2 = @addressLine2, 
	City = @city, 
	Country = @country
WHERE
	CustomerEmail = @custEmail
GO


CREATE PROC uspShowAllCustomers
AS
SELECT * FROM dbo.Customers
GO

CREATE PROC uspShowOneCustomer
	@custEmail VARCHAR(50)
	
AS
SELECT * FROM dbo.Customers WHERE CustomerEmail = @custEmail
GO

CREATE PROC uspDeleteCustomer
@CustomerEmail VARCHAR(50)
AS
DELETE FROM dbo.Customers
WHERE CustomerEmail = @CustomerEmail
GO

CREATE PROC uspInserttblStaff
	@email VARCHAR(50),
	@pass VARCHAR(80)

AS
INSERT INTO dbo.Staff (StaffEmail, Pass)
	VALUES(@email, @pass)
GO

CREATE PROC uspUpdateStaff
	@email VARCHAR(50),
	@pass VARCHAR(80)
	
AS
UPDATE  dbo.Staff 
SET 
	Pass = @pass
	
WHERE StaffEmail = @email
GO

CREATE PROC uspSelectAllStaff
AS
SELECT * FROM dbo.Staff
GO

CREATE PROC uspSelectOneStaff
	@email VARCHAR(50)
AS
SELECT * FROM dbo.Staff WHERE StaffEmail = @email
GO

CREATE PROC uspDeleteOneStaff
@email VARCHAR(50)
AS
DELETE FROM dbo.Staff
WHERE StaffEmail = @email
GO

CREATE PROC uspInsertOrder
@email VARCHAR(50)

AS
INSERT INTO Orders (CustomerEmail, DateOfOrder) 
VALUES (@email, GETDATE())
GO

CREATE PROC uspInsertOrderItem
@orderId INT OUTPUT,
@guitarId INT ,
@quantity INT , 
@price DECIMAL(6,2) 
AS
INSERT INTO OrderItem (OrderId, GuitarId, Quantity, Price)
VALUES (@orderId, @guitarId, @quantity, @price)
GO

CREATE PROCEDURE uspSelectOneOrder
	@orderId INT
AS
SELECT * FROM Orders
WHERE OrderId = @orderId
GO

CREATE PROCEDURE uspGetCustomerOrders
	@email VARCHAR(50)
AS
SELECT * FROM Orders
WHERE CustomerEmail = @email
GO

CREATE PROCEDURE uspGetAllOrders
AS
SELECT * FROM Orders
GO

CREATE PROCEDURE uspGetOrderItems
	@orderId INT
AS 
SELECT * FROM OrderItem 
WHERE OrderId = @orderId
GO


CREATE PROCEDURE uspCurrentOrder AS
SELECT IDENT_CURRENT('Orders') AS ID
GO

CREATE PROCEDURE uspGetMostRecentOrderIDForCustomer 
	@email VARCHAR(50)
AS
SELECT TOP 1 OrderID 
    FROM Orders
	WHERE CustomerEmail = @email
	ORDER BY DateOfOrder DESC
GO


EXEC uspCreateGuitarTable
EXEC uspCreateCustomersTable
EXEC uspCreateStaffTable
EXEC uspCreateOrdersTable
EXEC uspCreateOrderItemTable


EXEC uspInserttblStaff @email = "a@b.c", @pass = "admin";


