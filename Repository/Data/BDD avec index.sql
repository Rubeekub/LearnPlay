USE master;
IF DB_ID('LearnPlay') IS NOT NULL DROP DATABASE LearnPlay;
GO
CREATE DATABASE LearnPlay;
GO
USE LearnPlay;
GO

/* ======================= UTILISATEURS / ROLES / PROFILS ======================= */
CREATE TABLE utilisateurs (
    idUti INT IDENTITY NOT NULL,
    nomUti VARCHAR(50),
    prenomUti VARCHAR(50),
    mailUti VARCHAR(200) NOT NULL UNIQUE,
    mdpUti VARBINARY(64) NOT NULL,             -- hash (ex: SHA-256) géré côté appli
    dateInscription DATE NOT NULL,
    CONSTRAINT CK_utilisateurs_mail_format CHECK (mailUti LIKE '%_@_%._%')
);
-- PK
ALTER TABLE utilisateurs ADD CONSTRAINT PK_Uti PRIMARY KEY (idUti);
GO

CREATE TABLE roles (
    idRole INT IDENTITY NOT NULL,
    nomRole VARCHAR(50) NOT NULL
);
-- PK
ALTER TABLE roles ADD CONSTRAINT PK_Role PRIMARY KEY (idRole);
GO

CREATE TABLE profils (
    idProf INT IDENTITY NOT NULL,
    numProf INT NOT NULL,
    pseudoProf VARCHAR(50) NOT NULL,
    pointsProf INT NOT NULL,
    nivProf INT NOT NULL,
    dateNaissanceProf DATE,
    idUtiProf INT NOT NULL,
    idRoleProf INT NOT NULL,
    CONSTRAINT UQ_pseudoProf UNIQUE (pseudoProf),
    CONSTRAINT CK_profils_points_pos CHECK (pointsProf >= 0)
);
-- PK
ALTER TABLE profils ADD CONSTRAINT PK_Prof PRIMARY KEY (idProf);
-- FK
ALTER TABLE profils ADD CONSTRAINT FK_profils_idUti FOREIGN KEY (idUtiProf) REFERENCES utilisateurs(idUti) ON DELETE CASCADE;
ALTER TABLE profils ADD CONSTRAINT FK_profils_idRole FOREIGN KEY (idRoleProf) REFERENCES roles(idRole) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_profils_idUtiProf  ON profils(idUtiProf);
CREATE INDEX IX_profils_idRoleProf ON profils(idRoleProf);
GO

/* ======================= TYPES / THEMES / IMAGES ======================= */
CREATE TABLE types (
    idType INT IDENTITY NOT NULL,
    nomType VARCHAR(20) NOT NULL,
    descripType VARCHAR(150) NOT NULL
);
-- PK
ALTER TABLE types ADD CONSTRAINT PK_Type PRIMARY KEY (idType);
GO

CREATE TABLE themes (
    idTheme INT IDENTITY NOT NULL,
    nomTheme VARCHAR(50) NOT NULL,
    couleurTheme VARCHAR(50) NOT NULL,
    policeTheme VARCHAR(50) NOT NULL
);
-- PK
ALTER TABLE themes ADD CONSTRAINT PK_Theme PRIMARY KEY (idTheme);
GO

CREATE TABLE images (
    idImg INT IDENTITY NOT NULL,
    nomImg VARCHAR(50) NOT NULL,
    cheminImg VARCHAR(250) NOT NULL,
    idTypeImg INT
);
-- PK
ALTER TABLE images ADD CONSTRAINT PK_Img PRIMARY KEY (idImg);
-- FK
ALTER TABLE images ADD CONSTRAINT FK_images_idType FOREIGN KEY (idTypeImg) REFERENCES types(idType) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_images_idTypeImg ON images(idTypeImg);
GO

/* ======================= ASSOCIATIONS PERSONNALISATION ======================= */
CREATE TABLE profilsThemes (
    idThemeProf INT NOT NULL,
    idProfTheme INT NOT NULL,
    isActif BIT NULL
);
-- PK
ALTER TABLE profilsThemes ADD CONSTRAINT PK_ProfilsThemes PRIMARY KEY (idThemeProf, idProfTheme);
-- FK
ALTER TABLE profilsThemes ADD CONSTRAINT FK_profilsThemes_idTheme FOREIGN KEY (idThemeProf) REFERENCES themes(idTheme) ON DELETE CASCADE;
ALTER TABLE profilsThemes ADD CONSTRAINT FK_profilsThemes_idProf FOREIGN KEY (idProfTheme) REFERENCES profils(idProf) ON DELETE CASCADE;
-- Index (côté profil pour les recherches)
CREATE INDEX IX_profilsThemes_idProfTheme ON profilsThemes(idProfTheme);
GO

