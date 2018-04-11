CREATE TABLE [dbo].[Users] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Email]        NVARCHAR (150) NOT NULL,
    [Password]     NVARCHAR (150) NOT NULL,
    [PasswordSalt] NVARCHAR (MAX) NULL,
    [FirstName]    NVARCHAR (MAX) NULL,
    [LastName]     NVARCHAR (MAX) NULL,
    [ContactNo]    NVARCHAR (MAX) NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [IsActive]     BIT            NOT NULL,
    [IPAddress]    NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       NOT NULL,
    [UserName]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Email]
    ON [dbo].[Users]([Email] ASC);

