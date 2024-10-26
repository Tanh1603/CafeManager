-- Dữ liệu bảng User
INSERT INTO AppUser (UserName, Password, Email, Role, DisplayName, Avatar, IsDeleted) VALUES
('admin', 'hashed_password_1', 'admin@example.com', 1, 'Administrator', 'avatar_admin.png', FALSE),
('staff1', 'hashed_password_2', 'staff1@example.com', 0, 'Staff Member 1', 'avatar_staff1.png', FALSE),
('staff2', 'hashed_password_3', 'staff2@example.com', 0, 'Staff Member 2', 'avatar_staff2.png', FALSE),
('staff3', 'hashed_password_4', 'staff3@example.com', 0, 'Staff Member 3', 'avatar_staff3.png', FALSE),
('staff4', 'hashed_password_5', 'staff4@example.com', 0, 'Staff Member 4', 'avatar_staff4.png', FALSE);

-- Dữ liệu bảng Staff
INSERT INTO Staff (DisplayName, Phone, Email, Sex, Birthday, StartWorking, Role, WorkingStatus, BasicSalary, WorkingHours, IsDeleted) VALUES
('John Doe', '0123456789', 'johndoe@example.com', 'M', '1990-01-01', '2020-01-01', 'Manager', 'Active', 5000.00, 40, FALSE),
('Jane Smith', '0987654321', 'janesmith@example.com', 'F', '1995-05-05', '2021-01-01', 'Chef', 'Active', 4000.00, 35, FALSE),
('Alice Johnson', '0112233445', 'alice@example.com', 'F', '1992-03-03', '2019-01-01', 'Waitress', 'Active', 3000.00, 30, FALSE),
('Bob Brown', '0223344556', 'bob@example.com', 'M', '1988-07-07', '2022-01-01', 'Bartender', 'Active', 3500.00, 40, FALSE),
('Charlie White', '0334455667', 'charlie@example.com', 'M', '1985-12-12', '2023-01-01', 'Cashier', 'Active', 3200.00, 30, FALSE);

-- Dữ liệu bảng FoodCategory
INSERT INTO FoodCategory (DisplayName, IsDeleted) VALUES
('Appetizers', FALSE),
('Main Course', FALSE),
('Desserts', FALSE),
('Beverages', FALSE),
('Salads', FALSE);

-- Dữ liệu bảng Food
INSERT INTO Food (DisplayName, FoodCategoryId, Price, ImageFood, DiscountFood, IsDeleted) VALUES
('Spring Rolls', 1, 5.00, NULL, 0.00, FALSE),
('Grilled Chicken', 2, 10.00, NULL, 1.00, FALSE),
('Chocolate Cake', 3, 4.00, NULL, 0.50, FALSE),
('Orange Juice', 4, 3.00, NULL, 0.00, FALSE),
('Caesar Salad', 5, 6.00, NULL, 0.00, FALSE);

-- Dữ liệu bảng Customer
INSERT INTO Customer (DisplayName, BuyDate, Type, TotalSpent, IsDeleted) VALUES
('Customer 1', CURRENT_TIMESTAMP, 'Regular', 100.00, FALSE),
('Customer 2', CURRENT_TIMESTAMP, 'VIP', 250.00, FALSE),
('Customer 3', CURRENT_TIMESTAMP, 'Walk-in', 20.00, FALSE),
('Customer 4', CURRENT_TIMESTAMP, 'Regular', 150.00, FALSE),
('Customer 5', CURRENT_TIMESTAMP, 'VIP', 300.00, FALSE);

 -- Dữ Liệu Bảng Invoices
INSERT INTO Invoices (PaymentDate, PaymentStatus, DiscountInvoice, CoffeeTableId, CustomerId, IsDeleted) VALUES
(CURRENT_TIMESTAMP, 'Paid', 5.00, 1, 1, FALSE),
(CURRENT_TIMESTAMP, 'Pending', 0.00, 2, 2, FALSE),
(CURRENT_TIMESTAMP, 'Paid', 2.00, 3, 3, FALSE),
(CURRENT_TIMESTAMP, 'Paid', 0.00, 4, 4, FALSE),
(CURRENT_TIMESTAMP, 'Pending', 0.00, 1, 5, FALSE);

