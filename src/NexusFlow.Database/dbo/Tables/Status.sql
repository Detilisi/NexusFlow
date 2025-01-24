CREATE TABLE [dbo].[Status] (
    [code]        INT          IDENTITY (1, 1) NOT NULL,
    [status_type] VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([code] ASC)
);

