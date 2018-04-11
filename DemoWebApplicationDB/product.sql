CREATE TABLE [dbo].[Products] (
    [ID]            INT             IDENTITY (1, 1) NOT NULL,
    [Title]         NVARCHAR (256)  NOT NULL,
    [Description]   NVARCHAR (MAX)  NULL,
    [ImagePath]     NVARCHAR (MAX)  NULL,
    [OriginalPrice] DECIMAL (18, 2) NOT NULL,
    [OfferPrice]    DECIMAL (18, 2) NOT NULL,
    [IsOfferable]   BIT             NOT NULL,
    [Quantity]      INT             NOT NULL,
    [Rating]        DECIMAL (18, 2) NOT NULL,
    [SubCategoryId] INT             NOT NULL,
    [Sequence]      INT             NOT NULL,
    [CreatedOn]     DATETIME        NOT NULL,
    [CreatedBy]     NVARCHAR (MAX)  NULL,
    [UpdatedOn]     DATETIME        NOT NULL,
    [UpdatedBy]     NVARCHAR (MAX)  NULL,
    [CategoryId]    INT             DEFAULT ((0)) NOT NULL,
    [Discount]      DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Products] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_dbo.Products_dbo.SubCategories_SubCategoryId] FOREIGN KEY ([SubCategoryId]) REFERENCES [dbo].[SubCategories] ([ID]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Title]
    ON [dbo].[Products]([Title] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SubCategoryId]
    ON [dbo].[Products]([SubCategoryId] ASC);

