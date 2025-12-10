-- Insertar usuarios
INSERT INTO [dbo].[AspNetUsers] (
    [Id], [Name], [Surname], [Address], [AccountCreationDate],
    [UserName], [NormalizedUserName], [Email], [NormalizedEmail],
    [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
    [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled],
    [LockoutEnd], [LockoutEnabled], [AccessFailedCount]
) VALUES 
(N'1', N'Juan', N'García', N'Cardo Santo 2', N'2025-12-12 00:00:00', N'Juan1', NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 1, 0, NULL, 1, 0),
(N'2', N'Jaime', N'Sanchez', N'Calle inven', N'2025-11-04 00:00:00', NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, 1, 0, NULL, 0, 0);

-- Insertar tipos de queja
SET IDENTITY_INSERT [dbo].[ComplaintTypes] ON;
INSERT INTO [dbo].[ComplaintTypes] ([ID], [Name]) VALUES 
(1, N'Paco'),
(2, N'Juan'),
(3, N'Antonio'),
(4, N'Mría'),
(5, N'Ana');
SET IDENTITY_INSERT [dbo].[ComplaintTypes] OFF;

-- Insertar quejas
SET IDENTITY_INSERT [dbo].[Complaints] ON;
INSERT INTO [dbo].[Complaints] ([ID], [ComplaintDate], [Description], [Processed], [UserId], [TypeID]) VALUES 
(7, N'2025-12-02 00:00:00', N'Problema con el producto', 0, N'1', 1),
(8, N'2025-11-04 00:00:00', N'askgflsdafhg', 0, N'2', 3);
SET IDENTITY_INSERT [dbo].[Complaints] OFF;

-- Insertar reporte de baneo
SET IDENTITY_INSERT [dbo].[BanReports] ON;
INSERT INTO [dbo].[BanReports] ([ID], [Reason], [DetailedDescription], [StartDate], [EndDate]) VALUES 
(1, N'Harassment', N'User repeatedly violated rules', '2025-11-14', '2025-12-14');
SET IDENTITY_INSERT [dbo].[BanReports] OFF;

-- Insertar relación entre reporte y usuario
INSERT INTO [dbo].[ReportCustomers] ([BanReportId], [CustomerId], [State], [Message]) VALUES 
(1, 1, 0, N'Your account has been banned.');