SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (2, N'1', N'1', N'111111111', N'000000000', N'2026-10-31 00:00:00', N'User1@gmail.com')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
