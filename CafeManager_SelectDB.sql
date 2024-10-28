SELECT * FROM appuser;
SELECT * FROM staff;

SELECT * FROM foodcategory;
SELECT * FROM food;
SELECT * FROM customer;
SELECT * FROM invoices;
SELECT * FROM invoicedetails;
SELECT * FROM coffeetable;

SELECT * FROM supplier;
SELECT * FROM material;
SELECT * FROM consumedmaterials;
SELECT * FROM materialsupplier;
SELECT * FROM imports;
SELECT * FROM importdetails;

SELECT * FROM imports IP JOIN supplier sp ON IP.supplierid = sp.supplierid;

SELECT SP.displayname, M.displayname ,IPD.quantity, IPD.unitprice
FROM imports IP JOIN importdetails IPD ON IP.importid = IPD.importid
JOIN supplierimportdetails SPIP ON IP.supplierid = SPIP.supplierid
JOIN supplier SP ON SP.supplierid = SPIP.supplierid
JOIN materialsupplier MSP ON IPD.materialid = MSP.materialid
JOIN material M ON M.materialid = MSP.materialid

SELECT fc.displayname as "Danh mục", f.displayname as "Tên món ăn", f.price as "Giá", f.discountfood as "Giảm giá"
FROM foodcategory fc INNER JOIN food f ON fc.foodcategoryid = f.foodcategoryid
WHERE fc.isdeleted = FALSE AND f.isdeleted = false;

INSERT INTO foodcategory (displayname)
VALUES ('Cafe'), ('Bánh ngọt'), ('Khác');

INSERT INTO food (displayname,price,imagefood,foodcategoryid) VALUES 
('Trà sữa truyền thống',35000,'Trống', 7),
('Trà sữa socola',35000,'Trống',7),
('Cafe sữa',25000,'Trống',8),
('Cafe đen',25000,'Trống',8),
('Bánh kem bơ',35000,'Trống',9),
('Bánh creep sầu riêng',45000,'Trống',9),
('Bánh bông lan trứng muối',25000,'Trống',9),
('Chưa biết 1', 1,'Trống',10),
('Chưa biết 2',1,'Trống',10);
















