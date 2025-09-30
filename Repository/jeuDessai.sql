USE LearnPlay;
GO

INSERT INTO types(nomType, descripType) VALUES
('Avatar', 'Image de profil'),
('Badge', 'Badge de réussite'),
('AppType', 'Type d’application');

INSERT INTO categories(nomCat, descripCat) VALUES
('Maths', 'Exercices de mathématiques'),
('Langues', 'Apprentissage des langues'),
('Sciences', 'Expériences et cours');

INSERT INTO niveaux(nomNiv, descripNiv) VALUES
('Débutant', 'Niveau initial'),
('Intermédiaire', 'Niveau moyen'),
('Avancé', 'Niveau confirmé');

INSERT INTO themes(nomTheme, couleurTheme, policeTheme) VALUES
('Clair', '#FFFFFF', 'Segoe UI'),
('Sombre', '#000000', 'Segoe UI'),
('Océan', '#1CA3EC', 'Roboto');

DECLARE @themeClair  INT = (SELECT idTheme FROM themes WHERE nomTheme='Clair');
DECLARE @themeSombre INT = (SELECT idTheme FROM themes WHERE nomTheme='Sombre');
DECLARE @themeOcean  INT = (SELECT idTheme FROM themes WHERE nomTheme='Océan');

/* == IMAGES == */
DECLARE @idTypeAvatar INT = (SELECT idType FROM types WHERE nomType='Avatar');
DECLARE @idTypeBadge  INT = (SELECT idType FROM types WHERE nomType='Badge');

INSERT INTO images(nomImg, cheminImg, idTypeImg) VALUES
('Avatar-Enfant', '/img/avatars/enfant.png', @idTypeAvatar),
('Avatar-Prof',   '/img/avatars/prof.png',   @idTypeAvatar),
('Badge-Or',      '/img/badges/gold.png',    @idTypeBadge);

DECLARE @imgAvatarEnfant INT = (SELECT idImg FROM images WHERE nomImg='Avatar-Enfant');
DECLARE @imgAvatarProf   INT = (SELECT idImg FROM images WHERE nomImg='Avatar-Prof');


/* == UTILISATEURS == */
INSERT INTO utilisateurs(nomUti, prenomUti, mailUti, mdpUti, dateInscription) VALUES
('Durand','Alice','alice.durand@example.com', HASHBYTES('SHA2_256','Pwd!Alice'),  GETDATE()),
('Martin','Bruno','bruno.martin@example.com', HASHBYTES('SHA2_256','Pwd!Bruno'),  DATEADD(DAY,-3,GETDATE())),
('Nguyen','Chloé','chloe.nguyen@example.com', HASHBYTES('SHA2_256','Pwd!Chloe'),  DATEADD(DAY,-10,GETDATE()));

DECLARE @utiAlice INT = (SELECT idUti FROM utilisateurs WHERE mailUti='alice.durand@example.com');
DECLARE @utiBruno INT = (SELECT idUti FROM utilisateurs WHERE mailUti='bruno.martin@example.com');
DECLARE @utiChloe INT = (SELECT idUti FROM utilisateurs WHERE mailUti='chloe.nguyen@example.com');

/* ==  PROFILS (respecte UQ sur pseudoProf) == */
INSERT INTO profils(numProf, pseudoProf, pointsProf, nivProf, dateNaissanceProf, idUtiProf, idRoleProf) VALUES
(1, 'AliceD',    120, 2, '2005-04-12', @utiAlice, @roleEleve),
(1, 'ProfBruno',   0, 3, '1984-09-01', @utiBruno, @roleAdmin),
(2, 'ChloeN',     45, 1, '2007-01-20', @utiChloe, @roleEleve);

DECLARE @profAlice INT = (SELECT idProf FROM profils WHERE pseudoProf='AliceD');
DECLARE @profBruno INT = (SELECT idProf FROM profils WHERE pseudoProf='ProfBruno');
DECLARE @profChloe INT = (SELECT idProf FROM profils WHERE pseudoProf='ChloeN');

/* == RÔLES / TYPES / CAT / NIVEAUX / THÈMES == */
INSERT INTO roles(nomRole) VALUES
('Professeur'), ('Élève'), ('Parent');

DECLARE @roleProfesseur INT = (SELECT idRole FROM roles WHERE nomRole='Professeur');
DECLARE @roleEleve INT = (SELECT idRole FROM roles WHERE nomRole='Élève');
DECLARE @roleParent INT = (SELECT idRole FROM roles WHERE nomRole='Parent');


/* ==LIAISONS PROFILS ↔ THÈMES / IMAGES*/
   
INSERT INTO profilsThemes(idThemeProf, idProfTheme, isActif) VALUES
(@themeClair,  @profAlice, 1),
(@themeSombre, @profAlice, 0),
(@themeSombre, @profBruno, 1),
(@themeOcean,  @profChloe, 1);

INSERT INTO profilsImages(idImgProf, idProfImg, isActif) VALUES
(@imgAvatarEnfant, @profAlice, 1),
(@imgAvatarProf,   @profBruno, 1);

/* == APPLICATIONS == */
DECLARE @idTypeApp INT = (SELECT idType FROM types WHERE nomType='AppType');
DECLARE @catMaths  INT = (SELECT idCat  FROM categories WHERE nomCat='Maths');
DECLARE @catLang   INT = (SELECT idCat  FROM categories WHERE nomCat='Langues');

INSERT INTO applications(titreApp, descripApp, valeurApp, matiereApp, cheminApp, idCatApp, idTypeApp) VALUES
('Tables de Multiplication', 'Exos rapides', 50, 'Maths',   '/apps/tables.exe', @catMaths, @idTypeApp),
('Vocabulaire Anglais',      'Mots du jour', 70, 'Anglais', '/apps/vocab.exe',  @catLang,  @idTypeApp);

DECLARE @appTables INT = (SELECT idApp FROM applications WHERE titreApp='Tables de Multiplication');
DECLARE @appVocab  INT = (SELECT idApp FROM applications WHERE titreApp='Vocabulaire Anglais');

/* =================*/
