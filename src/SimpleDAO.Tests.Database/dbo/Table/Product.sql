CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(10) NOT NULL, 
	[Price] DECIMAL(10,2) NOT NULL,
    [CollectionId] INT NOT NULL, 
    CONSTRAINT [FK_Product_ToCollection] FOREIGN KEY ([CollectionId]) REFERENCES [Collection]([Id])
)
