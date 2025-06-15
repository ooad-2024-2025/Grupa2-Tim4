IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'00000000000000_CreateIdentitySchema', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250518210934_PrvaMigracija'
)
BEGIN
    CREATE TABLE [Korisnik] (
        [Id] int NOT NULL IDENTITY,
        [Ime] nvarchar(max) NOT NULL,
        [Prezime] nvarchar(max) NOT NULL,
        [Username] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Tip] int NOT NULL,
        CONSTRAINT [PK_Korisnik] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250518210934_PrvaMigracija'
)
BEGIN
    CREATE TABLE [Termin] (
        [Id] int NOT NULL IDENTITY,
        [Datum] date NOT NULL,
        [Vrijeme] time NOT NULL,
        [Vrsta] int NOT NULL,
        [TrenerId] int NOT NULL,
        CONSTRAINT [PK_Termin] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Termin_Korisnik_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250518210934_PrvaMigracija'
)
BEGIN
    CREATE INDEX [IX_Termin_TrenerId] ON [Termin] ([TrenerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250518210934_PrvaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250518210934_PrvaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    ALTER TABLE [Termin] DROP CONSTRAINT [FK_Termin_Korisnik_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    EXEC sp_rename N'[Termin].[Id]', N'IdTermin', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    EXEC sp_rename N'[Korisnik].[Id]', N'IdKorisnik', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE TABLE [Kupovina] (
        [IdKupovina] int NOT NULL IDENTITY,
        [DatumKupovine] date NOT NULL,
        [Artikal] nvarchar(max) NOT NULL,
        [Cijena] real NOT NULL,
        [Racun] nvarchar(max) NOT NULL,
        [IdKorisnik] int NOT NULL,
        CONSTRAINT [PK_Kupovina] PRIMARY KEY ([IdKupovina]),
        CONSTRAINT [FK_Kupovina_Korisnik_IdKorisnik] FOREIGN KEY ([IdKorisnik]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE TABLE [PlanIshrane] (
        [IdPlanishrane] int NOT NULL IDENTITY,
        [Ciljevi] int NOT NULL,
        [Plan] nvarchar(max) NOT NULL,
        [DatumGenerisanja] datetime2 NOT NULL,
        [Kilaza] float NOT NULL,
        [Godine] int NOT NULL,
        [ClanId] int NOT NULL,
        CONSTRAINT [PK_PlanIshrane] PRIMARY KEY ([IdPlanishrane]),
        CONSTRAINT [FK_PlanIshrane_Korisnik_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE TABLE [PrijavaZaZaposljavanje] (
        [IdPrijava] int NOT NULL IDENTITY,
        [Ime] nvarchar(max) NOT NULL,
        [Prezime] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [CV] nvarchar(max) NOT NULL,
        [Pregledano] bit NOT NULL,
        [KorisnikId] int NOT NULL,
        CONSTRAINT [PK_PrijavaZaZaposljavanje] PRIMARY KEY ([IdPrijava]),
        CONSTRAINT [FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE TABLE [Trening] (
        [IdTrening] int NOT NULL IDENTITY,
        [Datum] datetime2 NOT NULL,
        [Vrijeme] time NOT NULL,
        [Tip] int NOT NULL,
        [ClanId] int NOT NULL,
        [TrenerId] int NOT NULL,
        CONSTRAINT [PK_Trening] PRIMARY KEY ([IdTrening]),
        CONSTRAINT [FK_Trening_Korisnik_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Trening_Korisnik_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE INDEX [IX_Kupovina_IdKorisnik] ON [Kupovina] ([IdKorisnik]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PlanIshrane_ClanId] ON [PlanIshrane] ([ClanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE INDEX [IX_PrijavaZaZaposljavanje_KorisnikId] ON [PrijavaZaZaposljavanje] ([KorisnikId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE INDEX [IX_Trening_ClanId] ON [Trening] ([ClanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    CREATE INDEX [IX_Trening_TrenerId] ON [Trening] ([TrenerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    ALTER TABLE [Termin] ADD CONSTRAINT [FK_Termin_Korisnik_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [Korisnik] ([IdKorisnik]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250520200717_DrugaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250520200717_DrugaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrijavaZaZaposljavanje]') AND [c].[name] = N'Prezime');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [PrijavaZaZaposljavanje] ALTER COLUMN [Prezime] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrijavaZaZaposljavanje]') AND [c].[name] = N'Ime');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [PrijavaZaZaposljavanje] ALTER COLUMN [Ime] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrijavaZaZaposljavanje]') AND [c].[name] = N'CV');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [PrijavaZaZaposljavanje] ALTER COLUMN [CV] nvarchar(500) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlanIshrane]') AND [c].[name] = N'Plan');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [PlanIshrane] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [PlanIshrane] ALTER COLUMN [Plan] nvarchar(500) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Kupovina]') AND [c].[name] = N'Racun');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Kupovina] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Kupovina] ALTER COLUMN [Racun] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Kupovina]') AND [c].[name] = N'Artikal');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Kupovina] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Kupovina] ALTER COLUMN [Artikal] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Korisnik]') AND [c].[name] = N'Username');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Korisnik] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Korisnik] ALTER COLUMN [Username] nvarchar(20) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Korisnik]') AND [c].[name] = N'Prezime');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Korisnik] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Korisnik] ALTER COLUMN [Prezime] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Korisnik]') AND [c].[name] = N'Password');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Korisnik] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Korisnik] ALTER COLUMN [Password] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Korisnik]') AND [c].[name] = N'Ime');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Korisnik] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Korisnik] ALTER COLUMN [Ime] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250528181102_TrecaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250528181102_TrecaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113214_PetaMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] ADD [TipCilja] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113214_PetaMigracija'
)
BEGIN
    ALTER TABLE [Korisnik] ADD [IdentityUserId] nvarchar(450) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113214_PetaMigracija'
)
BEGIN
    CREATE INDEX [IX_Korisnik_IdentityUserId] ON [Korisnik] ([IdentityUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113214_PetaMigracija'
)
BEGIN
    ALTER TABLE [Korisnik] ADD CONSTRAINT [FK_Korisnik_AspNetUsers_IdentityUserId] FOREIGN KEY ([IdentityUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604113214_PetaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250604113214_PetaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604123449_SestaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250604123449_SestaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250604131406_SedmaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250604131406_SedmaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605112756_OsmaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250605112756_OsmaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Kupovina] DROP CONSTRAINT [FK_Kupovina_Korisnik_IdKorisnik];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] DROP CONSTRAINT [FK_PlanIshrane_Korisnik_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Termin] DROP CONSTRAINT [FK_Termin_Korisnik_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_Korisnik_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_Korisnik_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP TABLE [Korisnik];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_Kupovina_IdKorisnik] ON [Kupovina];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Kupovina]') AND [c].[name] = N'IdKorisnik');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Kupovina] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [Kupovina] DROP COLUMN [IdKorisnik];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_Trening_TrenerId] ON [Trening];
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trening]') AND [c].[name] = N'TrenerId');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Trening] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [Trening] ALTER COLUMN [TrenerId] nvarchar(450) NOT NULL;
    CREATE INDEX [IX_Trening_TrenerId] ON [Trening] ([TrenerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_Trening_ClanId] ON [Trening];
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trening]') AND [c].[name] = N'ClanId');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Trening] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [Trening] ALTER COLUMN [ClanId] nvarchar(450) NOT NULL;
    CREATE INDEX [IX_Trening_ClanId] ON [Trening] ([ClanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_Termin_TrenerId] ON [Termin];
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Termin]') AND [c].[name] = N'TrenerId');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Termin] DROP CONSTRAINT [' + @var13 + '];');
    ALTER TABLE [Termin] ALTER COLUMN [TrenerId] nvarchar(450) NOT NULL;
    CREATE INDEX [IX_Termin_TrenerId] ON [Termin] ([TrenerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_PrijavaZaZaposljavanje_KorisnikId] ON [PrijavaZaZaposljavanje];
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PrijavaZaZaposljavanje]') AND [c].[name] = N'KorisnikId');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [' + @var14 + '];');
    ALTER TABLE [PrijavaZaZaposljavanje] ALTER COLUMN [KorisnikId] nvarchar(450) NOT NULL;
    CREATE INDEX [IX_PrijavaZaZaposljavanje_KorisnikId] ON [PrijavaZaZaposljavanje] ([KorisnikId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    DROP INDEX [IX_PlanIshrane_ClanId] ON [PlanIshrane];
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlanIshrane]') AND [c].[name] = N'ClanId');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [PlanIshrane] DROP CONSTRAINT [' + @var15 + '];');
    ALTER TABLE [PlanIshrane] ALTER COLUMN [ClanId] nvarchar(450) NOT NULL;
    CREATE UNIQUE INDEX [IX_PlanIshrane_ClanId] ON [PlanIshrane] ([ClanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Kupovina] ADD [KorisnikId] nvarchar(450) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Ime] nvarchar(50) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Prezime] nvarchar(50) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Tip] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    CREATE INDEX [IX_Kupovina_KorisnikId] ON [Kupovina] ([KorisnikId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Kupovina] ADD CONSTRAINT [FK_Kupovina_AspNetUsers_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] ADD CONSTRAINT [FK_PlanIshrane_AspNetUsers_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [PrijavaZaZaposljavanje] ADD CONSTRAINT [FK_PrijavaZaZaposljavanje_AspNetUsers_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Termin] ADD CONSTRAINT [FK_Termin_AspNetUsers_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_AspNetUsers_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_AspNetUsers_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250605182048_DevetaMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250605182048_DevetaMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610152200_11taMigracija'
)
BEGIN
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlanIshrane]') AND [c].[name] = N'TipCilja');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [PlanIshrane] DROP CONSTRAINT [' + @var16 + '];');
    ALTER TABLE [PlanIshrane] DROP COLUMN [TipCilja];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610152200_11taMigracija'
)
BEGIN
    DECLARE @var17 sysname;
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlanIshrane]') AND [c].[name] = N'Plan');
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [PlanIshrane] DROP CONSTRAINT [' + @var17 + '];');
    ALTER TABLE [PlanIshrane] ALTER COLUMN [Plan] nvarchar(2000) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610152200_11taMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610152200_11taMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610155256_12taMigracija'
)
BEGIN
    CREATE TABLE [Obrok] (
        [IdObrok] int NOT NULL IDENTITY,
        [Naziv] nvarchar(100) NOT NULL,
        [Opis] nvarchar(1000) NULL,
        [BrojKalorija] int NOT NULL,
        [PlanIshraneId] int NOT NULL,
        CONSTRAINT [PK_Obrok] PRIMARY KEY ([IdObrok]),
        CONSTRAINT [FK_Obrok_PlanIshrane_PlanIshraneId] FOREIGN KEY ([PlanIshraneId]) REFERENCES [PlanIshrane] ([IdPlanishrane]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610155256_12taMigracija'
)
BEGIN
    CREATE INDEX [IX_Obrok_PlanIshraneId] ON [Obrok] ([PlanIshraneId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610155256_12taMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610155256_12taMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610164549_13taMigracija'
)
BEGIN
    DROP TABLE [Obrok];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610164549_13taMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] ADD [Visina] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610164549_13taMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610164549_13taMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610211834_DodajPrijavuNaTermin'
)
BEGIN
    CREATE TABLE [PrijaveNaTermine] (
        [Id] int NOT NULL IDENTITY,
        [ClanId] nvarchar(450) NOT NULL,
        [TerminId] int NOT NULL,
        CONSTRAINT [PK_PrijaveNaTermine] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PrijaveNaTermine_AspNetUsers_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PrijaveNaTermine_Termin_TerminId] FOREIGN KEY ([TerminId]) REFERENCES [Termin] ([IdTermin]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610211834_DodajPrijavuNaTermin'
)
BEGIN
    CREATE INDEX [IX_PrijaveNaTermine_ClanId] ON [PrijaveNaTermine] ([ClanId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610211834_DodajPrijavuNaTermin'
)
BEGIN
    CREATE INDEX [IX_PrijaveNaTermine_TerminId] ON [PrijaveNaTermine] ([TerminId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250610211834_DodajPrijavuNaTermin'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250610211834_DodajPrijavuNaTermin', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_AspNetUsers_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_AspNetUsers_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    DECLARE @var18 sysname;
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Termin]') AND [c].[name] = N'Datum');
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Termin] DROP CONSTRAINT [' + @var18 + '];');
    ALTER TABLE [Termin] ALTER COLUMN [Datum] datetime2 NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    DECLARE @var19 sysname;
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PlanIshrane]') AND [c].[name] = N'Plan');
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [PlanIshrane] DROP CONSTRAINT [' + @var19 + '];');
    ALTER TABLE [PlanIshrane] ALTER COLUMN [Plan] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_AspNetUsers_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_AspNetUsers_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613131235_15TAMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250613131235_15TAMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Kupovina] DROP CONSTRAINT [FK_Kupovina_AspNetUsers_KorisnikId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] DROP CONSTRAINT [FK_PlanIshrane_AspNetUsers_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PrijavaZaZaposljavanje] DROP CONSTRAINT [FK_PrijavaZaZaposljavanje_AspNetUsers_KorisnikId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PrijaveNaTermine] DROP CONSTRAINT [FK_PrijaveNaTermine_AspNetUsers_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Termin] DROP CONSTRAINT [FK_Termin_AspNetUsers_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_AspNetUsers_ClanId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] DROP CONSTRAINT [FK_Trening_AspNetUsers_TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    DROP INDEX [IX_Trening_TrenerId] ON [Trening];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUsers] DROP CONSTRAINT [PK_AspNetUsers];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    DECLARE @var20 sysname;
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trening]') AND [c].[name] = N'TrenerId');
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Trening] DROP CONSTRAINT [' + @var20 + '];');
    ALTER TABLE [Trening] DROP COLUMN [TrenerId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    DECLARE @var21 sysname;
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trening]') AND [c].[name] = N'Vrijeme');
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Trening] DROP CONSTRAINT [' + @var21 + '];');
    ALTER TABLE [Trening] DROP COLUMN [Vrijeme];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    EXEC sp_rename N'[AspNetUsers]', N'Korisnik';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    EXEC sp_rename N'[Trening].[Tip]', N'TerminId', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    EXEC sp_rename N'[Trening].[Datum]', N'DatumKreiranja', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD [KorisnikId] nvarchar(450) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD [Napomene] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD [PlanTreninga] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD [PoslednaIzmena] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD [Status] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Korisnik] ADD CONSTRAINT [PK_Korisnik] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    CREATE TABLE [Vezba] (
        [IdVezba] int NOT NULL IDENTITY,
        [TreningId] int NOT NULL,
        [NazivVezbe] nvarchar(100) NOT NULL,
        [Serije] int NOT NULL,
        [Ponavljanja] int NOT NULL,
        [Tezina] decimal(5,2) NULL,
        [Trajanje] time NULL,
        [Napomene] nvarchar(500) NULL,
        [Redosled] int NOT NULL,
        CONSTRAINT [PK_Vezba] PRIMARY KEY ([IdVezba]),
        CONSTRAINT [FK_Vezba_Trening_TreningId] FOREIGN KEY ([TreningId]) REFERENCES [Trening] ([IdTrening]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    CREATE INDEX [IX_Trening_KorisnikId] ON [Trening] ([KorisnikId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    CREATE INDEX [IX_Trening_TerminId] ON [Trening] ([TerminId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    CREATE INDEX [IX_Vezba_TreningId] ON [Vezba] ([TreningId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Vezba_TreningId_Redosled] ON [Vezba] ([TreningId], [Redosled]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_Korisnik_UserId] FOREIGN KEY ([UserId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_Korisnik_UserId] FOREIGN KEY ([UserId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_Korisnik_UserId] FOREIGN KEY ([UserId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ADD CONSTRAINT [FK_AspNetUserTokens_Korisnik_UserId] FOREIGN KEY ([UserId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Kupovina] ADD CONSTRAINT [FK_Kupovina_Korisnik_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PlanIshrane] ADD CONSTRAINT [FK_PlanIshrane_Korisnik_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PrijavaZaZaposljavanje] ADD CONSTRAINT [FK_PrijavaZaZaposljavanje_Korisnik_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [PrijaveNaTermine] ADD CONSTRAINT [FK_PrijaveNaTermine_Korisnik_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Termin] ADD CONSTRAINT [FK_Termin_Korisnik_TrenerId] FOREIGN KEY ([TrenerId]) REFERENCES [Korisnik] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_Korisnik_ClanId] FOREIGN KEY ([ClanId]) REFERENCES [Korisnik] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_Korisnik_KorisnikId] FOREIGN KEY ([KorisnikId]) REFERENCES [Korisnik] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    ALTER TABLE [Trening] ADD CONSTRAINT [FK_Trening_Termin_TerminId] FOREIGN KEY ([TerminId]) REFERENCES [Termin] ([IdTermin]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250613161053_16-taMigracija'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250613161053_16-taMigracija', N'8.0.16');
END;
GO

COMMIT;
GO

