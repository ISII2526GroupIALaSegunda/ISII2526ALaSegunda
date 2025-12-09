INSERT INTO [dbo].[AspNetUsers] 
([Id], [Name], [Surname], [Address], [AccountCreationDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
(N'1', N'User1', N'Userson', N'Elm Street', '2003-12-25 00:00:00', N'TheUser1', N'User1', N'User1@gmail.com', N'User1gmail', 1, N'Password1', N'Security1', N'Concurrency1', N'111111111', 1, 1, '2025-10-14 00:00:00', 1, 2),

(N'2', N'User2', N'Usersen', N'Eln Street', '2003-12-26 00:00:00', N'TheUser2', N'User2', N'User2@gmail.com', N'User2gmail', 0, N'Password2', N'Security2', N'Concurrency2', N'222222222', 0, 0, '2025-10-15 00:00:00', 0, 3);

SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (2, N'1', N'1', N'111111111', N'000000000', N'2026-10-31 00:00:00', N'User1@gmail.com')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF

SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (4, N'Ab', N'Av españa', N'02002', N'Alejandro Gomez', NULL, N'2025-10-13 00:00:00', 5, CAST(5.00 AS Decimal(10, 2)), 1, N'1', 1)
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (1, N'City1', N'Street1', N'Postal1', N'Name1Surname1', N'Description1', N'2025-11-29 00:00:00', 3, CAST(200.00 AS Decimal(10, 2)), 1, N'1', 2)
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (3, N'City2', N'Street2', N'Postal2', N'Name2Surname2', N'Descirption2', N'2025-11-30 00:00:00', 4, CAST(250.00 AS Decimal(10, 2)), 2, N'2', 2)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF

SET IDENTITY_INSERT [dbo].[Brands] ON
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (1, N'Location1', N'Brand1')
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (2, N'Location2', N'Brand2')
SET IDENTITY_INSERT [dbo].[Brands] OFF

SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (2, N'ProductCok', N'Second Product', N'Red', CAST(50.00 AS Decimal(10, 2)), 20, 1, 2)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (3, N'Product3', N'Third Product', N'Blue', CAST(75.00 AS Decimal(10, 2)), 15, 1, 1)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (4, N'Product4', N'Fourth Product', N'Yellow', CAST(100.00 AS Decimal(10, 2)), 10, 1, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF

SET IDENTITY_INSERT [dbo].[Returns] ON
INSERT INTO [dbo].[Returns] ([Id], [Name], [Date], [MoneyToReturn], [NewTotalPrice], [Rating], [TotalPrice], [PaymentMethodId], [CustomerId]) VALUES (6, N'Return1', N'2025-10-15 00:00:00', 50, CAST(100.00 AS Decimal(5, 2)), 3, CAST(200.00 AS Decimal(5, 2)), 2, N'1')
INSERT INTO [dbo].[Returns] ([Id], [Name], [Date], [MoneyToReturn], [NewTotalPrice], [Rating], [TotalPrice], [PaymentMethodId], [CustomerId]) VALUES (7, N'Return2', N'2025-10-16 00:00:00', 100, CAST(150.00 AS Decimal(5, 2)), 4, CAST(250.00 AS Decimal(5, 2)), 2, N'2')
SET IDENTITY_INSERT [dbo].[Returns] OFF

INSERT INTO [dbo].[PurchaseProducts] ([PurchaseOrderId], [ProductId], [Price], [Quantity]) VALUES (1, 2, CAST(200.00 AS Decimal(10, 2)), 10)
INSERT INTO [dbo].[PurchaseProducts] ([PurchaseOrderId], [ProductId], [Price], [Quantity]) VALUES (3, 3, CAST(250.00 AS Decimal(10, 2)), 11)
INSERT INTO [dbo].[PurchaseProducts] ([PurchaseOrderId], [ProductId], [Price], [Quantity]) VALUES (1, 4, CAST(300.00 AS Decimal(10, 2)), 12)

SET IDENTITY_INSERT [dbo].[ReturnProducts] ON
INSERT INTO [dbo].[ReturnProducts] ([Id], [Quantity], [Reason], [ReturnPurchaseOrderId], [ProductId], [PurchaseOrderId]) VALUES (25, 20, N'First Reason', 6, 1, 2)
INSERT INTO [dbo].[ReturnProducts] ([Id], [Quantity], [Reason], [ReturnPurchaseOrderId], [ProductId], [PurchaseOrderId]) VALUES (45, 25, N'Second Reason ', 6, 3, 3)
SET IDENTITY_INSERT [dbo].[ReturnProducts] OFF