CREATE TABLE profilsImages (
    idImgProf INT NOT NULL,
    idProfImg INT NOT NULL,
    isActif BIT
);
-- PK
ALTER TABLE profilsImages ADD CONSTRAINT PK_ProfilsImages PRIMARY KEY (idImgProf, idProfImg);
-- FK
ALTER TABLE profilsImages ADD CONSTRAINT FK_profilsImages_idImg FOREIGN KEY (idImgProf) REFERENCES images(idImg) ON DELETE CASCADE;
ALTER TABLE profilsImages ADD CONSTRAINT FK_profilsImages_idProf FOREIGN KEY (idProfImg) REFERENCES profils(idProf) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_profilsImages_idProf ON profilsImages(idProfImg);
GO

/* ======================= BOUTIQUE ======================= */
CREATE TABLE achats (
    idAchat INT IDENTITY NOT NULL,
    nomAchat VARCHAR(50) NOT NULL,
    valeurAchat INT NOT NULL,
    descripAchat VARCHAR(100) NOT NULL,
    dateAchat DATE,
    idProfAchat INT NOT NULL
);
-- PK
ALTER TABLE achats ADD CONSTRAINT PK_Achat PRIMARY KEY (idAchat);
-- FK
ALTER TABLE achats ADD CONSTRAINT FK_achats_idProf FOREIGN KEY (idProfAchat) REFERENCES profils(idProf) ON DELETE CASCADE;
-- CHECK
ALTER TABLE achats ADD CONSTRAINT CK_achats_valeur_pos CHECK (valeurAchat > 0);
-- Index
CREATE INDEX IX_achats_idProf ON achats(idProfAchat);
GO

CREATE TABLE themesAchats (
    idThemeAchat INT NOT NULL,
    idAchatTheme INT NOT NULL
);
-- PK
ALTER TABLE themesAchats ADD CONSTRAINT PK_ThemesAchats PRIMARY KEY (idThemeAchat, idAchatTheme);
-- FK
ALTER TABLE themesAchats ADD CONSTRAINT FK_themesAchats_idTheme FOREIGN KEY (idThemeAchat) REFERENCES themes(idTheme) ON DELETE CASCADE;
ALTER TABLE themesAchats ADD CONSTRAINT FK_themesAchats_idAchat FOREIGN KEY (idAchatTheme) REFERENCES achats(idAchat) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_themesAchats_idAchat ON themesAchats(idAchatTheme);
CREATE INDEX IX_themesAchats_idTheme ON themesAchats(idThemeAchat);
GO

CREATE TABLE imagesAchats (
    idImgAchat INT NOT NULL,
    idAchatImg INT NOT NULL
);
-- PK
ALTER TABLE imagesAchats ADD CONSTRAINT PK_ImagesAchats PRIMARY KEY (idImgAchat, idAchatImg);
-- FK
ALTER TABLE imagesAchats ADD CONSTRAINT FK_imagesAchats_idImg FOREIGN KEY (idImgAchat) REFERENCES images(idImg) ON DELETE CASCADE;
ALTER TABLE imagesAchats ADD CONSTRAINT FK_imagesAchats_idAchat FOREIGN KEY (idAchatImg) REFERENCES achats(idAchat) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_imagesAchats_idAchat ON imagesAchats(idAchatImg);
CREATE INDEX IX_imagesAchats_idImg   ON imagesAchats(idImgAchat);
GO

/* ======================= CATEGORIES / APPLICATIONS ======================= */
CREATE TABLE categories (
    idCat INT IDENTITY NOT NULL,
    nomCat VARCHAR(20) NOT NULL,
    descripCat VARCHAR(150) NOT NULL
);
-- PK
ALTER TABLE categories ADD CONSTRAINT PK_Cat PRIMARY KEY (idCat);
GO

