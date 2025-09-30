/*  Preparation de la création de la base de données*/
Use MASTER;
GO
DROP DATABASE IF EXISTS LearnPlay;
GO

/* Création de la base de données */
CREATE DATABASE LearnPlay;
GO
USE LearnPlay;

-- Création de la table "utilisateurs"
CREATE TABLE utilisateurs (
    idUti INT IDENTITY NOT NULL,
    nomUti VARCHAR(50),
    prenomUti VARCHAR(50),
    mailUti VARCHAR(255) NOT NULL UNIQUE,
    mdpUti VARBINARY(64) NOT NULL,
    dateInscription DATE NOT NULL
);
--La primary key
ALTER TABLE utilisateurs ADD CONSTRAINT pK_idUti PRIMARY KEY (idUti );
GO

-- Création de la table "roles"
CREATE TABLE roles (
    idRole INT IDENTITY NOT NULL,
    nomRole VARCHAR(50) NOT NULL
);
--La primary key
ALTER TABLE roles ADD CONSTRAINT pK_idRole PRIMARY KEY (idRole);
GO

-- Création de la table "profils"
CREATE TABLE profils (
    idProf INT IDENTITY NOT NULL,
    numProf INT NOT NULL,
    pseudoProf VARCHAR(50) NOT NULL,
    pointsProf INT NOT NULL,
    nivProf INT NOT NULL,
    dateNaissanceProf DATE,
    idUtiProf INT NOT NULL,
    idRoleProf INT NOT NULL
);
--La primary key
ALTER TABLE profils ADD CONSTRAINT pK_idProf PRIMARY KEY (idProf);
--clefs étrangères de “profils”
ALTER TABLE profils ADD CONSTRAINT FK_idUtiProf FOREIGN KEY (idUtiProf) REFERENCES utilisateurs(idUti) ;
ALTER TABLE profils ADD CONSTRAINT FK_idRoleProf FOREIGN KEY (idRoleProf) REFERENCES roles(idRole);
GO

-- Création de la table "types"
CREATE TABLE types (
    idType INT IDENTITY NOT NULL,
    nomType VARCHAR(20) NOT NULL,
    descripType VARCHAR(150) NOT NULL
);
--La primary key
ALTER TABLE types ADD CONSTRAINT pK_idType PRIMARY KEY (idType);
GO

-- Création de la table "themes"
CREATE TABLE themes (
    idTheme INT IDENTITY NOT NULL,
    nomTheme VARCHAR(50) NOT NULL,
    couleurTheme VARCHAR(50) NOT NULL,
    policeTheme VARCHAR(50) NOT NULL    
);
--La primary key
ALTER TABLE themes ADD CONSTRAINT pK_idTheme PRIMARY KEY (idTheme);
GO

-- Création de la table "images"
CREATE TABLE images (
    idImg INT IDENTITY NOT NULL,
    nomImg VARCHAR(50) NOT NULL,
    cheminImg VARCHAR(250) NOT NULL,
    idTypeImg INT
);
--La primary key
ALTER TABLE images ADD CONSTRAINT pK_idImg PRIMARY KEY (idImg);
--Clé etrangère de la table “images “
ALTER TABLE images ADD CONSTRAINT FK_idTypeImg FOREIGN KEY (idTypeImg) REFERENCES types(idType);
GO

