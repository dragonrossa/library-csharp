/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ClanID]
      ,[Ime]
      ,[Prezime]
      ,[AdresaStanovanja]
      ,[MjestoStanovanja]
      ,[Email]
      ,[DatumRodjenja]
      ,[DatumUclanjenja]
      ,[Telefon]
  FROM [knjiznica].[dbo].[Clanovi]

  select * from Zaposlenici

  select KorisnickoIme, Lozinka from Zaposlenici where KorisnickoIme='rosana' and Lozinka='abcd123' 

  IF EXISTS (SELECT * FROM Zaposlenici WHERE KorisnickoIme ='rosana') 
BEGIN
   SELECT 1 
END
ELSE
BEGIN
    SELECT 2
END

IF EXISTS (SELECT * FROM Zaposlenici WHERE KorisnickoIme = 'rosana' AND Lozinka='abcd123')  SELECT 1 ELSE SELECT 2

SELECT c.Ime,c.Prezime, k.NaslovKnjige FROM Posudba p JOIN Clanovi c ON c.ClanID=p.ClanID JOIN Knjige k ON k.KnjigaID=p.KnjigaID

