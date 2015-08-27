CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(10) NOT NULL, 
    [CollectionId] INT NULL, 
    CONSTRAINT [FK_Product_ToTable] FOREIGN KEY ([CollectionId]) REFERENCES [Collection]([Id])
)
