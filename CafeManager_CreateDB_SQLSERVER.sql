CREATE DATABASE CafeManagerApp;
USE CafeManagerApp;

------------------- Tạo bảng user -----------------------
CREATE TABLE AppUser (
    AppUserId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    Role INT DEFAULT 0, -- 1 = Admin, 0 = Staff
    DisplayName VARCHAR(100) DEFAULT 'Unknown',
    Avatar VARCHAR(MAX), -- Thay TEXT bằng VARCHAR(MAX)
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng Staff -----------------------
CREATE TABLE Staff (
    StaffId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    StaffName VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    Sex CHAR(1),
    Birthday DATE,
    StartWorking DATE,
    Role VARCHAR(50),
    WorkingStatus VARCHAR(50),
    BasicSalary DECIMAL(10, 2),
    WorkingHours DECIMAL(10,2),
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng FoodCategory -----------------------
CREATE TABLE FoodCategory (
    FoodCategoryId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    FoodCategoryName VARCHAR(100) NOT NULL,
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng Food -----------------------
CREATE TABLE Food (
    FoodId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    FoodName VARCHAR(100) NOT NULL,
    FoodCategoryId INT NOT NULL,
    Price DECIMAL(10, 2) DEFAULT 0,
    ImageFood VARCHAR(MAX), -- Thay TEXT bằng VARCHAR(MAX)
    DiscountFood DECIMAL(5, 2) DEFAULT 0,
    IsDeleted BIT DEFAULT 0, -- Thay BOOLEAN bằng BIT
    FOREIGN KEY (FoodCategoryId) REFERENCES FoodCategory(FoodCategoryId)
);

------------------- Tạo bảng Table -----------------------
CREATE TABLE CoffeeTable (
    CoffeeTableId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    StatusTable VARCHAR(50) DEFAULT 'Trống', -- "Available", "Occupied"
    NOTES VARCHAR(MAX), -- Thay TEXT bằng VARCHAR(MAX)
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng Invoices -----------------------
CREATE TABLE Invoices (
    InvoiceId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    PaymentStartDate DATETIME NOT NULL,
    PaymentEndDate DATETIME,
    PaymentStatus VARCHAR(50) DEFAULT 'Chưa thanh toán', 
    PaymentMethod VARCHAR(50) DEFAULT 'Thanh toán tiền mặt',
    DiscountInvoice DECIMAL(5, 2) DEFAULT 0,
    CoffeeTableId INT,
    IsDeleted BIT DEFAULT 0, -- Thay BOOLEAN bằng BIT
    FOREIGN KEY (CoffeeTableId) REFERENCES CoffeeTable(CoffeeTableId)
);

------------------- Tạo bảng InvoiceDetails -----------------------
CREATE TABLE InvoiceDetails (
    InvoiceDetailId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    InvoiceId INT,
    FoodId INT NOT NULL,
    Quantity INT DEFAULT 0,
    IsDeleted BIT DEFAULT 0, -- Thay BOOLEAN bằng BIT
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    FOREIGN KEY (FoodId) REFERENCES Food(FoodId)
);

------------------- Tạo bảng Supplier -----------------------
CREATE TABLE Supplier (
    SupplierId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    SupplierName VARCHAR(100) NOT NULL,
    RepresentativeSupplier VARCHAR(100) NOT NULL, -- Thay VARCHAR bằng VARCHAR(100)
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    Address VARCHAR(255),
    Notes VARCHAR(MAX), -- Thay TEXT bằng VARCHAR(MAX)
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng Material -----------------------
CREATE TABLE Material (
    MaterialId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    MaterialName VARCHAR(100) NOT NULL,
    Unit VARCHAR(50),
    ExpiryDate DATE,
    Price DECIMAL(10, 2) DEFAULT 0,
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng Material_Supplier -----------------------
CREATE TABLE MaterialSupplier (
    MaterialSupplierId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    MaterialId INT NOT NULL,
    SupplierId INT NOT NULL,
    FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);

------------------- Tạo bảng ConsumedMaterials -----------------------
CREATE TABLE ConsumedMaterials (
    ConsumedMaterialId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    MaterialId INT,
    Quantity DECIMAL(10, 2) DEFAULT 0, -- Số lượng nguyên liệu đã tiêu thụ
    Unit VARCHAR(50),
    IsDeleted BIT DEFAULT 0, -- Thay BOOLEAN bằng BIT
    FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId)
);

------------------- Tạo bảng Imports -----------------------
CREATE TABLE Imports (
    ImportId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    ImportDate DATE,
    DeliveryRepresentative VARCHAR(50) NOT NULL, 
    Phone VARCHAR(20) NOT NULL,
    IsDeleted BIT DEFAULT 0 -- Thay BOOLEAN bằng BIT
);

------------------- Tạo bảng ImportDetails -----------------------
CREATE TABLE ImportDetails (
    ImportDetailId INT IDENTITY(1,1) PRIMARY KEY, -- Thay SERIAL bằng INT IDENTITY
    ImportId INT NOT NULL,
    MaterialId INT NOT NULL,
    SupplierId INT NOT NULL,
    Quantity INT DEFAULT 0,
    IsDeleted BIT DEFAULT 0, -- Thay BOOLEAN bằng BIT
    FOREIGN KEY (ImportId) REFERENCES Imports(ImportId),
    FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);
