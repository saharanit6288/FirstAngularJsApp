CREATE TABLE [dbo].[HomeBanners] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (256) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [ImagePath]   NVARCHAR (MAX) NULL,
    [Url]         NVARCHAR (MAX) NULL,
    [Sequence]    INT            NOT NULL,
    [CreatedOn]   DATETIME       NOT NULL,
    [CreatedBy]   NVARCHAR (MAX) NULL,
    [UpdatedOn]   DATETIME       NOT NULL,
    [UpdatedBy]   NVARCHAR (MAX) NULL,
    [BannerType]  NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_dbo.HomeBanners] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Title]
    ON [dbo].[HomeBanners]([Title] ASC);

