/* ======================= VIEW: CUMULATED TIMERS ======================= */
CREATE OR ALTER VIEW vChronos AS
SELECT
  p.idProf,
  SUM(CASE WHEN t.nomType='Apprentissage' THEN par.duree ELSE 0 END) AS chronoApprentissage,
  SUM(CASE WHEN t.nomType='Jeu'           THEN par.duree ELSE 0 END) AS chronoJeu
FROM profils p
LEFT JOIN profAppRecomp par ON par.idProfil=p.idProf
LEFT JOIN applications a ON a.idApp=par.idApplication
LEFT JOIN types t ON t.idType=a.idTypeApp
GROUP BY p.idProf;
GO

/* ======================= FUNCTION: POINT RULES ======================= */
CREATE OR ALTER FUNCTION fn_PointsGain
(
  @score INT,
  @chronoJeu INT,
  @chronoAppr INT
)
RETURNS INT
AS
BEGIN
  DECLARE @gain INT = @score;               -- base
  IF @chronoJeu < @chronoAppr SET @gain = @score + 2;
  IF @chronoJeu = 0 AND @chronoAppr > 0 SET @gain = 2 * @score; -- edge case
  IF @chronoAppr >= 2 * NULLIF(@chronoJeu,0) SET @gain = 2 * @score;
  RETURN @gain;
END;
GO


/* ======================= MINIMAL SEED DATA ======================= */
INSERT INTO roles(nomRole) VALUES ('nonDetermine'),('Enfant'),('Parent'),('Eleve'),('Professeur');

INSERT INTO types(nomType,descripType) VALUES
 ('Jeu','Application de jeu'),
 ('Apprentissage','Module d''apprentissage'),
 ('Badge','Type de recompense'),
 ('Avatar','Image de profil'),
 ('Background','Image de fond');

INSERT INTO categories(nomCat,descripCat) VALUES
 ('Jeux','Jeux accessibles avec credits'),
 ('Cours','Apprentissages'),
 ('Bonus','Elements cosmetiques');

/* 1 user + 1 profil (hash bidon 0x01... pour exemple) */
INSERT INTO utilisateurs(nomUti,prenomUti,mailUti,mdpUti,dateInscription)
VALUES ('Dupont','Jean','jean.dupont@example.com', 0x0102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F20, CONVERT(date,GETDATE()));

INSERT INTO profils(numProf,pseudoProf,pointsProf,nivProf,dateNaissanceProf,idUtiProf,idRoleProf)
VALUES (1,'JDupont',100,1,'2016-05-10',1,2);

/* Themes/Images for demo */
INSERT INTO themes(nomTheme,couleurTheme,policeTheme) VALUES ('Classique','#3366ff','Arial'),('Nature','#2ecc71','Verdana');
INSERT INTO images(nomImg,cheminImg,idTypeImg) VALUES ('Avatar 1','/img/avatars/1.png', (SELECT idType FROM types WHERE nomType='Avatar'));
INSERT INTO profilsThemes(idThemeProf,idProfTheme,isActif) VALUES (1,1,1);
INSERT INTO profilsImages(idImgProf,idProfImg,isActif) VALUES (1,1,1);

/* Applications */
INSERT INTO applications(titreApp,descripApp,valeurApp,matiereApp,cheminApp,idCatApp,idTypeApp)
VALUES 
 ('PFC','Pierre-Feuille-Ciseaux (jeu)', 10, 'Jeu', '/app/pfc', (SELECT idCat FROM categories WHERE nomCat='Jeux'), (SELECT idType FROM types WHERE nomType='Jeu')),
 ('Tables x7','Tables de multiplication du 7', 2, 'Maths', '/app/tables/7', (SELECT idCat FROM categories WHERE nomCat='Cours'), (SELECT idType FROM types WHERE nomType='Apprentissage'));

INSERT INTO niveaux(nomNiv,descripNiv) VALUES ('N1','Debutant'),('N2','Intermediaire');
INSERT INTO applicationsNiveaux(idAppNiv,idNivApp) VALUES (1,1),(2,1);

/* Recompense generique "Participation" de type Badge */
INSERT INTO recompenses(nomRecomp,descripRecomp,conditions,Statut,idTypeRecomp)
VALUES ('Participation','Entree dans l''historique','Any','En cours',(SELECT idType FROM types WHERE nomType='Badge'));
GO