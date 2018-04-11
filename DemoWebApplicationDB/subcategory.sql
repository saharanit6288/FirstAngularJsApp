CREATE TABLE [dbo].[SubCategories] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]      NVARCHAR (256) NOT NULL,
    [CategoryId] INT            NOT NULL,
    [Sequence]   INT            NOT NULL,
    [CreatedOn]  DATETIME       NOT NULL,
    [CreatedBy]  NVARCHAR (MAX) NULL,
    [UpdatedOn]  DATETIME       NOT NULL,
    [UpdatedBy]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.SubCategories] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.SubCategories_dbo.Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([ID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Title]
    ON [dbo].[SubCategories]([Title] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CategoryId]
    ON [dbo].[SubCategories]([CategoryId] ASC);

