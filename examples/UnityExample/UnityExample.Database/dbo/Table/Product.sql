CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(10) NOT NULL, 
    [CollectionId] INT NOT NULL, 
    CONSTRAINT [FK_Product_ToTable] FOREIGN KEY ([CollectionId]) REFERENCES [Collection]([Id])
)
