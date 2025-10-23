INSERT INTO [dbo].[AspNetUsers] 
([Id], [Name], [Surname], [Address], [AccountCreationDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
(N'1', N'User1', N'Userson', N'Elm Street', '2003-12-25 00:00:00', N'TheUser1', N'User1', N'User1@gmail.com', N'User1gmail', 1, N'Password1', N'Security1', N'Concurrency1', N'111111111', 1, 1, '2025-10-14 00:00:00', 1, 2),

(N'2', N'User2', N'Usersen', N'Eln Street', '2003-12-26 00:00:00', N'TheUser2', N'User2', N'User2@gmail.com', N'User2gmail', 0, N'Password2', N'Security2', N'Concurrency2', N'222222222', 0, 0, '2025-10-15 00:00:00', 0, 3);
