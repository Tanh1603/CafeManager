SELECT * FROM appuser;
SELECT * FROM staff;

SELECT * FROM foodcategory;
SELECT foodid, foodname, isdeleted, discountfood FROM food;
ALTER SEQUENCE food_foodid_seq RESTART WITH 1;
DELETE FROM food;

SELECT * FROM invoices;
DELETE FROM invoices;
SELECT * FROM invoicedetails;
DELETE FROM invoicedetails;
SELECT * FROM coffeetable;

SELECT * FROM imports;
ALTER SEQUENCE imports_ImportId_seq RESTART WITH 1;
DELETE FROM imports;

SELECT * FROM supplier;
ALTER SEQUENCE supplier_supplierId_seq RESTART WITH 1;
DELETE FROM supplier;

SELECT * FROM material;
ALTER SEQUENCE material_materialId_seq RESTART WITH 1;
DELETE FROM material;

SELECT * FROM importdetails;
ALTER SEQUENCE importdetails_ImportDetailId_seq RESTART WITH 1;
DELETE FROM importdetails;

SELECT * FROM materialsupplier;
ALTER SEQUENCE materialsupplier_materialsupplierId_seq RESTART WITH 1;
DELETE FROM materialsupplier;

SELECT * FROM consumedmaterials;
ALTER SEQUENCE ConsumedMaterials_ConsumedMaterialId_seq RESTART WITH 1;
DELETE FROM consumedmaterials;

-- Tính tiền 1 bill
SELECT ROUND(SUM(f.price * ivd.quantity * ((100 - f.discountfood)/100) * ((100 - iv.discountinvoice)/100)  ), 2) AS "Tổng tiền"
FROM invoices iv JOIN invoicedetails ivd ON ivd.invoiceid = iv.invoiceid
JOIN food f ON f.foodid = ivd.foodid

SELECT ivd.invoiceid, f.foodname
FROM invoicedetails ivd JOIN food f ON f.foodid = ivd.foodid

-- IMPORT ---
INSERT INTO material(materialname, unit) VALUES
('Muối', 'kg'),
('Cafe', 'kg'),
('Đường', 'kg'),
('Ly', 'Thùng');

INSERT INTO supplier(suppliername,representativesupplier,phone, email, address, notes) VALUES
('Công ty A', 'Tanh A', '1111', '@gmail.com','VT','Chưa có'),
('Công ty B', 'Tanh B', '2222', '@gmail.com','HCM','Chưa có'),
('Công ty C', 'Tanh C', '3333', '@gmail.com','DN','Chưa có');

INSERT INTO materialsupplier(
	materialid, supplierid,manufacturer,original,manufacturedate,expirationdate,price
) VALUES
(1,1, 'Nhà máy A', 'Trung Quốc', NOW(), NOW(), 10000),
(1,1, 'Nhà máy B', 'Hàn Quốc', NOW(), NOW(), 15000),
(1,2, 'Nhà máy A', 'Trung Quốc', NOW(), NOW(), 11000),
(2,3, 'Nhày máy cafe', 'Trung Quốc', NOW(), NOW(), 150000),
(2,2, 'Nhà máy cafe 2', 'Việt Nam', NOW(), NOW(), 200000),
(3,3, 'Nhà máy đương', 'Mĩ', NOW(), NOW(), 14000),
(4,3, 'Nhà máy ly', 'Việt Nam', NOW(), NOW(), 20000);

INSERT INTO imports(deliveryperson, phone, shippingcompany, receiveddate, receiver) VALUES
( 'Người giao hàng 1', '1234', 'Shoppe', NOW(), 'Trần Văn A'),
( 'Người giao hàng 2', '4321', 'Lazada', NOW(), 'Trần Văn B'),
( 'Người giao hàng 3', '5678', 'Tiki', NOW(), 'Trần Văn C');

INSERT INTO importdetails(importid,materialsupplierid, quantity) VALUES
(1, 1, 20),
(1, 3, 10),
(1, 5, 5),
(2, 1, 10),
(2, 2, 5),
(2, 3, 8),
(3, 4, 2),
(3, 6, 2),
(3, 7, 2);

INSERT INTO consumedmaterials(materialid,quantity) VALUES
(1,10),
(2, 10);

-- Material Detail
SELECT m.materialname, m.unit, ms.manufacturer, ms.original, 
ms.manufacturedate, ms.expirationdate, s.suppliername, ms.price,
SUM(imd.quantity) as "Tổng số lượng"
FROM material m LEFT JOIN MaterialSupplier ms ON ms.materialid = m.materialid
JOIN Supplier s ON s.supplierid = ms.supplierid
JOIN importdetails imd ON imd.materialsupplierid = ms.materialsupplierid
GROUP BY m.materialname, m.unit, ms.original, 
ms.manufacturedate, ms.expirationdate,ms.manufacturer, s.suppliername, ms.price;

-- Used material
SELECT m.materialname, m.unit, ms.manufacturer, ms.original, 
ms.manufacturedate, ms.expirationdate, s.suppliername, ms.price, Sum(cs.quantity)
FROM consumedmaterials cs JOIN material m ON m.materialid = cs.materialid
JOIN materialsupplier ms ON ms.materialid = m.materialid
JOIN supplier s ON s.supplierid = ms.supplierid
JOIN importdetails imd ON imd.materialsupplierid = ms.materialsupplierid
GROUP BY m.materialname, m.unit, ms.original, 
ms.manufacturedate, ms.expirationdate,ms.manufacturer, s.suppliername, ms.price;





