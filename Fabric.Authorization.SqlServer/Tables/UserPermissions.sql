CREATE TABLE [dbo].[UserPermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedDateTimeUtc] [datetime2](7) NOT NULL,
	[IdentityProvider] [nvarchar](200) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedDateTimeUtc] [datetime2](7) NULL,
	[PermissionAction] [int] NOT NULL,
	[PermissionId] [uniqueidentifier] NOT NULL,
	[SubjectId] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_UserPermissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationData1]
) ON [HCFabricAuthorizationData1] TEXTIMAGE_ON [HCFabricAuthorizationData1]
GO

CREATE NONCLUSTERED INDEX [IX_UserPermissions_PermissionId] ON [dbo].[UserPermissions]
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [HCFabricAuthorizationIndex1]
GO

ALTER TABLE [dbo].[UserPermissions] ADD  DEFAULT ((0)) FOR [PermissionAction]
GO

ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Permissions_PermissionId] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([PermissionId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Permissions_PermissionId]
GO

ALTER TABLE [dbo].[UserPermissions]  WITH CHECK ADD  CONSTRAINT [FK_UserPermissions_Users_SubjectId_IdentityProvider] FOREIGN KEY([SubjectId], [IdentityProvider])
REFERENCES [dbo].[Users] ([SubjectId], [IdentityProvider])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserPermissions] CHECK CONSTRAINT [FK_UserPermissions_Users_SubjectId_IdentityProvider]
GO
