SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (1, N'City1', N'Street1', N'Postal1', N'Name1Surname1', N'Description1', N'2025-11-29 00:00:00', 3, CAST(200.00 AS Decimal(10, 2)), 1, N'1', 2)
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (3, N'City2', N'Street2', N'Postal2', N'Name2Surname2', N'Descirption2', N'2025-11-30 00:00:00', 4, CAST(250.00 AS Decimal(10, 2)), 2, N'2', 2)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
