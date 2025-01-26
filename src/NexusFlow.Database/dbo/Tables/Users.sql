CREATE TABLE [dbo].[Users] (
    [code]         INT            IDENTITY (1, 1) NOT NULL,
    [email]        NVARCHAR (50)  NOT NULL,
    [passwordHash] NVARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([code] ASC),
    UNIQUE NONCLUSTERED ([email] ASC)
);

