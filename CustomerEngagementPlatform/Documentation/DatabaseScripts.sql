-- Database script for Customer Engagement Platform
-- by Saumya

-- DDL reference for Customers table
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL
);

-- DDL reference for Tickets table
CREATE TABLE Tickets (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Subject NVARCHAR(120) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    Priority NVARCHAR(30) NULL,
    AssignedAgent NVARCHAR(80) NULL,
    CreatedAt DATETIME2 NOT NULL,
    UpdatedAt DATETIME2 NULL,
    CustomerId INT NOT NULL,
    CONSTRAINT FK_Tickets_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id)
);

-- Sample insert for Customers
INSERT INTO Customers (Name, Email, Phone, Address, CreatedAt)
VALUES ('Sample Customer', 'customer@example.com', '9999999999', 'Sample Address', GETDATE());

-- Sample insert for Tickets
INSERT INTO Tickets (Subject, Description, Status, Priority, AssignedAgent, CreatedAt, CustomerId)
VALUES ('Login Issue', 'Customer is unable to login.', 'Open', 'High', 'Support Team', GETDATE(), 1);

-- View all customers
SELECT * FROM Customers;

-- View all tickets
SELECT * FROM Tickets;

-- Join customers with their tickets
SELECT 
    c.Id AS CustomerId,
    c.Name AS CustomerName,
    c.Email,
    t.Id AS TicketId,
    t.Subject,
    t.Status,
    t.Priority,
    t.AssignedAgent,
    t.CreatedAt
FROM Customers c
INNER JOIN Tickets t 
ON c.Id = t.CustomerId;

-- Count tickets by status
SELECT 
    Status, 
    COUNT(*) AS TotalTickets 
FROM Tickets 
GROUP BY Status;

-- Count tickets by priority
SELECT 
    Priority, 
    COUNT(*) AS TotalTickets 
FROM Tickets 
GROUP BY Priority;