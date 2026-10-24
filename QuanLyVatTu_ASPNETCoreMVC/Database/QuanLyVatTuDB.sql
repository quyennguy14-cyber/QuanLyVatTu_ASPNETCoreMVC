    IF DB_ID(N'QuanLyVatTuDB') IS NULL CREATE DATABASE QuanLyVatTuDB;
    GO
    USE QuanLyVatTuDB;
    GO

    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL UNIQUE,
        Description NVARCHAR(300) NULL
    );
    CREATE TABLE Suppliers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(150) NOT NULL,
        Phone NVARCHAR(20) NULL,
        Email NVARCHAR(120) NULL,
        Address NVARCHAR(250) NULL
    );
    CREATE TABLE Materials (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(20) NOT NULL UNIQUE,
        Name NVARCHAR(150) NOT NULL,
        Unit NVARCHAR(30) NOT NULL,
        Quantity INT NOT NULL DEFAULT 0 CHECK (Quantity >= 0),
        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (UnitPrice >= 0),
        MinStock INT NOT NULL DEFAULT 5 CHECK (MinStock >= 0),
        CategoryId INT NOT NULL REFERENCES Categories(Id),
        SupplierId INT NOT NULL REFERENCES Suppliers(Id)
    );
    CREATE TABLE Employees (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Phone NVARCHAR(20) NULL,
        Department NVARCHAR(100) NULL
    );
    CREATE TABLE AppUsers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(128) NOT NULL,
        FullName NVARCHAR(100) NOT NULL,
        Role NVARCHAR(20) NOT NULL DEFAULT 'Staff'
    );
    CREATE TABLE Receipts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(30) NOT NULL UNIQUE,
        Date DATETIME2 NOT NULL,
        Note NVARCHAR(250) NULL
    );
    CREATE TABLE ReceiptDetails (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ReceiptId INT NOT NULL REFERENCES Receipts(Id) ON DELETE CASCADE,
        MaterialId INT NOT NULL REFERENCES Materials(Id),
        Quantity INT NOT NULL CHECK (Quantity > 0),
        UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0)
    );
    CREATE TABLE Issues (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(30) NOT NULL UNIQUE,
        Date DATETIME2 NOT NULL,
        Note NVARCHAR(250) NULL
    );
    CREATE TABLE IssueDetails (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        IssueId INT NOT NULL REFERENCES Issues(Id) ON DELETE CASCADE,
        MaterialId INT NOT NULL REFERENCES Materials(Id),
        Quantity INT NOT NULL CHECK (Quantity > 0),
        UnitPrice DECIMAL(18,2) NOT NULL CHECK (UnitPrice >= 0)
    );
    GO

    INSERT INTO Categories(Name, Description) VALUES
    (N'Vật tư điện', N'Dây điện, công tắc, thiết bị điện'),
    (N'Vật tư xây dựng', N'Xi măng, cát, thép'),
    (N'Văn phòng phẩm', N'Giấy, bút và vật dụng văn phòng');
    INSERT INTO Suppliers(Name, Phone, Email, Address) VALUES
    (N'Công ty Vật tư Minh Phát', N'0900000001', N'minhphat@example.com', N'Hà Nội');
    INSERT INTO Materials(Code, Name, Unit, Quantity, UnitPrice, MinStock, CategoryId, SupplierId)
    VALUES
    (N'VT001', N'Dây điện 2.5mm', N'Cuộn', 20, 150000, 5, 1, 1),
    (N'VT002', N'Công tắc điện', N'Cái', 50, 25000, 10, 1, 1);
    GO
    -- Tài khoản admin được ứng dụng tạo tự động nếu bảng AppUsers chưa có dữ liệu.
