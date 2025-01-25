CREATE TABLE [dbo].[Persons] (
    [code]      INT          IDENTITY (1, 1) NOT NULL,
    [name]      VARCHAR (50) NULL,
    [surname]   VARCHAR (50) NULL,
    [id_number] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED ([code] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Person_id]
    ON [dbo].[Persons]([id_number] ASC);