-- Dữ Liệu Bảng InvoiceDetails
INSERT INTO InvoiceDetails (InvoiceId, FoodId, Quantity, IsDeleted) VALUES
(1, 1, 2, FALSE),
(1, 2, 1, FALSE),
(2, 2, 3, FALSE),
(3, 3, 1, FALSE),
(4, 4, 4, FALSE);

-- Dữ Liệu Bảng CoffeeTable
INSERT INTO CoffeeTable (StatusTable, IsDeleted) VALUES
('Available', FALSE),
('Occupied', FALSE),
('Available', FALSE),
('Occupied', FALSE),
('Available', FALSE);

-- Dữ Liệu Bảng Supplier
INSERT INTO Supplier (DisplayName, Phone, Email, Address, ContractDate, IsDeleted) VALUES
('Supplier A', '1234567890', 'supplierA@example.com', '123 Supplier St.', '2022-01-01', FALSE),
('Supplier B', '0987654321', 'supplierB@example.com', '456 Supplier St.', '2022-02-01', FALSE),
('Supplier C', '1122334455', 'supplierC@example.com', '789 Supplier St.', '2022-03-01', FALSE),
('Supplier D', '2233445566', 'supplierD@example.com', '101 Supplier St.', '2022-04-01', FALSE),
('Supplier E', '3344556677', 'supplierE@example.com', '202 Supplier St.', '2022-05-01', FALSE);

-- Dữ Liệu Bảng Material
INSERT INTO Material (DisplayName, Unit, ExpiryDate, Price, IsDeleted) VALUES
('Tomatoes', 'kg', '2024-12-31', 2.00, FALSE),
('Chicken Breast', 'kg', '2024-11-30', 8.00, FALSE),
('Flour', 'kg', '2025-06-30', 1.50, FALSE),
('Sugar', 'kg', '2025-05-30', 1.20, FALSE),
('Lettuce', 'kg', '2024-10-31', 1.00, FALSE);

-- Dữ Liệu Bảng MaterialSupplier
INSERT INTO MaterialSupplier (MaterialId, SupplierId, SupplyDate, Price) VALUES
(1, 1, '2022-01-10', 1.80),
(2, 2, '2022-02-10', 7.50),
(3, 3, '2022-03-10', 1.30),
(4, 4, '2022-04-10', 1.00),
(5, 5, '2022-05-10', 0.90);

-- Dữ Liệu Bảng ConsumedMaterials
INSERT INTO ConsumedMaterials (ConsumedMaterialId, MaterialId, Quantity, Unit, IsDeleted) VALUES
(1, 1, 5.00, 'kg', FALSE),  
(2, 2, 3.50, 'kg', FALSE),   
(3, 3, 10.00, 'kg', FALSE),  
(4, 4, 2.00, 'kg', FALSE),   
(5, 5, 4.00, 'kg', FALSE);   

-- Dữ Liệu Bảng Imports
INSERT INTO Imports (SupplierId, ImportDate, IsDeleted) VALUES
(1, '2024-10-01', FALSE),
(2, '2024-10-02', FALSE),
(3, '2024-10-03', FALSE),
(4, '2024-10-04', FALSE),
(5, '2024-10-05', FALSE);

-- Dữ Liệu Bảng ImportDetails
INSERT INTO ImportDetails (ImportId, MaterialId, Quantity, UnitPrice, TotalPrice) VALUES
(1, 1, 100, 1.80, 180.00),
(1, 2, 50, 7.50, 375.00),
(2, 3, 200, 1.30, 260.00),
(3, 4, 150, 1.00, 150.00),
(4, 5, 250, 0.90, 225.00);

-- Dữ Liệu Bảng SupplierImportDetails
INSERT INTO SupplierImportDetails (SupplierId, ImportDetailId, IsDeleted) VALUES
(1, 1, FALSE),
(2, 2, FALSE),
(3, 3, FALSE),
(4, 4, FALSE),
(5, 5, FALSE);








