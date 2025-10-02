use  LearnPlay;
GO

CREATE TRIGGER trg_SuppressionUtilisateur
   ON  utilisateurs 
   INSTEAD OF DELETE
AS 
BEGIN
-- Supprimer les entrées dans profAppRecomp associées aux profils supprimés
DELETE FROM profAppRecomp 
WHERE idProfil IN (SELECT idProf FROM profils 
WHERE idUtiProf IN (SELECT idUti FROM deleted));

    -- Supprimer les entrées dans profilTheme associées aux profils supprimés
 DELETE FROM profilsThemes
WHERE idProfTheme IN (SELECT idProf FROM profils 
WHERE idUtiProf IN (SELECT idUti FROM deleted));
    
    -- Supprimer les entrées dans profilsImages associées aux profils supprimés
 DELETE FROM profilsImages 
WHERE idProfImg IN (SELECT idProf FROM profils 
WHERE idUtiProf IN (SELECT idUti FROM deleted));

-- Supprimer enregistrements table profils ( plusieur possible donc IN pas '=')
DELETE FROM profils 
WHERE idUtiProf IN (SELECT idUti FROM deleted);

--Supprimer les enregistrements de la table utilisateurs
DELETE FROM utilisateurs 
WHERE idUti = (SELECT idUti FROM deleted);
END;

-- Supprimer Trigger  trg_SuppressionUtilisateur
DROP TRIGGER  trg_SuppressionUtilisateur;
