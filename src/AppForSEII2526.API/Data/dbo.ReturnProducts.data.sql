SET IDENTITY_INSERT [dbo].[ReturnProducts] ON
INSERT INTO [dbo].[ReturnProducts] ([Id], [Quantity], [Reason], [ReturnPurchaseOrderId], [ProductId], [PurchaseOrderId]) VALUES (25, 20, N'First Reason', 6, 1, 2)
INSERT INTO [dbo].[ReturnProducts] ([Id], [Quantity], [Reason], [ReturnPurchaseOrderId], [ProductId], [PurchaseOrderId]) VALUES (45, 25, N'Second Reason ', 7, 3, 3)
SET IDENTITY_INSERT [dbo].[ReturnProducts] OFF
