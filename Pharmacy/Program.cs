
using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;


CategoryService categoryService = new CategoryService();
SupplierService supplierService = new SupplierService();
MedicineService medicineService = new MedicineService();
BatchService batchService =new BatchService();
/*
var category = new Category(
    1,
    "Painkillers"
);*/
categoryService.Add("Painkillers");
Category? category = categoryService.SearchByName("Painkillers");


supplierService.Add("Al-Shifa Supplier","777123456");


medicineService.Add( "Panadol",
    "GSK",
    "12345678",
    "Paracetamol",
    "500mg",
    1.50m,
    2.00m,
    10,
    DosageForm.Tablet,
    category);

var purchaseService = new PurchaseService(supplierService,medicineService,batchService);



var invoice = purchaseService.CreatePurchase(
    "PUR-001",
    PaymentMethod.Card,
    1,
    1,
    "A125",
    20,
    DateTime.Today.AddMonths(-2),
    DateTime.Today.AddMonths(10),
    1.50m
);


    var p=  purchaseService.GetById(1);
    Console.WriteLine($"dffddd {p.Id}");
Console.WriteLine("=== Purchase Test ===");

Console.WriteLine($"Invoice ID: {invoice.Id}");
Console.WriteLine($"Invoice Number: {invoice.InvoiceNumber}");
Console.WriteLine($"Supplier: {invoice.Supplier.Name}");
Console.WriteLine($"Total: {invoice.TotalPrice}");

var item = invoice.PurchaseInvoiceItems[0];

Console.WriteLine();
Console.WriteLine($"Medicine: {item.Medicine.Name}");
Console.WriteLine($"Quantity: {item.Quantity}");
Console.WriteLine($"Unit Price: {item.UnitPrice}");
Console.WriteLine($"Item Total: {item.TotalPrice}");

var invoiceBatch = item.InvoiceBatches[0];

Console.WriteLine();
Console.WriteLine($"Batch Number: {invoiceBatch.Batch.BatchNumber}");
Console.WriteLine($"Batch Quantity: {invoiceBatch.Batch.Quantity}");
Console.WriteLine($"Invoice Batch Quantity: {invoiceBatch.Quantity}");

Console.WriteLine();
Console.WriteLine(
    $"Purchase Invoices Count: {purchaseService.GetAll().Count}");
