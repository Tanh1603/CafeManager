INSERT INTO Supplier (SupplierName, RepresentativeSupplier, Phone, Email, Address, Notes, IsDeleted)
VALUES 
('Công ty TNHH Nguyên Liệu Việt', 'Nguyễn Văn A', '0123456789', 'nguyenlieuviet@gmail.com', '123 Đường ABC, Quận 1, TP.HCM', 'Chuyên cung cấp nguyên liệu thực phẩm', FALSE),
('Công ty CP Vật Tư Miền Nam', 'Trần Thị B', '0987654321', 'vatmumiennam@gmail.com', '456 Đường XYZ, Quận 3, TP.HCM', 'Chuyên cung cấp vật tư xây dựng', FALSE),
('Công ty TNHH Phát Triển Đông Á', 'Phạm Văn C', '0901234567', 'dongadevelopment@gmail.com', '789 Đường DEF, Quận 5, TP.HCM', 'Nhà phân phối đa ngành', FALSE);

INSERT INTO Material (MaterialName, Unit, IsDeleted)
VALUES 
('Xi măng', 'Bao 50kg', FALSE),
('Thép xây dựng', 'Tấn', FALSE),
('Gạch men', 'Hộp', FALSE),
('Cát xây dựng', 'Khối', FALSE),
('Đá xây dựng', 'Khối', FALSE);

INSERT INTO MaterialSupplier (MaterialId, SupplierId, ManufactureDate, ExpirationDate, Original, Manufacturer, Price, IsDeleted)
VALUES 
(1, 1, '2024-01-01 00:00:00', '2026-01-01 00:00:00', 'Việt Nam', 'Công ty Xi măng Hà Tiên', 85000, FALSE),
(2, 2, '2023-06-15 00:00:00', '2025-06-15 00:00:00', 'Việt Nam', 'Công ty Thép Việt Nhật', 13000000, FALSE),
(3, 3, '2024-05-01 00:00:00', '2027-05-01 00:00:00', 'Trung Quốc', 'Công ty Gạch Hoa Đông', 300000, FALSE),
(4, 1, '2024-03-20 00:00:00', '2025-03-20 00:00:00', 'Việt Nam', 'Công ty Cát Bình Thuận', 120000, FALSE),
(5, 2, '2024-07-01 00:00:00', '2025-07-01 00:00:00', 'Việt Nam', 'Công ty Đá Đồng Nai', 150000, FALSE);

INSERT INTO ConsumedMaterials (MaterialSupplierId, Quantity, IsDeleted)
VALUES 
(1, 500, FALSE),
(2, 20, FALSE),
(3, 200, FALSE),
(4, 1000, FALSE),
(5, 800, FALSE);

INSERT INTO Imports (DeliveryPerson, Phone, ShippingCompany, ReceivedDate, StaffId, SupplierId, IsDeleted)
VALUES 
('Nguyễn Văn D', '0934567890', 'Công ty Vận tải Thành Công', '2024-11-15 10:00:00', 1, 1, FALSE),
('Trần Thị E', '0912345678', 'Công ty Vận tải Hòa Bình', '2024-11-18 14:30:00', 2, 2, FALSE),
('Phạm Văn F', '0923456789', 'Công ty Vận tải Bắc Nam', '2024-11-20 09:15:00', 3, 3, FALSE);

INSERT INTO ImportDetails (ImportId, MaterialId, Quantity, IsDeleted)
VALUES 
(1, 1, 300, FALSE),
(1, 4, 500, FALSE),
(2, 2, 15, FALSE),
(2, 5, 200, FALSE),
(3, 3, 100, FALSE);
