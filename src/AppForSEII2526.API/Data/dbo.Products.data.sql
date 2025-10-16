SET IDENTITY_INSERT [dbo].[Brands] ON
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (1, N'Madrid', N'Zara')
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (2, N'Barcelona', N'Uniqlo')
SET IDENTITY_INSERT [dbo].[Brands] OFF


SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (1, N'Jacket', NULL, N'Red', CAST(20.00 AS Decimal(10, 2)), 4, 1, 1)
INSERT INTO [dbo].[Products] ([ProductId], [Name], [Description], [Colour], [Price], [Stock], [IsReturnable], [BrandId]) VALUES (2, N'Shirt', NULL, N'Blue', CAST(10.00 AS Decimal(10, 2)), 2, 0, 2)
SET IDENTITY_INSERT [dbo].[Products] OFF
