SET IDENTITY_INSERT [dbo].[Returns] ON
INSERT INTO [dbo].[Returns] ([Id], [Name], [Date], [MoneyToReturn], [NewTotalPrice], [Rating], [TotalPrice], [PaymentMethodId], [CustomerId]) VALUES (6, N'Return1', N'2025-10-15 00:00:00', 50, CAST(100.00 AS Decimal(5, 2)), 3, CAST(200.00 AS Decimal(5, 2)), 2, N'1')
INSERT INTO [dbo].[Returns] ([Id], [Name], [Date], [MoneyToReturn], [NewTotalPrice], [Rating], [TotalPrice], [PaymentMethodId], [CustomerId]) VALUES (7, N'Return2', N'2025-10-16 00:00:00', 100, CAST(150.00 AS Decimal(5, 2)), 4, CAST(250.00 AS Decimal(5, 2)), 2, N'2')
SET IDENTITY_INSERT [dbo].[Returns] OFF
