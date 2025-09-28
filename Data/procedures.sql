/* PROCEDURES STOCKEES */
CREATE OR ALTER PROCEDURE ObtenirUtilisateurs
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM utilisateurs;
END;
GO

CREATE OR ALTER PROCEDURE ObtenirUnUtilisateur
    @idUti INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM utilisateurs WHERE idUti = @idUti;
END;
GO

CREATE OR ALTER PROCEDURE AjouterUtilisateur
    @nomUti        VARCHAR(50),
    @prenomUti     VARCHAR(50),
    @mailUti       VARCHAR(200),
    @mdpHash       VARBINARY(64),
    @dateInscription DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO utilisateurs (nomUti, prenomUti, mailUti, mdpUti, dateInscription)
    VALUES (@nomUti, @prenomUti, @mailUti, @mdpHash, @dateInscription);
    SELECT SCOPE_IDENTITY() AS idUtiCree;
END;
GO

CREATE OR ALTER PROCEDURE AjouterProfil
 @numProf INT,
 @pseudoProf VARCHAR(50),
 @pointsProf INT,
 @nivProf INT,
 @dateNaissanceProf DATE = NULL,
 @idUtiProf INT,
 @idRoleProf INT
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM utilisateurs WHERE idUti=@idUtiProf)
     THROW 50002, 'Utilisateur inexistant', 1;

  IF NOT EXISTS (SELECT 1 FROM roles WHERE idRole=@idRoleProf)
     THROW 50003, 'Role inexistant', 1;

  IF EXISTS (SELECT 1 FROM profils WHERE pseudoProf=@pseudoProf)
     THROW 50004, 'Pseudo deja utilise', 1;

  INSERT INTO profils(numProf,pseudoProf,pointsProf,nivProf,dateNaissanceProf,idUtiProf,idRoleProf)
  VALUES(@numProf,@pseudoProf,@pointsProf,@nivProf,@dateNaissanceProf,@idUtiProf,@idRoleProf);

  SELECT SCOPE_IDENTITY() AS idProfCree;
END;
GO

/* Enregistre une partie (jeu ou apprentissage) et applique les règles de points */
CREATE OR ALTER PROCEDURE EnregistrerPartie
 @idProfil INT,
 @idApplication INT,
 @duree INT,              -- seconds
 @score INT,              -- raw score (0 for pay-to-play games if you want)
 @estJeu BIT,             -- 1=Game, 0=Learning
 @forcerJeu BIT = 0       -- bypass solde check (admin)
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;
  BEGIN TRAN;

  DECLARE @points INT=0, @chronoJ INT=0, @chronoA INT=0;

  /* Verify app type */
  DECLARE @typeApp VARCHAR(20), @coutJeu INT;
  SELECT @typeApp = t.nomType, @coutJeu = a.valeurApp
  FROM applications a LEFT JOIN types t ON t.idType=a.idTypeApp
  WHERE a.idApp=@idApplication;

  IF @typeApp IS NULL THROW 50010, 'Application inconnue', 1;

  /* Pre-chronos */
  SELECT @chronoA=ISNULL(v.chronoApprentissage,0),
         @chronoJ=ISNULL(v.chronoJeu,0)
  FROM vChronos v WHERE v.idProf=@idProfil;

  IF @estJeu=1
  BEGIN
      DECLARE @solde INT; SELECT @solde=pointsProf FROM profils WHERE idProf=@idProfil;
      IF (@solde < @coutJeu) AND (@forcerJeu=0) THROW 50011, 'Solde insuffisant', 1;
      SET @points = -ISNULL(@coutJeu,0);  -- debit
  END
  ELSE
  BEGIN
      SET @points = dbo.fn_PointsGain(@score, @chronoJ, @chronoA);
      IF @score=0 SET @points = 1;        -- reward effort
  END

  INSERT INTO profAppRecomp(idProfil,idRecompense,idApplication,duree,resultat,points,dateObtention)
  VALUES (@idProfil, 1, @idApplication, @duree, IIF(@score>0,1,0), @points, SYSDATETIME());

  UPDATE profils SET pointsProf = pointsProf + @points WHERE idProf=@idProfil;

  COMMIT;

  SELECT @points AS pointsAppliques;
END;
GO
