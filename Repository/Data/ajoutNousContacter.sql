/*le contact ser plus facile en gestion par l administrateur
du site web*/
USE LearnPlay;
GO

/* ---------- TABLE MINIMALE DE CONTACT ---------- */
CREATE TABLE contacts (
  idContact   INT IDENTITY PRIMARY KEY,
  createdAt   DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
  sujet       NVARCHAR(150)  NOT NULL,
  type        NVARCHAR(20)   NOT NULL
               CHECK (type IN (N'question',N'suggestion',N'bug',N'autre')),
  message     NVARCHAR(2000) NOT NULL,

  -- facultatifs
  pseudo      NVARCHAR(50)   NULL,
  email       NVARCHAR(200)  NULL,

  -- si l’utilisateur est connecté, tu peux lier son profil (sinon NULL)
  idProf      INT            NULL
               REFERENCES profils(idProf) ON DELETE SET NULL,

  consent     BIT            NOT NULL,

  CONSTRAINT CK_contacts_email_format
    CHECK (email IS NULL OR email LIKE '%_@_%._%')
);

CREATE INDEX IX_contacts_createdAt ON contacts(createdAt);
CREATE INDEX IX_contacts_idProf    ON contacts(idProf);
GO

/* ---------- PROCÉDURE D’AJOUT ---------- */
CREATE OR ALTER PROCEDURE AjouterContact
  @sujet   NVARCHAR(150),
  @type    NVARCHAR(20),
  @message NVARCHAR(2000),
  @pseudo  NVARCHAR(50)  = NULL,
  @email   NVARCHAR(200) = NULL,
  @idProf  INT           = NULL,
  @consent BIT
AS
BEGIN
  SET NOCOUNT ON;

  IF @consent = 0
    THROW 50040, 'Consentement requis', 1;
    /*
  IF @idProf IS NOT NULL AND NOT EXISTS (SELECT 1 FROM profils WHERE idProf=@idProf)
    THROW 50041, 'Profil inexistant', 1;
    */
  INSERT INTO contacts(sujet,type,message,pseudo,email,idProf,consent)
  VALUES(@sujet,@type,@message,@pseudo,@email,@idProf,@consent);

  SELECT SCOPE_IDENTITY() AS idContactCree;
END;
GO
