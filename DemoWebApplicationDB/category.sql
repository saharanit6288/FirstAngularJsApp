CREATE TABLE [dbo].[Categories] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (256) NOT NULL,
    [Sequence]  INT            NOT NULL,
    [CreatedOn] DATETIME       NOT NULL,
    [CreatedBy] NVARCHAR (MAX) NULL,
    [UpdatedOn] DATETIME       NOT NULL,
    [UpdatedBy] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Title]
    ON [dbo].[Categories]([Title] ASC);