-- Création de la table "profilsThemes" (table d'association entre profils et themes)
CREATE TABLE profilsThemes (
    idThemeProf INT NOT NULL,
    idProfTheme INT NOT NULL,
    isActif BIT DEFAULT(0)
);
--La primary key
ALTER TABLE  profilsThemes ADD CONSTRAINT pK_idProfilsThemes  PRIMARY KEY (idThemeProf, idProfTheme);
--clef étrangère de "profilsThemes"
ALTER TABLE profilsThemes ADD CONSTRAINT FK_idThemeProf FOREIGN KEY (idThemeProf) REFERENCES themes(idTheme);
ALTER TABLE profilsThemes ADD CONSTRAINT FK_idProfTheme FOREIGN KEY (idProfTheme) REFERENCES profils(idProf);
GO

-- Création de la table "profilsImages" (table d'association entre profils et images)
CREATE TABLE profilsImages (
    idImgProf INT NOT NULL,
    idProfImg INT NOT NULL,
    isActif BIT NOT NULL DEFAULT(0)
);
--La primary key
ALTER TABLE profilsImages ADD CONSTRAINT pK_idProfilsImages  PRIMARY KEY (idImgProf, idProfImg);
--clef étrangère "profilsImages"
ALTER TABLE profilsImages ADD CONSTRAINT FK_idImgProf FOREIGN KEY (idImgProf) REFERENCES images(idImg);
ALTER TABLE profilsImages ADD CONSTRAINT FK_idProfImg FOREIGN KEY (idProfImg) REFERENCES profils(idProf);
GO

-- Création de la table "achats"
CREATE TABLE achats (
    idAchat INT IDENTITY NOT NULL,
    nomAchat VARCHAR(50) NOT NULL,
    valeurAchat INT NOT NULL,
    descripAchat VARCHAR(100) NOT NULL,
    dateAchat DATE,
   idProfAchat INT NOT NULL
);
--La primary key
ALTER TABLE achats ADD CONSTRAINT pK_idAchat  PRIMARY KEY (idAchat);
-- Clé étrangère de la table achats
ALTER TABLE achats ADD CONSTRAINT FK_idProfAchat FOREIGN KEY (idProfAchat) REFERENCES profils(idProf);
GO

-- Création de la table "themesAchats" (table d'association entre themes et Achats)
CREATE TABLE themesAchats (
    idThemeAchat INT NOT NULL,
    idAchatTheme INT NOT NULL
);
--La primary key
ALTER TABLE themesAchats ADD CONSTRAINT pK_idThemesAchats PRIMARY KEY (idThemeAchat, idAchatTheme);
--clefs étrangères "themesAchats"
ALTER TABLE themesAchats ADD CONSTRAINT FK_idThemeAchat FOREIGN KEY (idThemeAchat) REFERENCES themes(idTheme);
ALTER TABLE themesAchats ADD CONSTRAINT FK_idAchatTheme FOREIGN KEY (idAchatTheme) REFERENCES achats(idAchat);
GO

-- Création de la table "imagesAchats" (table d'association entre images et achats)
CREATE TABLE imagesAchats (
    idImgAchat INT NOT NULL,
    idAchatImg INT NOT NULL
);
--La primary key
ALTER TABLE imagesAchats ADD CONSTRAINT pK_idImagesAchats PRIMARY KEY (idImgAchat, idAchatImg);
--clefs étrangères "imagesAchats" 
ALTER TABLE imagesAchats ADD CONSTRAINT FK_idImgAchat FOREIGN KEY (idImgAchat ) REFERENCES images(idImg);
ALTER TABLE imagesAchats ADD CONSTRAINT FK_idAchatImg FOREIGN KEY (idAchatImg ) REFERENCES achats(idAchat);
GO

-- Création de la table "categories"
CREATE TABLE categories (
    idCat INT IDENTITY NOT NULL,
    nomCat VARCHAR(20) NOT NULL,
   descripCat VARCHAR(150) NOT NULL
);
--La primary key
ALTER TABLE categories ADD CONSTRAINT pK_idCat PRIMARY KEY (idCat);
GO

-- Création de la table "applications"
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
--La primary key
ALTER TABLE applications ADD CONSTRAINT pK_idApp   PRIMARY KEY (idApp );
--clefs étrangères "applications"
ALTER TABLE applications ADD CONSTRAINT FK_idCatApp FOREIGN KEY (idCatApp) REFERENCES categories(idCat);
ALTER TABLE applications ADD CONSTRAINT FK_idTypeApp FOREIGN KEY (idTypeApp) REFERENCES types(idType);
GO

-- Création de la table "applicationsAchats" (table d'association entre applications et achats)
CREATE TABLE applicationsAchats (
    idAppAchat INT NOT NULL,
    idAchatApp INT NOT NULL
);
--La primary key
ALTER TABLE applicationsAchats ADD CONSTRAINT pK_applicationsAchats PRIMARY KEY (idAppAchat, idAchatApp);
--clefs étrangères  “applicationsAchats”
ALTER TABLE applicationsAchats ADD CONSTRAINT FK_idAppAchat FOREIGN KEY (idAppAchat) REFERENCES applications(idApp);
ALTER TABLE applicationsAchats ADD CONSTRAINT FK_idAchatApp FOREIGN KEY (idAchatApp) REFERENCES achats(idAchat);
GO

-- Création de la table "niveaux"
CREATE TABLE niveaux (
    idNiv INT IDENTITY NOT NULL,
    nomNiv VARCHAR(20) NOT NULL,
    descripNiv VARCHAR(100) NOT NULL
);
--La primary key
ALTER TABLE niveaux ADD CONSTRAINT pK_idNiv PRIMARY KEY (idNiv );
GO

-- Création de la table "applicationsNiveaux" (table d'association entre applications et niveaux)
CREATE TABLE applicationsNiveaux (
    idApp INT NOT NULL,
    idNiv INT NOT NULL
);
--La primary key
ALTER TABLE applicationsNiveaux ADD CONSTRAINT pK_idApplicationsNiveaux PRIMARY KEY (idApp, idNiv);
--clefs étrangères  "applicationsNiveaux"
ALTER TABLE applicationsNiveaux ADD CONSTRAINT FK_idApp FOREIGN KEY (idApp) REFERENCES applications(idApp);
ALTER TABLE applicationsNiveaux ADD CONSTRAINT FK_idNiv FOREIGN KEY (idNiv) REFERENCES niveaux(idNiv);
GO

-- Création de la table "recompenses"
CREATE TABLE recompenses (
    idRecomp INT IDENTITY NOT NULL,
    nomRecomp VARCHAR(20) NOT NULL,
    descripRecomp VARCHAR(250) NOT NULL,
    conditions VARCHAR(50) NOT NULL,
    Statut VARCHAR(50) NOT NULL,
    idTypeRecomp INT
);
--La primary key
ALTER TABLE recompenses ADD CONSTRAINT pK_idRecomp PRIMARY KEY (idRecomp);
--Clé étrangère de la table “recompenses “
ALTER TABLE recompenses ADD CONSTRAINT FK_idTypeRecomp FOREIGN KEY (idTypeRecomp) REFERENCES types(idType);
GO

-- Création de la table "profAppRecomp" (table d'association entre profils, récompenses et applications)
CREATE TABLE profAppRecomp (
    idProfil INT NOT NULL,
    idRecompense INT NOT NULL,
    idApplication INT NOT NULL,
    duree INT NOT NULL,
    resultat BIT NOT NULL,
    points INT NOT NULL,
    dateObtention DATETIME2 NOT NULL
);
--La primary key
ALTER TABLE profAppRecomp ADD CONSTRAINT pK_idProfAppRecomp  PRIMARY KEY (idProfil,idRecompense, idApplication);
--clefs étrangères de “profAppRecomp”
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_idProfil  FOREIGN KEY (idProfil) REFERENCES profils(idProf) ;
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_idRecompense  FOREIGN KEY (idRecompense ) REFERENCES recompenses(idRecomp);
ALTER TABLE profAppRecomp ADD CONSTRAINT FK_idApplication  FOREIGN KEY (idApplication ) REFERENCES applications(idApp);
GO
-- fin ---