SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (2, N'Product2', N'Second Product', N'Red', CAST(50.00 AS Decimal(10, 2)), 20, 1, 2)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (3, N'Product3', N'Third Product', N'Blue', CAST(75.00 AS Decimal(10, 2)), 15, 0, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
