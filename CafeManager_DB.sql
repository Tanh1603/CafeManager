------------------- Tạo bảng user -----------------------
CREATE TABLE AppUser (
    AppUserId SERIAL,
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    Role INT DEFAULT 0, -- 1 = Admin, 0 = Staff
    DisplayName VARCHAR(100),
    Avatar TEXT,
    IsDeleted BOOLEAN DEFAULT FALSE,

    CONSTRAINT PK_AppUserId PRIMARY KEY (AppUserId)
);
------------------- Tạo bảng Staff -----------------------
CREATE TABLE Staff (
    StaffId SERIAL,
    DisplayName VARCHAR(100),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Sex CHAR(1),
    Birthday DATE,
    StartWorking DATE,
    Role VARCHAR(50),
    WorkingStatus VARCHAR(50),
    BasicSalary DECIMAL(10, 2),
    WorkingHours DECIMAL(10,2),
    Salary DECIMAL(10, 2),
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Staff PRIMARY KEY (StaffId)
);
------------------- Tạo bảng FoodCategory -----------------------
CREATE TABLE FoodCategory (
    FoodCategoryId SERIAL,
    DisplayName VARCHAR(100) NOT NULL,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_FoodCategory PRIMARY KEY (FoodCategoryId)
);

------------------- Tạo bảng Food -----------------------
CREATE TABLE Food (
    FoodId SERIAL,
    DisplayName VARCHAR(100),
    FoodCategoryId INT,
    Price DECIMAL(10, 2),
    ImageFood BYTEA,
    DiscountFood DECIMAL(5, 2),
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Food PRIMARY KEY (FoodId),
    CONSTRAINT FK_Food_FoodCategory FOREIGN KEY (FoodCategoryId) REFERENCES FoodCategory(FoodCategoryId)
);
------------------- Tạo bảng Customer -----------------------
CREATE TABLE Customer (
    CustomerId SERIAL,
    DisplayName VARCHAR(100),
    BuyDate TIMESTAMP,
    Type VARCHAR(50), -- ( Vãng lai, thường xuyên , vip ... )
    TotalSpent DECIMAL(10, 2),
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Customer PRIMARY KEY (CustomerId)
);
------------------- Tạo bảng Invoices -----------------------
CREATE TABLE Invoices (
    InvoiceId SERIAL,
    PaymentDate TIMESTAMP,
    PaymentStatus VARCHAR(50), -- "Paid", "Pending"
    DiscountInvoice DECIMAL(5, 2),
    CoffeeTableId INT,
    CustomerId INT,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Invoices PRIMARY KEY (InvoiceId),
    CONSTRAINT FK_Invoices_CoffeeTable FOREIGN KEY (CoffeeTableId) REFERENCES CoffeeTable(CoffeeTableId),
    CONSTRAINT FK_Invoices_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(CustomerId)
);
------------------- Tạo bảng InvoiceDetails -----------------------
CREATE TABLE InvoiceDetails (
    InvoiceDetailId SERIAL,
    InvoiceId INT,
    FoodId INT,
    Quantity INT DEFAULT 0,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_InvoiceDetails PRIMARY KEY (InvoiceDetailId),
    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    CONSTRAINT FK_InvoiceDetails_Food FOREIGN KEY (FoodId) REFERENCES Food(FoodId)
);
------------------- Tạo bảng Table -----------------------
CREATE TABLE CoffeeTable (
    CoffeeTableId SERIAL,
    StatusTable VARCHAR(50), -- "Available", "Occupied"
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_CoffeeTable PRIMARY KEY (CoffeeTableId)
);
------------------- Tạo bảng Supplier -----------------------
CREATE TABLE Supplier (
    SupplierId SERIAL,
    DisplayName VARCHAR(100),
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address VARCHAR(255),
    ContractDate DATE,
    IsDeleted BOOLEAN DEFAULT FALSE,
    CONSTRAINT PK_Supplier PRIMARY KEY (SupplierId)
);
------------------- Tạo bảng Material -----------------------
CREATE TABLE Material (
    MaterialId SERIAL,
    DisplayName VARCHAR(100),
    Unit VARCHAR(50),
    ExpiryDate DATE,
    Price DECIMAL(10, 2),
    IsDeleted BOOLEAN DEFAULT FALSE,
    CONSTRAINT PK_Material PRIMARY KEY (MaterialId)
);

------------------- Tạo bảng Material_Supplier -----------------------
CREATE TABLE MaterialSupplier (
    MaterialSupplierId SERIAL PRIMARY KEY,
    MaterialId INT NOT NULL,
    SupplierId INT NOT NULL,
    SupplyDate DATE,
    Price DECIMAL(10, 2), -- Giá nhập cho vật tư từ nhà cung cấp
	
    FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
    FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);


------------------- Tạo bảng ConsumedMaterials -----------------------
CREATE TABLE ConsumedMaterials (
    ConsumedMaterialId SERIAL,
    MaterialId INT,
    Quantity DECIMAL(10, 2) DEFAULT 0, -- Số lượng nguyên liệu đã tiêu thụ
    Unit VARCHAR(50),
    IsDeleted BOOLEAN DEFAULT FALSE,

    CONSTRAINT PK_ConsumedMaterials PRIMARY KEY (ConsumedMaterialId),
    CONSTRAINT FK_ConsumedMaterials_Material FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId)
);


------------------- Tạo bảng Imports -----------------------
CREATE TABLE Imports (
    ImportId SERIAL,
    ImportDate DATE,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Imports PRIMARY KEY (ImportId)
);

------------------- Tạo bảng ImportDetails -----------------------
CREATE TABLE ImportDetails (
    ImportDetailId SERIAL,
    ImportId INT NOT NULL,
    MaterialId INT NOT NULL,
	SupplierId INT NOT NULL,
    Quantity INT DEFAULT 0,
    UnitPrice DECIMAL(10, 2) DEFAULT 0,
    TotalPrice DECIMAL(10, 2) DEFAULT 0,
	
    CONSTRAINT PK_ImportDetails PRIMARY KEY (ImportDetailId),
    CONSTRAINT FK_ImportDetails_Imports FOREIGN KEY (ImportId) REFERENCES Imports(ImportId),
    CONSTRAINT FK_ImportDetails_Material FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
	CONSTRAINT FK_ImportDetails_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);













