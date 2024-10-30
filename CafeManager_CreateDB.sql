------------------- Tạo bảng user -----------------------
CREATE TABLE AppUser (
    AppUserId SERIAL,
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    Role INT DEFAULT 0, -- 1 = Admin, 0 = Staff
    DisplayName VARCHAR(100) DEFAULT 'Unkown',
    Avatar TEXT,
    IsDeleted BOOLEAN DEFAULT FALSE,

    CONSTRAINT PK_AppUserId PRIMARY KEY (AppUserId)
);
------------------- Tạo bảng Staff -----------------------
CREATE TABLE Staff (
    StaffId SERIAL,
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
	
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Staff PRIMARY KEY (StaffId)
);
------------------- Tạo bảng FoodCategory -----------------------
CREATE TABLE FoodCategory (
    FoodCategoryId SERIAL,
    FoodCategoryName VARCHAR(100) NOT NULL,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_FoodCategory PRIMARY KEY (FoodCategoryId)
);
------------------- Tạo bảng Food -----------------------
CREATE TABLE Food (
    FoodId SERIAL,
    FoodName VARCHAR(100) NOT NULL,
    FoodCategoryId INT NOT NULL,
    Price DECIMAL(10, 2) DEFAULT 0,
    ImageFood TEXT,
    DiscountFood DECIMAL(5, 2) DEFAULT 0,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Food PRIMARY KEY (FoodId),
    CONSTRAINT FK_Food_FoodCategory FOREIGN KEY (FoodCategoryId) REFERENCES FoodCategory(FoodCategoryId)
);
------------------- Tạo bảng Table -----------------------
CREATE TABLE CoffeeTable (
    CoffeeTableId SERIAL,
    StatusTable VARCHAR(50) DEFAULT 'Trống', -- "Available", "Occupied"
	NOTES TEXT,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_CoffeeTable PRIMARY KEY (CoffeeTableId)
);
------------------- Tạo bảng Invoices -----------------------
CREATE TABLE Invoices (
    InvoiceId SERIAL,
    PaymentStartDate TIMESTAMP NOT NULL,
	PaymentEndDate TIMESTAMP,
    PaymentStatus VARCHAR(50) DEFAULT 'Chưa thanh toán', 
	PaymentMethod VARCHAR(50) DEFAULT 'Thanh toán tiền mặt',
    DiscountInvoice DECIMAL(5, 2) DEFAULT 0,
    CoffeeTableId INT,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Invoices PRIMARY KEY (InvoiceId),
    CONSTRAINT FK_Invoices_CoffeeTable FOREIGN KEY (CoffeeTableId) REFERENCES CoffeeTable(CoffeeTableId)
);
------------------- Tạo bảng InvoiceDetails -----------------------
CREATE TABLE InvoiceDetails (
    InvoiceDetailId SERIAL,
    InvoiceId INT,
    FoodId INT NOT NULL,
    Quantity INT DEFAULT 0,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_InvoiceDetails PRIMARY KEY (InvoiceDetailId),
    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId),
    CONSTRAINT FK_InvoiceDetails_Food FOREIGN KEY (FoodId) REFERENCES Food(FoodId)
);

------------------- Tạo bảng Supplier -----------------------
CREATE TABLE Supplier (
    SupplierId SERIAL,
	SupplierName VARCHAR(100) NOT NULL,
	RepresentativeSupplier VARCHAR NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    Address VARCHAR(255),
	Notes TEXT,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Supplier PRIMARY KEY (SupplierId)
);
------------------- Tạo bảng Material -----------------------
CREATE TABLE Material (
    MaterialId SERIAL,
    MaterialName VARCHAR(100) NOT NULL,
    Unit VARCHAR(50),
    ExpiryDate DATE,
    Price DECIMAL(10, 2) DEFAULT 0,
    IsDeleted BOOLEAN DEFAULT FALSE,
    CONSTRAINT PK_Material PRIMARY KEY (MaterialId)
);
------------------- Tạo bảng Material_Supplier -----------------------
CREATE TABLE MaterialSupplier (
    MaterialSupplierId SERIAL,
    MaterialId INT NOT NULL,
    SupplierId INT NOT NULL,

	CONSTRAINT PK_MaterialSupplier PRIMARY KEY (SupplierId),
    CONSTRAINT Fk_Material_Supplier FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
    CONSTRAINT Fk_Supplier_Material FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
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
	DeliveryRepresentative VARCHAR(50) NOT NULL, 
	Phone VARCHAR(20) NOT NULL,
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
	
    CONSTRAINT PK_ImportDetails PRIMARY KEY (ImportDetailId),
    CONSTRAINT FK_ImportDetails_Imports FOREIGN KEY (ImportId) REFERENCES Imports(ImportId),
    CONSTRAINT FK_ImportDetails_Material FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
	CONSTRAINT FK_ImportDetails_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);



