CREATE TABLE [dbo].[UsersRoles] (
    [ID]     INT IDENTITY (1, 1) NOT NULL,
    [UserID] INT NOT NULL,
    [RoleID] INT NOT NULL,
    CONSTRAINT [PK_dbo.UsersRoles] PRIMARY KEY CLUSTERED ([ID] ASC)
);

