-- Insert 50 Suppliers
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO Supplier (SupplierName, RepresentativeSupplier, Phone, Email, Address, Notes, IsDeleted)
        VALUES (
            CONCAT('Supplier ', i),
            CONCAT('Representative ', i),
            CONCAT('09', i, '456789'),
            CONCAT('supplier', i, '@example.com'),
            CONCAT(i, ' Street, City'),
            'Notes for supplier ' || i,
            FALSE
        );
    END LOOP;
END $$;

-- Insert 50 Materials
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO Material (MaterialName, Unit, IsDeleted)
        VALUES (
            CONCAT('Material ', i),
            CASE WHEN i % 2 = 0 THEN 'kg' ELSE 'pack' END,
            FALSE
        );
    END LOOP;
END $$;

-- Insert 50 MaterialSuppliers
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO MaterialSupplier (MaterialId, SupplierId, ManufactureDate, ExpirationDate, Original, Manufacturer, Price, IsDeleted)
        VALUES (
            i, -- Valid MaterialId
            i, -- Valid SupplierId
            NOW() - INTERVAL '1 year',
            NOW() + INTERVAL '1 year',
            'Country ' || i,
            CONCAT('Manufacturer ', i),
            (i * 10.00),
            FALSE
        );
    END LOOP;
END $$;

-- Insert 50 ConsumedMaterials
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO ConsumedMaterials (MaterialSupplierId, Quantity, UsageDate, IsDeleted)
        VALUES (
            i, -- Match MaterialSupplierId
            (i * 5.00),
            NOW() - INTERVAL '1 day' * i,
            FALSE
        );
    END LOOP;
END $$;

-- Insert 50 Imports
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO Imports (DeliveryPerson, Phone, ShippingCompany, ReceivedDate, StaffId, SupplierId, IsDeleted)
        VALUES (
            CONCAT('Delivery ', i),
            CONCAT('09', i, '123456'),
            CONCAT('Shipping Company ', i),
            NOW() - INTERVAL '1 day' * i,
            1,
            i, -- Match SupplierId
            FALSE
        );
    END LOOP;
END $$;

-- Insert 50 ImportDetails
DO $$
BEGIN
    FOR i IN 1..50 LOOP
        INSERT INTO ImportDetails (ImportId, MaterialSupplierId, Quantity, IsDeleted)
        VALUES (
            i, -- Match ImportId
            i, -- Match MaterialSupplierId
            (i * 10.00),
            FALSE
        );
    END LOOP;
END $$;
