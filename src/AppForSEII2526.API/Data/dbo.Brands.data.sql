SET IDENTITY_INSERT [dbo].[Brands] ON
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (1, N'Location1', N'Brand1')
INSERT INTO [dbo].[Brands] ([Id], [Location], [Name]) VALUES (2, N'Location2', N'Brand2')
SET IDENTITY_INSERT [dbo].[Brands] OFF
