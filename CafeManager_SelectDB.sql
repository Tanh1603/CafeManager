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
SELECT * FROM supplierimportdetails;

SELECT * FROM imports IP JOIN supplier sp ON IP.supplierid = sp.supplierid;

SELECT SP.displayname, M.displayname ,IPD.quantity, IPD.unitprice
FROM imports IP JOIN importdetails IPD ON IP.importid = IPD.importid
JOIN supplierimportdetails SPIP ON IP.supplierid = SPIP.supplierid
JOIN supplier SP ON SP.supplierid = SPIP.supplierid
JOIN materialsupplier MSP ON IPD.materialid = MSP.materialid
JOIN material M ON M.materialid = MSP.materialid

















