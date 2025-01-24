CREATE TABLE [dbo].[Accounts] (
    [code]                INT          IDENTITY (1, 1) NOT NULL,
    [person_code]         INT          NOT NULL,
    [account_number]      VARCHAR (50) NOT NULL,
    [outstanding_balance] MONEY        NOT NULL,
    [status_code]         INT          DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([code] ASC),
    CONSTRAINT [FK_Account_Person] FOREIGN KEY ([person_code]) REFERENCES [dbo].[Persons] ([code]),
    CONSTRAINT [FK_Account_Status] FOREIGN KEY ([status_code]) REFERENCES [dbo].[Status] ([code])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Account_num]
    ON [dbo].[Accounts]([account_number] ASC);