CREATE TABLE applications (
    idApp INT IDENTITY NOT NULL,
    titreApp VARCHAR(50) NOT NULL,
    descripApp VARCHAR(150),
    valeurApp INT NOT NULL,
    matiereApp VARCHAR(50) NOT NULL,
    cheminApp VARCHAR(250) NOT NULL,
    idCatApp INT,
    idTypeApp INT
);
-- PK
ALTER TABLE applications ADD CONSTRAINT PK_App PRIMARY KEY (idApp);
-- FK
ALTER TABLE applications ADD CONSTRAINT FK_applications_idCat FOREIGN KEY (idCatApp) REFERENCES categories(idCat) ON DELETE CASCADE;
ALTER TABLE applications ADD CONSTRAINT FK_applications_idType FOREIGN KEY (idTypeApp) REFERENCES types(idType) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_app_idCatApp  ON applications(idCatApp);
CREATE INDEX IX_app_idTypeApp ON applications(idTypeApp);
GO

/* ======================= NIVEAUX & LIAISON APP-NIVEAU ======================= */
CREATE TABLE niveaux (
    idNiv INT IDENTITY NOT NULL,
    nomNiv VARCHAR(20) NOT NULL,
    descripNiv VARCHAR(100) NOT NULL
);
-- PK
ALTER TABLE niveaux ADD CONSTRAINT PK_Niv PRIMARY KEY (idNiv);
GO

CREATE TABLE applicationsNiveaux (
    idAppNiv INT NOT NULL,
    idNivApp INT NOT NULL
);
-- PK
ALTER TABLE applicationsNiveaux ADD CONSTRAINT PK_ApplicationsNiveaux PRIMARY KEY (idAppNiv, idNivApp);
-- FK
ALTER TABLE applicationsNiveaux ADD CONSTRAINT FK_applicationsNiveaux_idApp FOREIGN KEY (idAppNiv) REFERENCES applications(idApp) ON DELETE CASCADE;
ALTER TABLE applicationsNiveaux ADD CONSTRAINT FK_applicationsNiveaux_idNiv FOREIGN KEY (idNivApp) REFERENCES niveaux(idNiv) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_applicationsNiveaux_idApp ON applicationsNiveaux(idAppNiv);
CREATE INDEX IX_applicationsNiveaux_idNiv ON applicationsNiveaux(idNivApp);
GO

/* ======================= RECOMPENSES & HISTORIQUE ======================= */
CREATE TABLE recompenses (
    idRecomp INT IDENTITY NOT NULL,
    nomRecomp VARCHAR(20) NOT NULL,
    descripRecomp VARCHAR(250) NOT NULL,
    conditions VARCHAR(50) NOT NULL,
    Statut VARCHAR(50) NOT NULL,      -- (contrôle géré dans le code côté appli, à ta demande)
    idTypeRecomp INT
);
-- PK
ALTER TABLE recompenses ADD CONSTRAINT PK_Recomp PRIMARY KEY (idRecomp);
-- FK
ALTER TABLE recompenses ADD CONSTRAINT FK_recompenses_idType FOREIGN KEY (idTypeRecomp) REFERENCES types(idType) ON DELETE CASCADE;
GO

CREATE TABLE profAppRecomp (
    idProfil INT NOT NULL,
    idRecompense INT NOT NULL,
    idApplication INT NOT NULL,
    dateObtention DATETIME2 NOT NULL,
    duree INT NOT NULL,                        -- secondes
    resultat BIT NOT NULL,
    points INT NOT NULL,
);
-- PK (inclut dateObtention pour autoriser plusieurs entrées par trio)
ALTER TABLE profAppRecomp ADD CONSTRAINT PK_ProfAppRecomp PRIMARY KEY (idProfil, idRecompense, idApplication, dateObtention);
-- FK
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_profAppRecomp_idProfil      FOREIGN KEY (idProfil)     REFERENCES profils(idProf) ON DELETE CASCADE;
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_profAppRecomp_idRecompense  FOREIGN KEY (idRecompense) REFERENCES recompenses(idRecomp) ON DELETE CASCADE;
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_profAppRecomp_idApplication FOREIGN KEY (idApplication) REFERENCES applications(idApp) ON DELETE CASCADE;
-- Index
CREATE INDEX IX_par_idProfil ON profAppRecomp(idProfil);
CREATE INDEX IX_par_idApp    ON profAppRecomp(idApplication);
GO

-- FIN