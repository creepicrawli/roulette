-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,Adam Daniels>
-- Create date: <Create Date,,16/02/2023>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE pr_GetOrderSummary
 @StartDate datetime ,
 @EndDate datetime,
 @CustomerID nchar(5),
 @EmployeeID int
AS
SELECT 
	CONCAT( TitleOfCourtesy,' ',FirstName,' ', LastName) AS EmployeeFullName, 
	c.CompanyName as CustomerCompanyName,
	s.CompanyName as ShipperCompanyName, 
	COUNT(o.OrderID) as NumberOfOrders,
	o.OrderDate as [Date],
	SUM(o.Freight) as TotalFreightCost,
	COUNT(DISTINCT od.ProductID) as NumberOfDifferentProducts,
	SUM(os.Subtotal) as TotalOrderValue
FROM Orders As o
	INNER JOIN [Order Details] As  od ON o.OrderID = od.OrderID
	INNER JOIN Employees As e ON o.EmployeeID = e.EmployeeID
	INNER JOIN Customers AS c ON o.CustomerID = c.CustomerID
	INNER JOIN Shippers as s ON o.ShipVia = s.ShipperID
	INNER JOIN [Order Subtotals] as os  ON o.OrderID = os.OrderID
WHERE   (o.OrderDate BETWEEN @StartDate AND @EndDate)
		AND o.CustomerID = ISNULL(NULLIF(@CustomerID,''),o.CustomerID)
		AND o.EmployeeID = ISNULL(NULLIF(@EmployeeID,''),o.EmployeeID)
		
GROUP BY e.TitleOfCourtesy, e.FirstName,e.LastName,
c.CompanyName, o.OrderDate, s.CompanyName
GO
