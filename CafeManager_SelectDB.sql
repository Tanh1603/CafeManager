SELECT * FROM appuser;
ALTER SEQUENCE appuser_appuserid_seq RESTART WITH 1;
DELETE FROM appuser;

SELECT * FROM staff;
ALTER SEQUENCE staff_staffid_seq RESTART WITH 1;
DELETE FROM staff;

SELECT * FROM staffsalaryhistory;
ALTER SEQUENCE staffsalaryhistory_staffsalaryhistoryid_seq RESTART WITH 1;
DELETE FROM staffsalaryhistory;


SELECT * FROM foodcategory;
SELECT * FROM food;
SELECT foodid, foodname, isdeleted, discountfood FROM food;
ALTER SEQUENCE food_foodid_seq RESTART WITH 1;
DELETE FROM food;

SELECT * FROM invoices;
ALTER SEQUENCE invoices_invoiceid_seq RESTART WITH 1;
DELETE FROM invoices;

SELECT * FROM invoicedetails;
ALTER SEQUENCE invoicedetails_invoicedetailid_seq RESTART WITH 1;
DELETE FROM invoicedetails;

SELECT * FROM coffeetable;
ALTER SEQUENCE coffeetable_coffeetableId_seq RESTART WITH 1;
DELETE FROM coffeetable;

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





