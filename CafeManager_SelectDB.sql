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

INSERT INTO customer (displayname, buydate, "type", totalspent) VALUES
('Tanh 1', NOW(), 'Thường xuyên',0),
('Tanh 2', NOW(), 'Vip',0),
('Tanh 3', NOW(), 'Khách vãng lai',0),
('Tanh 4', NOW(), 'Thường xuyên',0),
('Tanh 5', NOW(), 'Vip',0);

INSERT INTO coffeetable (statustable) VALUES ('Trống'), ('Đã đặt'), ('Trống');

INSERT INTO invoices (paymentdate, payementendate, paymentstatus, discountinvoice, coffeetableid, customerid) VALUES
(Now(), Now(), 'Đã thanh toán', 0, 6, NULL),
(Now(), Now(), 'Chưa thanh toán', 0, 6, NULL),
(Now(), Now(), 'Chưa thanh toán', 0, NULL, 6),
(Now(), Now(), 'Bị hủy', 0, NULL, 6),
(Now(), Now(), 'Đã thanh toán', 0, NULL, 8),
(Now(), Now(), 'Bị hủy', 0, NULL, 9),
(Now(), Now(), 'Đã thanh toán', 0, 8, NULL);

INSERT INTO invoicedetails (invoiceid, foodid, quantity) VALUES
(6, 6, 2),
(6, 7, 5),
(6, 8, 3),
(7, 11, 5),
(7, 12, 1),
(8, 9, 2),
(8, 10, 2),
(8, 11, 2);


SELECT i.paymentdate, i.payementendate, i.paymentstatus, i.discountinvoice, c.statustable, f.displayname, ivd.quantity
FROM invoices i JOIN coffeetable c ON i.coffeetableid = c.coffeetableid
JOIN invoicedetails ivd ON ivd.invoiceid = i.invoiceid
JOIN food f ON f.foodid= ivd.foodid;


SELECT c.displayname, c."type", C.buydate,
ROUND( SUM(
	COALESCE(IVD.QUANTITY, 0) * COALESCE(F.PRICE, 0) * (1 - COALESCE(F.DISCOUNTFOOD / 100, 0)) * (1 - COALESCE(IV.DISCOUNTINVOICE / 100, 0))
) , 2) AS "Total spent"
FROM
	CUSTOMER C
	JOIN INVOICES IV ON C.CUstomerid = iv.customerid
LEFT JOIN invoicedetails ivd ON ivd.invoiceid = iv.invoiceid
LEFT JOIN food f On f.foodid = ivd.foodid
GROUP BY c.customerid;

SELECT * from invoices i JOIN invoicedetails ivd ON i.invoiceid = ivd.invoiceid;
SELECT * from imports i JOIN importdetails ON importdetails.importid = i.importid ;



















