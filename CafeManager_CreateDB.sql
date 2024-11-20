------------------- Tạo bảng user -----------------------
CREATE TABLE AppUser (
    AppUserId SERIAL,
    UserName VARCHAR(50) NOT NULL,
    DisplayName VARCHAR(100) DEFAULT 'Unkown',
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    Role INT DEFAULT 0, -- 1 = Admin, 0 = Staff
    Avatar TEXT,
    IsDeleted BOOLEAN DEFAULT FALSE,

    CONSTRAINT PK_AppUserId PRIMARY KEY (AppUserId)
);
------------------- Tạo bảng Staff -----------------------
CREATE TABLE Staff (
    StaffId SERIAL,
    StaffName VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Sex BOOLEAN,
    Birthday DATE NOT NULL,
	Address VARCHAR(50) NOT NULL,
    StartWorkingDate DATE NOT NULL,
	EndWorkingDate DATE,
    Role VARCHAR(50),
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Staff PRIMARY KEY (StaffId),
	CONSTRAINT CK_Staff_WorkingDate CHECK (EndWorkingDate IS NULL OR StartWorkingDate <= EndWorkingDate)
);
CREATE TABLE StaffSalaryHistory (
	StaffSalaryHistoryId SERIAL,
	StaffId INT NOT NULL,
	Salary DECIMAL(10,2) DEFAULT 0 NOT NULL ,
	EffectiveDate DATE NOT NULL,
	IsDeleted BOOLEAN DEFAULT FALSE,

	CONSTRAINT PK_StaffSalaryHistory PRIMARY KEY (StaffSalaryHistoryId),
	CONSTRAINT FK_StaffSalaryHistory_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId)
);
----------------- Tạo bảng FoodCategory -----------------------
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
	TableNumber INT UNIQUE NOT NULL,
	SeatingCapacity INT DEFAULT 4,
	StatusTable VARCHAR(50) DEFAULT 'Đang sử dụng', -- "Hư", "Xóa"
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
	StaffId INT NOT NULL,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Invoices PRIMARY KEY (InvoiceId),
	CONSTRAINT FK_Invoices_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId),
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
    MaterialName VARCHAR(100) UNIQUE NOT NULL,
    Unit VARCHAR(50) NOT NULL,
	
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Material PRIMARY KEY (MaterialId)
);
------------------- Tạo bảng Material_Supplier -----------------------
CREATE TABLE MaterialSupplier (
    MaterialSupplierId SERIAL,
    MaterialId INT NOT NULL,
    SupplierId INT NOT NULL,
	ManufactureDate TIMESTAMP NOT NULL,
	ExpirationDate TIMESTAMP NOT NULL,
	Original VARCHAR(20) NOT NULL,
	Manufacturer VARCHAR(50) NOT NULL,
	Price DECIMAL(10, 2) DEFAULT 0,
	IsDeleted BOOLEAN DEFAULT FALSE,
	
	CONSTRAINT PK_MaterialSupplier PRIMARY KEY (MaterialSupplierId),
    CONSTRAINT Fk_Material_Supplier FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId),
    CONSTRAINT Fk_Supplier_Material FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);
------------------- Tạo bảng ConsumedMaterials -----------------------
CREATE TABLE ConsumedMaterials (
    ConsumedMaterialId SERIAL,
    MaterialSupplierId INT,
    Quantity DECIMAL(10, 2) DEFAULT 0,
    IsDeleted BOOLEAN DEFAULT FALSE,

    CONSTRAINT PK_ConsumedMaterials PRIMARY KEY (ConsumedMaterialId),
    CONSTRAINT PK_ConsumedMaterials_MaterialSupplierId FOREIGN KEY (MaterialSupplierId) REFERENCES MaterialSupplier(MaterialSupplierId)
);
------------------- Tạo bảng Imports -----------------------
CREATE TABLE Imports (
    ImportId SERIAL,
	DeliveryPerson VARCHAR(50) NOT NULL, ---
	Phone VARCHAR(20) NOT NULL,
	ShippingCompany VARCHAR(100),          
    ReceivedDate TIMESTAMP NOT NULL,          
    StaffId INT NOT NULL, 
	SupplierId INT NOT NULL,
    IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_Imports PRIMARY KEY (ImportId),
    CONSTRAINT FK_Imports_Staff FOREIGN KEY (StaffId) REFERENCES Staff(StaffId),
    CONSTRAINT FK_Imports_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId)
);
------------------- Tạo bảng ImportDetails -----------------------
CREATE TABLE ImportDetails (
    ImportDetailId SERIAL,
    ImportId INT NOT NULL,
	MaterialId INT NOT NULL,
	Quantity DECIMAL(10, 2) DEFAULT 0,
	IsDeleted BOOLEAN DEFAULT FALSE,
	
    CONSTRAINT PK_ImportDetails PRIMARY KEY (ImportDetailId),
    CONSTRAINT FK_ImportDetails_Imports FOREIGN KEY (ImportId) REFERENCES Imports(ImportId),
    CONSTRAINT FK_ImportDetails_Material FOREIGN KEY (MaterialId) REFERENCES Material(MaterialId)
);

