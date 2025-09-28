-- 1 profil par utilisateur
-- patch de la bdd pour ajouter la contrainte
ALTER TABLE utilisateurs
  ADD idProfActif INT NULL;

ALTER TABLE utilisateurs
  ADD CONSTRAINT FK_utilisateurs_idProfActif
  FOREIGN KEY (idProfActif) REFERENCES profils(idProf)
  ON DELETE SET NULL;  -- si le profil actif est supprimé

CREATE INDEX IX_utilisateurs_idProfActif ON utilisateurs(idProfActif);
GO

-- mettre à jour le profil en vérifiant l'appartennance à l'utilisateur
CREATE OR ALTER PROCEDURE SetProfilActif
  @idUti INT,
  @idProf INT
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM profils WHERE idProf=@idProf AND idUtiProf=@idUti)
     THROW 50021, 'Profil non lié à cet utilisateur', 1;

  UPDATE utilisateurs SET idProfActif=@idProf WHERE idUti=@idUti;

  SELECT idUti, idProfActif FROM utilisateurs WHERE idUti=@idUti;
END;
GO
