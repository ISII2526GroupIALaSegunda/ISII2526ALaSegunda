INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [Address], [AccountCreationDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'Juan', N'García', N'Cardo Santo 2', N'2025-12-12 00:00:00', N'Juan1', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, 0, NULL, 1, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [Address], [AccountCreationDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2', N'Jaime', N'Sanchez', N'Calle inven', N'2025-11-04 00:00:00', NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, 1, 0, NULL, 0, 0)

SET IDENTITY_INSERT [dbo].[ComplaintTypes] ON
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES (5, N'Ana')
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES (3, N'Antonio')
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES (2, N'Juan')
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES (4, N'Mría')
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES (1, N'Paco')
SET IDENTITY_INSERT [dbo].[ComplaintTypes] OFF

SET IDENTITY_INSERT [dbo].[Complaints] ON
INSERT INTO [dbo].[Complaints] ([ID], [ComplaintDate], [Description], [Processed], [UserId], [TypeID]) VALUES (7, N'2025-12-02 00:00:00', N'Problema con el producto', 0, N'1', 1)
INSERT INTO [dbo].[Complaints] ([ID], [ComplaintDate], [Description], [Processed], [UserId], [TypeID]) VALUES (8, N'2025-11-04 00:00:00', N'askgflsdafhg', 0, N'2', 3)
SET IDENTITY_INSERT [dbo].[Complaints] OFF
