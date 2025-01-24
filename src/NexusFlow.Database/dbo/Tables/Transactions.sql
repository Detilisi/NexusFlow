CREATE TABLE [dbo].[Transactions] (
    [code]             INT           IDENTITY (1, 1) NOT NULL,
    [account_code]     INT           NOT NULL,
    [transaction_date] DATETIME      NOT NULL,
    [capture_date]     DATETIME      NOT NULL,
    [amount]           MONEY         NOT NULL,
    [description]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([code] ASC),
    CONSTRAINT [FK_Transaction_Account] FOREIGN KEY ([account_code]) REFERENCES [dbo].[Accounts] ([code])
);

